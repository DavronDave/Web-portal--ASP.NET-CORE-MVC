using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ReviewTag
    {
        public int ReviewId { get; set; }
        public Review? Review { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
