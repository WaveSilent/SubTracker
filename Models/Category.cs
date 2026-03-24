using System.ComponentModel.DataAnnotations;


namespace SubscriptionManager.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 50 символов")]
        [Display(Name = "Название категории")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Иконка (класс Font Awesome)")]
        public string IconClass { get; set; } = "fa-solid fa-tag"; 

        // Навигационное свойство для связи с подписками
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
