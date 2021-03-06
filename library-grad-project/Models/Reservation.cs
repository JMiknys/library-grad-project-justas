﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Runtime.Serialization;


namespace LibraryGradProject.Models
{
    //[JsonObject(IsReference = true)]
    public class Reservation
    {
        public Reservation ()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReservationStart { get; set; }
        public string ReservationEnd { get; set; }

        public int BookId { get; set; }

        [NotMapped]
        public int UserId { get; set; }

        //[JsonIgnore]
        public virtual User User { get; set; }

        //[JsonIgnore]
        public virtual Book Book { get; set; }
    }
}