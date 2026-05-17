using System.Net.Http.Headers;
using System.Text.Json;
using Shared.Contracts.Request.Avatar;
using Shared.Contracts.Response.Avatar;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.File
{
    public class FileService(HttpClient httpClient) : IFileService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<Result<PresignedUrlResponse?>> GetPresignedUrlAsync(PresignedUrlRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("presigned-url", request, _jsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result<PresignedUrlResponse?>.Failure(Error.New(ErrorCode.NotFound, await response.Content.ReadAsStringAsync()));
                    
                return Result<PresignedUrlResponse?>.Success(await response.Content.ReadFromJsonAsync<PresignedUrlResponse>());
            }
            catch (Exception ex)
            {
                return Result<PresignedUrlResponse?>.Failure(Error.New(ErrorCode.NotFound, $"Ошибка получения: {ex.Message}"));
            }
        }
        
        public async Task<Result<AvatarResponse>> UploadAvatarAsync(string bucketName, Stream fileStream, string contentType, string fileName, string userId)
        {
            // string BucketName, string AdditionalName, string? SubFolder

            using var formData = new MultipartFormDataContent();

            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            formData.Add(streamContent, "File", fileName);
            formData.Add(new StringContent(bucketName), "BucketName");
            formData.Add(new StringContent("Avatar"), "AdditionalName");
            formData.Add(new StringContent($"/{userId}/avatar"), "SubFolder");

            try
            {
                var response = await _httpClient.PostAsync("files", formData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<AvatarResponse>();

                return Result<AvatarResponse>.Success(result!);
            }
            catch (HttpRequestException ex)
            {
                return Result<AvatarResponse>.Failure(Error.New(ErrorCode.Upload, $"Ошибка HTTP: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<AvatarResponse>.Failure(Error.New(ErrorCode.Upload, $"Ошибка загрузки: {ex.Message}"));
            }
        }

        public async Task<string> GetLink(string key)
        {
            var encodeKey = Uri.EscapeDataString(key);

            var response = await _httpClient.GetAsync($"api/files/link?key={encodeKey}");

            return await response.Content.ReadAsStringAsync();
        }

        private readonly Dictionary<string, string> MimeMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" },
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
        };

        public string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (extension == null || !MimeMappings.TryGetValue(extension, out var mimeType))
                return "application/octet-stream";

            return mimeType;
        }
    }
}