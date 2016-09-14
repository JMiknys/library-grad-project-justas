using LibraryGradProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LibraryGradProject.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext() : base("DefaultConnection")
        {
            //Database.SetInitializer<LibraryContext>(null);
            /*
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            */
            this.Configuration.ProxyCreationEnabled = false;
        }
       
        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookRating> BookRatings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}