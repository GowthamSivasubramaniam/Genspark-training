namespace BankingApp.DTO
{
    public class ShowUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phoneno { get; set; } = string.Empty;
        public string Door_no { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public string PAN { get; set; } = string.Empty;
        public List<ShowAccountInfoDto>? Accounts { get; set; }
     }

   
}