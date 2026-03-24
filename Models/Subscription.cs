using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SubscriptionManager.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название подписки обязательно")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите стоимость")]
        [Range(0.01, 1000000, ErrorMessage = "Стоимость должна быть от 0.01 до 1 000 000")]
        [DataType(DataType.Currency)]
        [Display(Name = "Стоимость в месяц (₽)")]
        public decimal MonthlyPrice { get; set; }

        [Required(ErrorMessage = "Укажите день списания")]
        [Range(1, 31, ErrorMessage = "День должен быть от 1 до 31")]
        [Display(Name = "День списания")]
        public int BillingDay { get; set; }

        [Display(Name = "Заметки")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Максимум 500 символов")]
        public string? Notes { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Внешний ключ
        [Required]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        // Навигационное свойство
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // Вычисляемое свойство - дни до следующего списания
        private int? _daysUntilBilling;
        
        public int DaysUntilBilling
        {
            get
            {
                // Если уже вычислили - возвращаем кэшированное значение
                if (_daysUntilBilling.HasValue)
                    return _daysUntilBilling.Value;

                try
                {
                    var today = DateTime.Today;
                    var currentYear = today.Year;
                    var currentMonth = today.Month;

                    // Проверяем, что день списания корректен
                    var billingDay = BillingDay;
                    if (billingDay < 1) billingDay = 1;

                    // Получаем количество дней в текущем месяце
                    var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                    // Корректируем день, если он больше дней в месяце
                    billingDay = Math.Min(billingDay, daysInMonth);

                    // Пробуем создать дату списания в текущем месяце
                    DateTime billingDateThisMonth;
                    try
                    {
                        billingDateThisMonth = new DateTime(currentYear, currentMonth, billingDay);
                    }
                    catch
                    {
                        // Если не получилось - берем последний день месяца
                        billingDateThisMonth = new DateTime(currentYear, currentMonth, daysInMonth);
                    }

                    // Если дата списания в этом месяце уже прошла - берем следующий месяц
                    if (billingDateThisMonth < today)
                    {
                        // Переходим на следующий месяц
                        var nextMonth = today.AddMonths(1);
                        var nextYear = nextMonth.Year;
                        var nextMonthNumber = nextMonth.Month;

                        daysInMonth = DateTime.DaysInMonth(nextYear, nextMonthNumber);
                        billingDay = Math.Min(BillingDay, daysInMonth);

                        billingDateThisMonth = new DateTime(nextYear, nextMonthNumber, billingDay);
                    }

                    _daysUntilBilling = (int)(billingDateThisMonth - today).TotalDays;
                    return _daysUntilBilling.Value;
                }
                catch
                {
                    // В случае любой ошибки возвращаем 0 (безопасное значение)
                    _daysUntilBilling = 0;
                    return 0;
                }
            }
        }
    }
}
