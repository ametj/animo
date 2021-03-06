using Animo.Web.Core;
using Microsoft.EntityFrameworkCore;

namespace Animo.Web.Data
{
    public class AnimoDbContext : BaseDbContext<AnimoDbContext>
    {
        public AnimoDbContext(DbContextOptions<AnimoDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}