using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class Items
    {
        //This is Service can be some fething here too (inside string can be a Class Model) we have to plug it in Startup.cs (configureServices)
           public async Task<string> ServiceItem() {
            return "From items.cs";
        }
    }
}
