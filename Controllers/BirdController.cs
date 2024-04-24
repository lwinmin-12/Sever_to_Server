using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webApi.Models;

namespace webApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private readonly string _url = "https://burma-project-ideas.vercel.app/birds";

        private BirdViewModel Change(BirdDataModel bird)
        {
            var item = new BirdViewModel
            {
                BirdId = bird.Id,
                BirdName = bird.BirdMyanmarName,
                Description = bird.Description,
                PhotoUrl = $"https://burma-project-ideas.vercel.app/{bird.ImagePath}"
            };

            return item;

        }
        [HttpGet]
        public async Task<IActionResult> GetBirds()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                List<BirdDataModel> birds = JsonConvert.DeserializeObject<List<BirdDataModel>>(json)!;
                List<BirdViewModel> lst = birds.Select(ea => Change(ea)).ToList();
                return Ok(lst);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBird(int id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{_url}/{id}");



            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                BirdDataModel birds = JsonConvert.DeserializeObject<BirdDataModel>(json)!;
                var item = Change(birds);
                return Ok(item);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}