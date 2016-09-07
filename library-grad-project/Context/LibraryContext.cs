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
            Database.SetInitializer<LibraryContext>(null);
        }
       
        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}