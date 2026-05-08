using CloudinaryDotNet.Actions;

namespace RunGroupWebApp.Interfaces
{
    //Photo upload feature step 4: Create an interface for photo service with needed method's declaration
    public interface IPhotoService
    {
        /// <summary>
        /// Asynchronously uploads an image file and returns the result of the upload operation.
        /// </summary>
        /// <param name="file">IFromFile from asp.net allows us to send file from web request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ImageUploadResult (from cloudinary package) describing
        /// the outcome of the upload.</returns>
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);

        /// <summary>
        /// Asynchronously deletes an image identified by the specified public ID.
        /// </summary>
        /// <param name="publicId">The unique identifier of the image to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains a DeletionResult (from cloudinary package)
        /// indicating the outcome of the deletion.</returns>
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
