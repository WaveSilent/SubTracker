using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Models;

namespace SubscriptionManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Добавляем таблицы
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Добавляем начальные категории
            SeedInitialCategories(modelBuilder);

            // Настройка связей и индексов

            // Индекс для быстрого поиска по названию категории
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Устанавливаем связь между подпиской и категорией
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем удалять категорию, если есть подписки

            // Индекс для сортировки подписок по дате создания
            modelBuilder.Entity<Subscription>()
                .HasIndex(s => s.CreatedAt);

            // Индекс для поиска по дню списания
            modelBuilder.Entity<Subscription>()
                .HasIndex(s => s.BillingDay);

        }
        public static void SeedInitialCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Кино и сериалы", IconClass = "fa-solid fa-film" },
                new Category { Id = 2, Name = "Музыка", IconClass = "fa-solid fa-music" },
                new Category { Id = 3, Name = "Облачные сервисы", IconClass = "fa-solid fa-cloud" },
                new Category { Id = 4, Name = "Игры", IconClass = "fa-solid fa-gamepad" },
                new Category { Id = 5, Name = "Спорт и фитнес", IconClass = "fa-solid fa-dumbbell" },
                new Category { Id = 6, Name = "Образование", IconClass = "fa-solid fa-graduation-cap" },
                new Category { Id = 7, Name = "Другое", IconClass = "fa-solid fa-ellipsis" }
            );
        }
    }
}