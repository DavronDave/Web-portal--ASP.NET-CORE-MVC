using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ReviewCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Review>? Reviews { get; set; }

    }
}
