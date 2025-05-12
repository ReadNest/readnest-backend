using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Book : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public double AvarageRating { get; set; }
        public string Description { get; set; }
        public ICollection<FavoriteBook> FavoriteBooks { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<AffiliateLink> AffiliateLinks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
