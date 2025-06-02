namespace BankingApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phoneno { get; set; } = string.Empty;
        public string Door_no { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public string PAN { get; set; } = string.Empty;
        public ICollection<Account>? Accounts { get; set; }

    }
        

}