namespace ReadNest.Application.Models.Responses.Transaction
{
    public class GetTransactionResponse
    {
        public Guid Id { get; set; }
        public long OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionStatus { get; set; }
        public string PackageName { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
