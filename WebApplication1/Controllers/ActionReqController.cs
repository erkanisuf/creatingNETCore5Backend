using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
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
            IEnumerable<PersonModel> objList = _db.PersonModel;
            return objList;
        }
    }
    /*[Route("api/[controller]/[action]")] // when it has action can be http://localhost:5000/api/ActionReq/actiontwo othervise u cant have 2 get req.
    [ApiController]
    public class ActionReqController : ControllerBase
    {
        private readonly Items _itemServices; // Here Items comes from Items.cs ("public class Items")

        public ActionReqController(Items itemServices)
        {
             _itemServices = itemServices;
        }
        [HttpGet]
        public async Task<ActionResult<string>> ActionOne()
        {
            return "ACtion One";
        }
        [HttpGet]
        public string ActionTwo()
        {
            return "ACtion Two";
        }
    }*/
}