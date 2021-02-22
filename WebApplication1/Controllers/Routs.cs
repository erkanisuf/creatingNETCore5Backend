using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Generated with:     dotnet aspnet-codegenerator controller -name Todo -async -api -outDir Controllers        : in cmd
    [Route("api/[controller]/[action]")] // /api/controllerName/  RoutsController (withouth Controller ) so  /api/Todo/  --  /api/routs cuz changed name
    [ApiController]
    public class Routs : ControllerBase
    {
        

        [HttpGet]
        public  ActionResult<IEnumerable<string>> Get()
        {
            Console.WriteLine("sss");
            
            return Ok();

        }
        [HttpGet]
        public  async Task<string> front10() // places 10
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?language_filter=en&limit=10");
            return response;
        }
        [HttpGet("{id}")]
        public async Task<string> PlacebyID(string id) // place by id
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/place/"+id);
            return response;
        }

        [HttpGet]
        public async Task<string> front10Events() // events 10 
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/events/?language_filter=en&limit=10");
            return response;
        }
        [HttpGet("{id}")]
        public async Task<string>eventID(string id) // event by id
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/event/" + id);
            return response;
        }

        [HttpGet]
        public async Task<string> front10Activities() // activities 10
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/activities/?language_filter=en&limit=10");
            return response;
        }
        [HttpGet("{id}")]
        public async Task<string> activitybyID(string id) // ACtivity by Id
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/activity/" + id);
            return response;
        }
        [HttpGet]
        public async Task<string> front10Eat() // places to eat 10
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES&limit=10");
            return response;
        }
        /// Get all Items Route
        [HttpGet]
        public async Task<IActionResult> allPlacesToEat() // All Places to Eat
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES");
            return Ok( response);
        }

        public async Task<IActionResult> allEvents() // All Events
        {
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/events/?language_filter=en");
            return Ok(response);
        }

        ////////////

    }
}