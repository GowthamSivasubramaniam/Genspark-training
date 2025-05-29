namespace BankingApp.DTO
{
    public class WithdrawDto
    {
         public string FromAccountNo { get; set; } = string.Empty;
         public float Amount { get; set; }
    }
}