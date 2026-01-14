using dj_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace dj_backend.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }
        public DbSet<FoodEntry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<FoodEntry>().HasData(
                new FoodEntry() {
                    Id = 1,
                    Title = "Pizza",
                    Date = new DateOnly(2024, 1, 15),
                    Ingredients = new List<string> { "Cheese", "Tomatoes" },
                    GotSick = false
                });
        }
    }
}
