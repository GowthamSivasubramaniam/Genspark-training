namespace BankingApp.DTO
{
    public class AccountAddDto
    {
        public string AccountType { get; set; } = string.Empty;
        public float Balance { get; set; } = 0;
        public int UserId { get; set; }

    }
}