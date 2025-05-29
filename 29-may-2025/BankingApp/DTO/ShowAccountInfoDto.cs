namespace BankingApp.DTO
{
    public class ShowAccountInfoDto
    {
        public string AccountNo { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public float Balance { get; set; } = 0;
        
    }
}