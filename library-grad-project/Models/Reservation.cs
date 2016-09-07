using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryGradProject.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReservationStart { get; set; }
        public string ReservationEnd { get; set; }

        public int BookId { get; set; }

        [JsonIgnore]
        public virtual Book Book { get; set; }
    }
}