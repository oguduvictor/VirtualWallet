namespace VirtualWallet.Application.DTOs.Transaction
{
    public class TransactionAlert
    {
        public string TransactionReference { get; set; }
        public decimal AccountBalance { get; set; }
        public string Description { get; set; }
    }
}
