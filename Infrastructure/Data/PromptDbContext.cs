using Chatbot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Infrastructure.Data
{
    public class PromptDbContext : DbContext
    {
        public PromptDbContext(DbContextOptions<PromptDbContext> options)
            : base(options)
        {
        }

        public DbSet<PromptLog> PromptLogs { get; set; }
        //prompt logları için DbSet tanımlandı.table ismi PromptLogs olacak.Entitiesdeki PromptLog sınıfı ile eşleştirildi.
    }
}
  