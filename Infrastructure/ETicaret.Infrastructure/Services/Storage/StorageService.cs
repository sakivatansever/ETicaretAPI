﻿using ETicaretAPI.Application.Abstraction.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task DeleteAsync(string pathOrContainer, string fileName)
        =>await _storage.DeleteAsync(pathOrContainer, fileName);

        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);  
       

        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);
      
        public Task<List<(string fileName, string path)>> UploadAsync(string pathorContainerName, IFormFileCollection files)
            => _storage.UploadAsync(pathorContainerName, files);
       
    }
}
