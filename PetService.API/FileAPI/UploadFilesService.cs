using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PetService.API.FileAPI
{
    public class UploadFilesService : IUploadFilesService
    {
        private readonly HttpClient _httpClient;

        public UploadFilesService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FileAPI");
        }
        public async Task<PhotoUrlsResponse> PostAsync(List<IFormFile> photos)
        {

            try
            {
                var formData = new MultipartFormDataContent();
                foreach (var photo in photos)
                {
                    formData.Add(new StreamContent(photo.OpenReadStream()), "photos", photo.FileName);
                }
                var response = await _httpClient.PostAsync(_httpClient.BaseAddress, formData);

                // Handle the response as needed
                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    // You might want to return or process data from the response here
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PhotoUrlsResponse>(responseContent);
                }
                else
                {
                    // Handle error response
                    throw new Exception($"Error making the request to the API: {response.StatusCode}");

                }
            }
            catch (Exception e)
            {
                // Handle exceptions
                throw new Exception($"Error making the request to the API: {e.Message}");
            }
        }
    }
}