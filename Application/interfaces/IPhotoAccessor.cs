using Microsoft.AspNetCore.Http;
using Application.Photos;

namespace Application.interfaces
{
    public interface IPhotoAccessor
    {
        // IFormFile file do http
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}