namespace BankingApp.DTO
{
    public class TransferDto
    {
        public string FromAccountNo { get; set; } = string.Empty;
         public string ToAccountNo { get; set; } = string.Empty;
         public float Amount { get; set; }
    }
}