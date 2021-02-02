using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
    }
}
