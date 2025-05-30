﻿using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class FavoriteBook : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
