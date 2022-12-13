using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<ReviewTag>? ReviewTags { get; set; }        
        public int ReviewCategoryId { get; set; }
        public ReviewCategory? Category { get; set; }
    }
}
