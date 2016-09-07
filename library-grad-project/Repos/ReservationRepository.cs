using LibraryGradProject.Context;
using LibraryGradProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryGradProject.Repos
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private List<Reservation> _reservationCollection = new List<Reservation>();

        public void Add(Reservation entity)
        {
            using (var context = new LibraryContext())
            {
                List<Reservation> reservations = context.Reservations.Where(reservation => reservation.BookId == entity.BookId).ToList();

                bool validReservation = true;
                
                foreach (Reservation oldReservations in reservations)
                {
                    DateTime oldStartDate = Convert.ToDateTime(oldReservations.ReservationStart);
                    DateTime oldEndDate = Convert.ToDateTime(oldReservations.ReservationEnd);

                    DateTime newStartDate = Convert.ToDateTime(entity.ReservationStart);
                    DateTime newEndDate = Convert.ToDateTime(entity.ReservationEnd);

                    // Check if reservation is valid
                    if (!((newStartDate < oldStartDate && newEndDate < oldStartDate) || (newStartDate > oldStartDate && oldEndDate < newStartDate)))
                    {
                        validReservation = false;
                    }
                }                

                if (validReservation)
                {
                    entity.Book = context.Books.Where(book => book.Id == entity.BookId).SingleOrDefault();
                    if (entity.Book == null)
                    {
                        throw new ArgumentException("Book was not found");
                    }

                    context.Reservations.Add(entity);
                    context.SaveChanges();
                    //entity.Id = _reservationCollection.Count;
                    //_reservationCollection.Add(entity);
                }
                else
                {
                    throw new ArgumentException("Book is already reserved, or unable to make a reservation with given dates");
                }
            }
        }

        public IEnumerable<Reservation> GetAll()
        {
            using (var context = new LibraryContext())
            {
                return context.Reservations.ToList();
            }
        }

        public Reservation Get(int id)
        {
            using (var context = new LibraryContext())
            {
                Reservation r = context.Reservations.Where(reservation => reservation.Id == id).SingleOrDefault();
                if (r == null)
                {
                    throw new ArgumentException("Reservation not found.");
                }
                return r;
            }
        }

        public void Remove(int id)
        {
            using (var context = new LibraryContext())
            {
                Reservation reservationToRemove = Get(id);
                context.Reservations.Attach(reservationToRemove);
                context.Reservations.Remove(reservationToRemove);
                context.SaveChanges();
            }
        }
    }
}