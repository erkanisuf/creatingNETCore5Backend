using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    // THIS FILE WILL BE DELETED  - used to test some CRUD
    [Route("api/[controller]/[action]")]
    public class ActionReqController : ControllerBase
    {
        private readonly PersonDBContext _db;
        public ActionReqController(PersonDBContext db) // PersonDBContext is in DATA folder
        {
           
            _db = db;
        }

       


        [HttpGet]
        public object Index()
        {
            IEnumerable<PersonModel> objList = _db.PersonModel; // http://localhost:13220/api/ActionReq/index
            return objList;
        }
        [HttpGet]
        public object Products()
        {
              IEnumerable<ProductModel> objList = _db.ProductModel;
            //http://localhost:13220/api/ActionReq/products

            return objList;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMe() { // Task is for Async , ACtion result if have multiple result eg. View ,Json,File
            var values = await _db.ProductModel.Select(c => new { c.ProductPrice,c.ProductName }).ToListAsync(); // Entity Framework thins , this selects filed
            //http://localhost:13220/api/ActionReq/getme
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get(int id)
        { // Task is for Async , ACtion result if have multiple result eg. View ,Json,File
            var values = await _db.ProductModel.FindAsync(id); // http://localhost:13220/api/ActionReq/Get/1
            if (values == null)
            {
                return NotFound();
            }
            return Ok(values);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReviewsModel product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Check all fields !");
            }

            var values = _db.ReviewsModel.Where(b => b.placeId == product.placeId).Where(b=>b.writtenBy == product.writtenBy);

            if (values.Any())
            {
                return BadRequest("Has already reviewed it!!!");
            }
            else {
                var additem = _db.ReviewsModel.Add(product);
                var result = await _db.SaveChangesAsync();
                return Ok(result);
            }


            /* "placeId":"asdf",
            "writtenBy":"dadaman",
            "rating":41,
            "comment":"eghhehee",
            "CreatedDate": "2019-01-06T17:16:40"*/ //This is the JSON From postman ,ISO 8601




        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ReviewsModel>>> GetReviewsByID(string id)
        { // Task is for Async , ACtion result if have multiple result eg. View ,Json,File
            var values = _db.ReviewsModel.Where(b => b.placeId == id)
                    ;
            


            return Ok(values);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Change(long id, [FromBody]  ProductModel product)
        {

            /*{
                "Id" :2,
            "productName": "Pineapledde",
            "productPrice": 5
              }*/ //From POstman should bne like this otherwise cant get body http://localhost:5000/api/ActionReq/Change/1/

            if (id != product.Id)
            {
                return BadRequest();
            }

            _db.Entry(product).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok("Succsess");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> delete(int id)
        { // http://localhost:5000/api/ActionReq/delete/1/
            var delitemsearch= await _db.ProductModel.FindAsync(id);
            if (delitemsearch == null)
            {
                return NotFound();
            }

            _db.ProductModel.Remove(delitemsearch);
            await _db.SaveChangesAsync();

            return Ok("deleted");
        }
    }
   
}