using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunGroupWebApp.Helpers;
using RunGroupWebApp.Interfaces;

namespace RunGroupWebApp.Services
{
    //Photo upload feature step 5: Implement the interface in the service.
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        //IOptions - Used to retrive the instances
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            //Photo upload feature step 6: get cloud configuration from config and establish cloud connectoin.
            var account = new Account
            {
                Cloud = config.Value.CloudName,
                ApiKey = config.Value.APIKey,
                ApiSecret = config.Value.APISecret
            };

            _cloudinary = new Cloudinary(account);
        }
        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result;
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var imageUploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var imageUploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    //Cloudinary allows us to edit image to the standard while uploading
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                imageUploadResult = await _cloudinary.UploadAsync(imageUploadParams);
            }

            return imageUploadResult;
        }
    }
}
