using System.ComponentModel.DataAnnotations;

namespace ChitChat.Models
{
    public enum UserType
    {
        Client,
        Sender,
        Reciever,
        Admin,
    }
    public class User
    {
        [Key]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public UserType Type { get; set; } = UserType.Client;


    }
}
