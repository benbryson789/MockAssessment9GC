using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mock9.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mock9.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Search(int id)
        {
            var model = new Donut();
            if (id > 0)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(@"https://grandcircusco.github.io/");
                var response = await client.GetAsync($"demo-apis/donuts/{id}.json");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JObject>(content);
                model.Id = result["id"].ToObject<int>();
                model.Name = result["name"].ToObject<string>();
                model.Calories = result["calories"].ToObject<int>();
                if (result["photo"] != null)
                    model.PhotoURL = result["photo"].ToObject<string>();
                if (result["extras"] != null)
                    model.Extras = result["extras"].ToObject<string[]>();
            }
            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
