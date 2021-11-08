using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCloud.Model
{
    public class CloudContext: DbContext
    {
        private string DatabasePath= "c:\\Cloud.db";

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlite($"Filename={DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var usersBuilder = modelBuilder.Entity<User>();
            usersBuilder.HasData(new User() { Id = 1, Login = "sergey@gmail.com", Password = "111" });
        }
    }
}
