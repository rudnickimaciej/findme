using Microsoft.AspNetCore.Mvc;

namespace PetService.API.FileAPI
{
    public interface IUploadFilesService
    {
        Task<PhotoUrlsResponse> PostAsync(List<IFormFile> photos);
    }
}