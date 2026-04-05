using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyApi.BLL.Settings;

namespace MyApi.BLL.Service;

public class FileService : IFileService
{
    private readonly Cloudinary _cloudinary;

    public FileService(IOptions<CloudinarySettings> cloudinaryConfig)
    {
        var settings = cloudinaryConfig.Value;

        var account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

public async Task<string?> UploadAsync(IFormFile file)
{
    if (file == null || file.Length == 0)
        throw new Exception("File is null or empty");

    await using var stream = file.OpenReadStream();

    var uploadParams = new ImageUploadParams
    {
        File = new FileDescription(file.FileName, stream),
        Folder = "products"
    };

    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

    if (uploadResult.Error != null)
    {
        throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");
    }

    if (string.IsNullOrWhiteSpace(uploadResult.SecureUrl?.ToString()))
    {
        throw new Exception("Cloudinary upload failed: SecureUrl is null");
    }

    return uploadResult.SecureUrl.ToString();
}
    public async Task DeleteAsync(string fileUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;

            var uri = new Uri(fileUrl);
            var segments = uri.AbsolutePath.Split('/');

            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex == -1 || uploadIndex + 2 >= segments.Length)
                return;

            var publicIdWithExtension = string.Join('/', segments.Skip(uploadIndex + 2));
            var publicId = Path.ChangeExtension(publicIdWithExtension, null);

            await _cloudinary.DestroyAsync(new DeletionParams(publicId));
        }
        catch
        {
        }
    }
}