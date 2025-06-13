

namespace VSM.DTO
{

    public class UserAddDto
    {
        public string Mail { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public required byte[] Password { get; set; }
        public required byte[] Hashkey { get; set; }
    }
}