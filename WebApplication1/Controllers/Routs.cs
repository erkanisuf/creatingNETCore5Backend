using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    // Generated with:     dotnet aspnet-codegenerator controller -name Todo -async -api -outDir Controllers        : in cmd
    [Route("api/[controller]")] // /api/controllerName/  RoutsController (withouth Controller ) so  /api/Todo/  --  /api/routs cuz changed name
    [ApiController]
    public class Routs : ControllerBase
    {


        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string[] name = { "1222", "V321321" };
            return name;

        }

        [HttpGet("{id}")]
        public string ShowID()
        {
            return "with Id ";
        }


    }
}