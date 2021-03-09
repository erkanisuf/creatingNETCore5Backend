using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ReviewsController : Controller

    {

        private readonly PersonDBContext _db;
        public ReviewsController(PersonDBContext db) // PersonDBContext is in DATA folder
        {

            _db = db;
        }

        public class ResponseResult // sends errors in list
        {
            public bool IsSuccs { get; set; }
            public string Error { get; set; }
            public IEnumerable<object> Data { get; set; }

        }
        
        [Authorize]
        [HttpPost]
        /* "placeId":"asdf",
         "writtenBy":"dadaman",
         "rating":41,
         "comment":"eghhehee",
         "CreatedDate": "2019-01-06T17:16:40"*/ //This is the JSON From postman ,ISO 8601
        public async Task<ActionResult> PostReview([FromBody] ReviewsModel product) {
            // Checks which user is loged in from the token !
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            // 
            //Creates new model taking the values from the request but for writtenBy uses the token .
            ReviewsModel reviewToSave = new ReviewsModel { writtenBy = userId ,
                placeId=product.placeId,
                rating = product.rating,
                comment = product.comment,
                CreatedDate = product.CreatedDate
                
            };

            if (!ModelState.IsValid)
            {
                // In Case some issues with the model 
                return BadRequest(new ResponseResult { Error="Wrong Model request!",IsSuccs=false});
            }
            //This one Checks if user has already posted for same id review. Users can post only one review per item.
            var values = _db.ReviewsModel.Where(b => b.placeId == product.placeId).Where(b => b.writtenBy == userId);

            if (values.Any())
            {
                // If it has posted already sends error
                return Ok(new ResponseResult { Error = "You have already add review to this item!",IsSuccs=false });
            }
            else
            {
                //Saves newreveiw to DB
                var additem = _db.ReviewsModel.Add(reviewToSave);
                var result = await _db.SaveChangesAsync();

                return Ok(new ResponseResult {  IsSuccs = true });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReviews(string id)
        { // Task is for Async , ACtion result if have multiple result eg. View ,Json,File
            var values = _db.ReviewsModel.Where(b => b.placeId == id);
            if (values == null)
            {
                return NotFound();
            }
            return Ok(new ResponseResult { IsSuccs = true ,Data = values });
        }
    }
}
