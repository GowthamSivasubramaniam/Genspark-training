namespace VSM.DTO
{
    public class RefreshRequest
    {
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}

// filepath: /Users/gowtham/Desktop/Genspark/MiniProject/VehicleServiceManagement/DTO/LogoutRequest.cs
namespace VSM.DTO
{
    public class LogoutRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}