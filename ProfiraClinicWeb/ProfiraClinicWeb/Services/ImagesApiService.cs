using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ProfiraClinicWeb.Services
{
    // DTO for parsing the JSON response from the /upload endpoint.
    // Adjust property names if your backend returns different JSON keys.
    public class UploadImageResult
    {
        public string Message { get; set; }      // e.g. "Image uploaded successfully!"
        public string FileName { get; set; }     // The GUID-based filename returned by the server
    }

    public class ImagesApiService
    {
        private readonly HttpClient _httpClient;

        // Inject the HttpClient instance (configured in Program.cs / Startup.cs)
        public ImagesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Uploads a single image stream to the backend. The backend will return a JSON payload
        /// containing a "message" and the generated "fileName" (GUID + extension).
        /// </summary>
        /// <param name="imageStream">
        /// A <see cref="Stream"/> representing the file contents (e.g. from an <see cref="IBrowserFile"/> 
        /// or FileStream).
        /// </param>
        /// <param name="originalFileName">
        /// The original filename (including extension) so that the backend can preserve the extension.
        /// </param>
        /// <returns>
        /// An <see cref="UploadImageResult"/> containing the server’s response:
        /// - Message (string)
        /// - FileName (GUID-based, sanitized on the backend)
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="imageStream"/> or <paramref name="originalFileName"/> is null.
        /// </exception>
        public async Task<UploadImageResult> UploadImageAsync(Stream imageStream, string originalFileName)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream));
            if (string.IsNullOrWhiteSpace(originalFileName))
                throw new ArgumentNullException(nameof(originalFileName));

            // Prepare MultipartFormDataContent. 
            // "file" must match the parameter name in your backend action (IFormFile file).
            using var content = new MultipartFormDataContent();

            // Create a StreamContent for the image; you could also send a byte[] if you prefer.
            var fileContent = new StreamContent(imageStream);
            // Optionally, set the content type if you know it (e.g. "image/png" or "image/jpeg").
            // If you don’t set it, the backend may infer from the extension, but setting it helps.
            var mediaType = GetMediaTypeFromFileName(originalFileName);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            // The name "file" must match your backend parameter: UploadImage(IFormFile file)
            // The second argument is the filename that the server will see (it uses Path.GetExtension to keep extension).
            content.Add(fileContent, "file", originalFileName);

            // Send a POST to "api/images/upload"
            var response = await _httpClient.PostAsync("api/images/upload", content);

            // If the status code is not 200-299, throw or return null/exception as appropriate.
            response.EnsureSuccessStatusCode();

            // Parse the JSON body into UploadImageResult
            var result = await response.Content.ReadFromJsonAsync<UploadImageResult>();
            return result;
        }

        /// <summary>
        /// Constructs the absolute URI for fetching an image by filename.
        /// You can bind this to an &lt;img src="..."&gt; in your Razor/Blazor component.
        /// 
        /// Example usage:
        /// &lt;img src="@imagesService.GetImageUri("abcd-1234.png")" /&gt;
        /// </summary>
        /// <param name="fileName">
        /// The sanitized, GUID-based filename returned by the server (e.g. "e8fb3a27-4c9d-4f2a-bd8f-5f1e7d9abcde.jpg").
        /// </param>
        /// <returns>
        /// A string URI, e.g. "https://your-api-base/api/images/{fileName}"
        /// </returns>
        public string GetImageUri(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            // Ensure BaseAddress ends with a slash, or combine carefully.
            // If BaseAddress is "https://localhost:5001/" then this yields 
            // "https://localhost:5001/api/images/{fileName}".
            return new Uri(_httpClient.BaseAddress!, $"api/images/{Uri.EscapeDataString(fileName)}").ToString();
        }

        /// <summary>
        /// (Optional) If you need the raw bytes (e.g. to convert to a data URL or process in code),
        /// you can fetch the image as a byte array. Note: this does not set any MIME type headers;
        /// it simply retrieves the raw response bytes.
        /// </summary>
        /// <param name="fileName">
        /// The sanitized filename to retrieve (e.g. "e8fb3a27-4c9d-4f2a-bd8f-5f1e7d9abcde.jpg").
        /// </param>
        /// <returns>
        /// A byte[] containing the image data, or null if not found (404).
        /// </returns>
        public async Task<byte[]> GetImageBytesAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            // Issue a GET to "api/images/{fileName}"
            var response = await _httpClient.GetAsync($"api/images/{Uri.EscapeDataString(fileName)}");
            if (!response.IsSuccessStatusCode)
            {
                // You could throw here, or return null/empty array, depending on your error handling strategy.
                return null;
            }

            // Read the raw bytes
            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            return imageBytes;
        }

        /// <summary>
        /// Helper to guess MIME type from extension. The server does the same,
        /// but it’s helpful to set ContentType on upload so the backend sees it correctly.
        /// </summary>
        private static string GetMediaTypeFromFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
