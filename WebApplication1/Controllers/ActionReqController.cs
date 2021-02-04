using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{

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
        public async Task<ActionResult> Post([FromBody] ProductModel product)
        {
            _db.ProductModel.Add(product);
            await _db.SaveChangesAsync();
            return Ok("Posted");
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