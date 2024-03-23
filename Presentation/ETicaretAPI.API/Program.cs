using ETicaret.Infrastructure.Filters;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;

namespace ETicaretAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddPersistenceServices ();
            builder.Services.AddCors(options=>options.AddDefaultPolicy(policy=>
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
           // policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin() herþeye izin ver
            ));

            builder.Services.AddControllers(options=>options.Filters.Add<ValidationFilter>())

                .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())

                //Defaultda varolan filteri kaldýrmak için aþaðýdaki kod eklendi Filter devreye girerek controllerin tetiklenmesini engelliyordu .
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); 
   
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}