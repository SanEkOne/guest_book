using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }   
        public User? User { get; set; }
        public string? Text { get; set; }
        public DateTime DateTime { get; set; }
    }
}