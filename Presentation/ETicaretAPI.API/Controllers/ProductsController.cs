
using ETicaretAPI.Application.Abstraction.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;

using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productwriteRepository;
        readonly private IProductReadRepository _productreadRepository;
        readonly private IWebHostEnvironment _webHostEnvironment;
     
      readonly  IFileWriteRepository _fileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;





        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productreadRepository, IWebHostEnvironment webHostEnvironment, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService)
        {
            _productreadRepository = productreadRepository;
            _productwriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;

            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
        }

        // [HttpGet]
        // public async Task Get()
        //  {
        //  await  _productwriteRepository.AddRangeAsync(new()
        //    {
        //        new() {Id=Guid.NewGuid(),Name="Product1",Price=100,CreatedDate=DateTime.UtcNow,Stock=10 },
        //          new() {Id=Guid.NewGuid(),Name="Product2",Price=200,CreatedDate=DateTime.UtcNow,Stock=20 },
        //            new() {Id=Guid.NewGuid(),Name="Product3",Price=300,CreatedDate=DateTime.UtcNow,Stock=1300 },
        //    });
        // var count=   await _productwriteRepository.SaveAsync();


        //Product p = await _productreadRepository.GetByIdAsync("c948a430-42c9-4897-8049-02b1f78be8df"); // track işlemi izleme işlemi 
        //p.Name = "Ümit";
        //await _productwriteRepository.SaveAsync();



        //await    _productwriteRepository.AddAsync(new Product() { Name = "c product", Price = 1.500F, Stock = 20, CreatedDate = DateTime.UtcNow });
        //    await _productwriteRepository.SaveAsync();


        //var customerId = Guid.NewGuid();
        //await _customerWriteRepository.AddAsync(new() { Id = customerId, Name = "tEST" });

        //await _customerWriteRepository.SaveAsync();

        //await _orderwriteRepository.AddAsync(new() { Description = "bla sad ", Adress = "İSTANBUL ,KADIKOY", CustomerId = customerId });
        //await _orderwriteRepository.AddAsync(new() { Description = "bla sad ", Adress = "İSTANBUL ,Maslak", CustomerId = customerId });
        //await _orderwriteRepository.SaveAsync();

        //Order order = await _orderReadRepository.GetByIdAsync("0c33a2af-1cd8-4744-862a-5bca76bab133");
        //order.Adress = "kUZEY";
        //await _orderwriteRepository.SaveAsync();



        // }
        // Test için
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    Product product = await _productreadRepository.GetByIdAsync(id);
        //    return Ok(product);
        //}

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            
            var totolCount = _productreadRepository.GetAll(false).Count();
            var products = _productreadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();
            return Ok(new
            {
                totolCount,
                products
            });

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {


            return Ok(await _productreadRepository.GetByIdAsync(id, false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {


            await _productwriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });

            await _productwriteRepository.SaveAsync();

            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productreadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Name = model.Name;
            product.Price = model.Price;
            await _productwriteRepository.SaveAsync();
            return Ok();



        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productwriteRepository.RemoveAsync(id); 
            await _productwriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {


            //wwwroot/resources/product-images
            //string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");

            //if (!Directory.Exists(uploadPath))
            //    Directory.CreateDirectory(uploadPath);

            //Random r = new();
            //foreach (IFormFile file in Request.Form.Files)
            //{
            //  string fullPath = Path.Combine(uploadPath,$"{ r.Next()}{Path.GetExtension(file.FileName)}");
            //   using FileStream fileStream=new(fullPath,FileMode.Create,FileAccess.Write,FileShare.None,1024*1024,false);
            //   await file.CopyToAsync(fileStream);
            //   await fileStream.FlushAsync();
            //}

            //"resource/product-images"
            ////  var datas=   await _fileService.UploadAsync("resource/files", Request.Form.Files);
            ///
            var datas = await _storageService.UploadAsync("resource/files", Request.Form.Files);
            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.path,
                Storage = _storageService.StorageName
            }).ToList()) ;
            await _productImageFileWriteRepository.SaveAsync();




            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.filename,
            //    Path = d.path,
            //    Price=new Random().Next()
            //}).ToList());
            //await _invoiceFileWriteRepository.SaveAsync();

            //await _fileWriteRepository.AddRangeAsync(datas.Select(d => new ETicaretAPI.Domain.Entities.File()
            //{
            //    FileName = d.filename,
            //    Path = d.path, 

            //}).ToList());
            //await _fileWriteRepository.SaveAsync();



            return Ok();    
        }

    }
}
