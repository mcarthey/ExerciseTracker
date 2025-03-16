using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using ExerciseTracker.Models;

namespace ExerciseTracker.Services
{
    public class NounProjectApi
    {
        private const string BaseUrl = "https://api.thenounproject.com/v2";
        private const string ClientKey = "706bf3abd1694a44a4978a9e79296764";
        private const string ClientSecret = "6ff30e094227470cb4a3a38cdf570711";

        private readonly HttpClient _httpClient;

        public NounProjectApi()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        private string BuildOAuthHeader(string query)
        {
            // 1. Gather and format OAuth parameters
            string nonce = Guid.NewGuid().ToString("N");
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            string signatureMethod = "HMAC-SHA1";
            string version = "1.0";

            // 2. Construct the base string (method, URL, parameters) per OAuth 1.0 spec
            //    This is a simplified placeholder; you need to encode all parameters:
            string baseString = $"GET&{Uri.EscapeDataString(BaseUrl + "/icon")}&" +
                                Uri.EscapeDataString($"limit=1&query={query}&oauth_consumer_key={ClientKey}&oauth_nonce={nonce}&oauth_signature_method={signatureMethod}" +
                                                     $"&oauth_timestamp={timestamp}&oauth_version={version}");

            // 3. Sign it using HMAC-SHA1 with your consumer secret (no token for 2-legged)
            byte[] keyBytes = Encoding.UTF8.GetBytes($"{ClientSecret}&");
            byte[] messageBytes = Encoding.UTF8.GetBytes(baseString);
            using var hmac = new HMACSHA1(keyBytes);
            string signature = Convert.ToBase64String(hmac.ComputeHash(messageBytes));

            // 4. Build final OAuth header
            return "OAuth " +
                $"oauth_consumer_key=\"{ClientKey}\", " +
                $"oauth_nonce=\"{nonce}\", " +
                $"oauth_signature=\"{Uri.EscapeDataString(signature)}\", " +
                $"oauth_signature_method=\"{signatureMethod}\", " +
                $"oauth_timestamp=\"{timestamp}\", " +
                $"oauth_version=\"{version}\"";
        }

        public async Task<string> FetchExerciseIcon(string exerciseName)
        {
            var path = $"/icon?query={Uri.EscapeDataString(exerciseName)}&limit=1";

            // Create request
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            // Set Authorization header
            request.Headers.TryAddWithoutValidation("Authorization", BuildOAuthHeader(Uri.EscapeDataString(exerciseName)));

            // Send
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return "default_icon";
            }

            // Parse response
            var content = await response.Content.ReadAsStringAsync();
            var nounResponse = JsonSerializer.Deserialize<NounProjectResponse>(content);

            return nounResponse?.Icons?.FirstOrDefault()?.ThumbnailUrl ?? "default_icon";
        }
    }
}
