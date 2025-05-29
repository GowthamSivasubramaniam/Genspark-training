namespace BankingApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string FromAccountNo { get; set; }
        public string ToAccountNo { get; set; }
        public float Amount { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }


    }
}