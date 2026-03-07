using Microsoft.AspNetCore.Http;

namespace MyApi.BLL.Service;

public class FileService : IFileService
{
    public async Task<string?> UploadAsync(IFormFile file)
    {
        if(file != null && file.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images", fileName);
            using (var stream = File.Create(filePath))//using --> close and dispose the stream after use
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        return null;
    }

    public async Task DeleteAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch
        {
        }
    }
}
