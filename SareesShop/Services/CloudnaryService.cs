using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace SareesShop.Services
{
    public class CloudnaryService
    {
        public readonly Cloudinary _cloudinary;
        public CloudnaryService(IConfiguration config)
        {
            var acc = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "SareesShop"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Cloudinary upload failed: " + result.Error?.Message);

            return result.SecureUrl.ToString();
        }
    }
}
