using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Generated with:     dotnet aspnet-codegenerator controller -name Todo -async -api -outDir Controllers        : in cmd
    [Route("api/[controller]/[action]")] // /api/controllerName/  RoutsController (withouth Controller ) so  /api/Todo/  --  /api/routs cuz changed name
    [ApiController]
    public class Routs : ControllerBase
    {

        HttpClient client = new HttpClient();
        public class FetchOptions // Class for the pagination
        {
            public int limit { get; set; }
            public int start { get; set; }
            public string typeplace { get; set; }
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Console.WriteLine("sss");

            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> front10() // places 10
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?language_filter=en&limit=25");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> PlacebyID(string id) // place by id
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/place/" + id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> front10Events() // events 10 
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/events/?language_filter=en&limit=25");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> eventID(string id) // event by id
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/event/" + id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> front10Activities() // activities 10
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/activities/?language_filter=en&limit=25");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> activitybyID(string id) // ACtivity by Id
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/activity/" + id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> front10Eat() // places to eat 10
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES&limit=25");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        /// Get all Items Route
        [HttpGet]
        public async Task<IActionResult> allPlacesToEat() // All Places to Eat
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES&limit=20&start=0");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> allEvents() // All Events
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/events/?limit=20&start=0");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> allActivities() // All ACtivities
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/activities/?limit=20&start=0");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        public async Task<IActionResult> allPlaces() // All Events
        {


            string response = await client.GetStringAsync("http://open-api.myhelsinki.fi/v1/places/?limit=20&start=0");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        //Pagination Fetching
        [HttpPost]


        public async Task<IActionResult> pagingFetch([FromBody] FetchOptions content) // fetch places by value from body (pagination)
        {
            string response = "";
            if (content.typeplace == "placetoeat")
            {
                response = await client.GetStringAsync($" http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES&limit={content.limit}&start={content.start}");

            }
            else if (content.typeplace == "events") {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/events/?limit={content.limit}&start={content.start}");
            } else if (content.typeplace == "activities") {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/activities/?limit={content.limit}&start={content.start}");
            }
            else if (content.typeplace == "allplaces")
            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/places/?limit={content.limit}&start={content.start}");
            }



            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        ////////////
        ///
            //GEO LOCATION class for nearby items from frontend
                public class GeoLocation {
                 public string latitude { get; set; }
                public  string longitude { get; set; }
                public  string typeplace { get; set; }
            public int limit { get; set; }
            public int start { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Nearby([FromBody] GeoLocation location) // DIstance Nearby Places
        {


            //60.26429609999999,25.108021299999997    ,100&limit=
            /*string response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/places/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit=20");*/
            string response = "";
            if (location.typeplace == "placetoeat")
            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES&distance_filter={location.latitude}%2C{location.longitude}%2C100&limit=20");

            } else if (location.typeplace == "allplaces")
              
                {
                    response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/places/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit=20");

                }
            else if (location.typeplace == "activities")

            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/activities/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit=20");

            }
            else if (location.typeplace == "events")

            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/events/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit=20");

            }

            
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        //Pagination FetchingWIth NearbY Result
        [HttpPost]


        public async Task<IActionResult> NearbyFetchPagination([FromBody] GeoLocation location) // fetch places by value from body (pagination)
        {
            
            string response = "";
            if (location.typeplace == "placetoeat")
            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/places/?tags_search=RESTAURANTS%20%26%20CAFES&distance_filter={location.latitude}%2C{location.longitude}%2C100&limit={location.limit}&start={location.start}");

            }
            else if (location.typeplace == "allplaces")

            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/places/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit={location.limit}&start={location.start}");

            }
            else if (location.typeplace == "activities")

            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/activities/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit={location.limit}&start={location.start}");

            }
            else if (location.typeplace == "events")

            {
                response = await client.GetStringAsync($"http://open-api.myhelsinki.fi/v1/events/?distance_filter={location.latitude}%2C{location.longitude}%2C100&limit={location.limit}&start={location.start}");

            }


            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }


    }
}