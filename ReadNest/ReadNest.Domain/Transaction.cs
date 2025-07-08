using ReadNest.Domain.Base;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Enums;

namespace ReadNest.Domain
{
    public class Transaction : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionStatus { get; set; } = StatusEnum.Success.ToString();
        public User User { get; set; }
        public Package Package { get; set; }
    }
}
