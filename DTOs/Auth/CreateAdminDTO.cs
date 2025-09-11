namespace VHSKCD.DTOs.Auth
{
    public class CreateAdminDTO
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? Role { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }

        public sbyte Status { get; set; }

        public string Phonenumber { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
