using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
namespace BankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly HttpClient _httpClient;
        public UserController(IUserService userService, IHttpClientFactory httpClientFactory)
        {
            _userService = userService;
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserAddDto userDto)
        {
            try
            {
                var user = await _userService.AddUser(userDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        [HttpPost("get-answer")]
        public async Task<IActionResult> GetAnswer([FromBody] FaqRequestDto request)
        {
            var flaskUrl = "http://127.0.0.1:6003/get_answer";

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(flaskUrl, content);

            if (!response.IsSuccessStatusCode)
                return BadRequest($"{response}");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var answerObj = JsonConvert.DeserializeObject<FaqResponseDto>(jsonResponse);

            return Ok(answerObj);
        }
    }

    public class FaqRequestDto
    {
        public string Question { get; set; }
    }

    public class FaqResponseDto
    {
        public string Answer { get; set; }
        public float Confidence { get; set; }
    }

}
