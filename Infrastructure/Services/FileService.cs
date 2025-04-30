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
        // Ensure the uploads folder exists
        var uploadFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        // Generate a unique file name using GUID to avoid conflicts
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(uploadFolder, fileName);

        // Save the file to the server
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        // Return the relative file path to save in the database
        return "/images/" + fileName;
    }
}
