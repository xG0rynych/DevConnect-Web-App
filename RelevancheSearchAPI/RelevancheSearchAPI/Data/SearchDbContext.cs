using Microsoft.EntityFrameworkCore;
using RelevancheSearchAPI.Models;

namespace RelevancheSearchAPI.Data
{
    public class SearchDbContext:DbContext
    {
        public SearchDbContext(DbContextOptions<SearchDbContext> opt):base(opt)
        {

        }
        public DbSet<ArticleVectors> ArticleVectors { get; set; }
        public DbSet<QuestionVectors> QuestionVectors { get; set; }
        public DbSet<ArticlesCosSim> ArticlesCosSims { get; set; }
        public DbSet<QuestionsCosSim> QuestionsCosSims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticlesCosSim>()
                .HasIndex(a => new { a.ArticleId1, a.ArticleId2 })
                .IsUnique();

            modelBuilder.Entity<QuestionsCosSim>()
                .HasIndex(a => new { a.QuestionId1, a.QuestionId2 })
                .IsUnique();
        }
    }
}
