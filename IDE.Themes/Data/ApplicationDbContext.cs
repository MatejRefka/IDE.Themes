using IDE.Themes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Representing the connection with the DB as a 'context'. An instance of this class can be 
/// interpreted as the 'database' itself, similarly with the usage.
/// </summary>


namespace IDE.Themes.Data {

    public class ApplicationDbContext : DbContext {

        private IConfiguration config;

        //PROPERTY: Theme Colours Table
        public DbSet<UserColorDataModel> UserColorTable { get; set; }

        public ApplicationDbContext(IConfiguration config) {

            this.config = config;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(config.GetConnectionString("ApplicationDBConnection"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
        }

    }
}
