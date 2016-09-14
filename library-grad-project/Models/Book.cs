using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace LibraryGradProject.Models
{
    public class Book
    {
        public Book ()
        {

        }

        public Book (string author, string title, string isbn, string date)
        {
            this.Author = author;
            this.Title = title;
            this.ISBN = isbn;
            this.PublishDate = date;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishDate { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}