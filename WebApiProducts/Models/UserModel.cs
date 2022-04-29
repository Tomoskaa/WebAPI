using System.ComponentModel.DataAnnotations;

namespace WebApiProducts.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string UserRole { get; set; }
    }
}
