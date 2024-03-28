using KyecloackNet6.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KyecloackNet6.Models;

namespace KyecloackNet6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [HttpPost]
        public async Task<IActionResult> GetAccessToken([FromBody] TokenRequestModel model)
        {
           
            try
            {
                var client = _httpClientFactory.CreateClient();
                var tokenEndpoint = "http://localhost:8080/realms/Omar/protocol/openid-connect/token";
                
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", Securite.password),
                    new KeyValuePair<string, string>("client_id", Securite.ClientId),
                   new KeyValuePair<string, string>("client_secret", Securite.ClientSecret),
                    new KeyValuePair<string, string>("username", model.Username?? ""),
                    new KeyValuePair<string, string>("password", model.Password?? "")
                });

                var response = await client.PostAsync(tokenEndpoint, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var accessToken = await response.Content.ReadAsStringAsync();
                    return Ok(accessToken);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to get access token. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving access token: {ex.Message}");
            }
        }

    }
}
