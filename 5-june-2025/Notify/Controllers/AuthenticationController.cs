
using Notify.Interfaces;
using Notify.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Notify.Services;
using System.Text.Json;
using Google.Apis.Auth;
namespace Notify.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly Interfaces.IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }
        [HttpPost]
       
        public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            try
            {
            var result = await _authenticationService.Login(loginRequest);
            return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Unauthorized(e.Message);
            }
        }
    //     [HttpGet("auth/google")]
    //     public IActionResult GoogleLogin()
    //     {
    //         var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
    //         "?response_type=code" +
    //         "&client_id=872285693484-2731ss6rpkalb382lp8lqc3q8qih9f0r.apps.googleusercontent.com" +
    //         "&redirect_uri=http://localhost:5297/auth/google/callback" +
    //         "&scope=openid%20email%20profile" +
    //         "&access_type=offline";
           
    //         return Ok(new { redirectUrl });
    //     }

    //     [HttpGet("auth/google/callback")]
    //     public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    //     {
    //         // Exchange auth code for Google token
    //         var client = new HttpClient();

    //         var tokenRequest = new Dictionary<string, string>
    //      {
    // {"code", code},
    // {"client_id", "872285693484-2731ss6rpkalb382lp8lqc3q8qih9f0r.apps.googleusercontent.com"},
    
    // {"redirect_uri", "http://localhost:5297/auth/google/callback"},
    // {"grant_type", "authorization_code"}
    //      };

    //         var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
    //         {
    //             Content = new FormUrlEncodedContent(tokenRequest)
    //         };

    //         var response = await client.SendAsync(request);
    //         var content = await response.Content.ReadAsStringAsync();

    //         var tokenData = JsonSerializer.Deserialize<JsonElement>(content);
    //         var idToken = tokenData.GetProperty("id_token").GetString();

    //         // Validate id_token
    //         var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

    //         var result = await _authenticationService.GoogleLogin(payload.Email);
    //         return Ok(result);




        }
    }
