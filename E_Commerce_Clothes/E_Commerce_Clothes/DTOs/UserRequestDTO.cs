using E_Commerce_Clothes.Models;

namespace E_Commerce_Clothes.DTOs
{
    public class UserRequestDTO
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public string Password { get; set; } = null!;

        //public byte[]? PasswordHash { get; set; }

        //public byte[]? PasswordSalt { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

    }
}
