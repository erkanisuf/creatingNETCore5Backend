using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ReviewsModel
    {

        [Key]
        public int Id { get; set; }
        public string placeId { get; set; }

        [Required(ErrorMessage = "Please add rating!")]
        public int rating { get; set; }
        public string comment { get; set; }
        [Required(ErrorMessage = "User email must be added!")]
        public string writtenBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    
}
