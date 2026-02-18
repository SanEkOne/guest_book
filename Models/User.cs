using System.ComponentModel.DataAnnotations;

namespace mvc
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [MinLength(3, ErrorMessage = "Логин должен быть не менее 3 символов.")]
        [MaxLength(20, ErrorMessage = "Логин не должен превышать 20 символов.")]
        public string? Login { get; set; }
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
        [MaxLength(50, ErrorMessage = "Пароль не должен превышать 50 символов.")]
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин может содержать только буквы и цифры.")]
        public string? Password { get; set; }

        public List<Message> Messages { get; set; } = new();
    }
}
