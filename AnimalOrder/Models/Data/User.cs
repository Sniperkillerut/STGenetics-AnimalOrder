using System.ComponentModel.DataAnnotations;

namespace AnimalOrder.Models.Data
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
