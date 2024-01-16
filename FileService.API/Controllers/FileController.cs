using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FileService.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileService.API.Controllers
{

    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileController(Cloudinary cloudinary, IWebHostEnvironment hostingEnvironment)
        {
            _cloudinary = cloudinary;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles()
        {
            var photoUrls = new List<string>();
            var contentRootPath = _hostingEnvironment.ContentRootPath;

            // Always upload the same two images to Cloudinary
            var staticImagePaths = new List<string>
            {
            Path.Combine(contentRootPath, "StaticImages", "2.png"),
            };

            foreach (var imagePath in staticImagePaths)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imagePath),
                    Folder = "pets", // Set your desired folder in Cloudinary
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return BadRequest($"Error uploading file: {uploadResult.Error.Message}");
                }

                // Add the uploaded photo URL to the list
                photoUrls.Add(uploadResult.Url.ToString());
            }

            // Optionally, you can return the list of photo URLs
            return Ok(new UplaodFilesResponseModel() { PhotoUrls = photoUrls });
        }

        // [HttpPost]
        // public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        // {
        //     if (files == null || files.Count == 0)
        //         return BadRequest("No files uploaded");

        //     var photoUrls = new List<string>();

        //     foreach (var file in files)
        //     {
        //         var uploadParams = new ImageUploadParams
        //         {
        //             File = new FileDescription(file.FileName, file.OpenReadStream()),
        //             Folder = "pets", // Set your desired folder in Cloudinary
        //         };

        //         var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        //         if (uploadResult.Error != null)
        //         {
        //             return BadRequest($"Error uploading file: {uploadResult.Error.Message}");
        //         }

        //         // Add the uploaded photo URL to the list
        //         photoUrls.Add(uploadResult.Url.ToString());
        //     }

        //     // Optionally, you can return the list of photo URLs
        //     return Ok(new UplaodFilesResponseModel() { PhotoUrls = photoUrls });
        // }

        [HttpDelete]
        [Route("{publicId}")]
        public async Task<IActionResult> DeleteFile(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            try
            {
                var result = await _cloudinary.DestroyAsync(deleteParams);

                if (result.Result == "ok")
                {
                    return Ok($"File '{publicId}' deleted successfully.");
                }
                else
                {
                    return NotFound($"File '{publicId}' not found.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(500, $"Error deleting file: {ex.Message}");
            }
        }

        [HttpGet("download/{publicId}")]
        public IActionResult DownloadFile(string publicId)
        {
            // Cloudinary serves files directly via their URLs, so there's no need to download the file
            // You can generate a URL to the file and redirect the client or use it in an <img> tag, etc.
            var fileUrl = _cloudinary.Api.UrlImgUp.BuildUrl(publicId);

            return Redirect(fileUrl);
        }
    }
}