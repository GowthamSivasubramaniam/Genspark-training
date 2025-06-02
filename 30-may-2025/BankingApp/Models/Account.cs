

namespace BankingApp.Models
{
    public class Account
    {
        public string AccountNo { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public float Balance { get; set; } = 0;
        public int UserId { get; set; }
         public User? user { get; set; }
        public ICollection<Transaction>? SentTransactions { get; set; }
        public ICollection<Transaction>? ReceivedTransactions { get; set; }
        
    }
}