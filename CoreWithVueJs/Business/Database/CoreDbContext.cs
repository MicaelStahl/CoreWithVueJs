using CoreWithVueJs.Models.Interfaces.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreWithVueJs.Business.Database
{
    public class CoreDbContext : IdentityDbContext
    {
        private static CoreDbContext _default;

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            :base (options)
        {
        }

        public DbSet<IPost> Posts { get; set; }
        // In case I make it possible to search for specific comments later on.
        public DbSet<IComment> Comments { get; set; }

        /// <summary>
        /// The Default instance of <see cref="CoreDbContext"/> created using default settings used in <see cref="Startup"/>
        /// </summary>
        public static CoreDbContext Default => _default ?? CreateDefaultContext();

        private static CoreDbContext CreateDefaultContext()
        {
            const string CONNECTIONSTRING = "Server=(localdb)\\MSSQLLocalDB;Database=CoreDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            var type = typeof(CoreDbContext);
            DbContextOptionsBuilder<CoreDbContext> options = new DbContextOptionsBuilder<CoreDbContext>();

            options.UseSqlServer(CONNECTIONSTRING);

            _default = Activator.CreateInstance(type, options.Options) as CoreDbContext;

            return _default;
        }
    }
}
