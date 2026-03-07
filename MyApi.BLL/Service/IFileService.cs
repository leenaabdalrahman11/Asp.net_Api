using System;
using Microsoft.AspNetCore.Http;

namespace MyApi.BLL.Service;

    public interface IFileService      
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string url);
    }