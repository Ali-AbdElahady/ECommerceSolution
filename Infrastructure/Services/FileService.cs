// Infrastructure/Services/FileService.cs
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Application.Common.Interfaces;

public class FileService : IFileService
{
    private readonly IHostEnvironment _hostEnvironment;

    public FileService(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async Task<string> SaveImageAsync(IFormFile image)
    {
        
        if (image == null || image.Length == 0)
            throw new ArgumentException("No file uploaded.");

        
        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
        if (!allowedContentTypes.Contains(image.ContentType.ToLower()))
            throw new ArgumentException("Invalid file type. Only image files are allowed.");

        
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        var fileExtension = Path.GetExtension(image.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
            throw new ArgumentException("Invalid file extension. Only image files are allowed.");

        
        var uploadFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        
        var fileName = Guid.NewGuid().ToString() + fileExtension;
        var filePath = Path.Combine(uploadFolder, fileName);

        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        
        return "/images/" + fileName;
    }

}
