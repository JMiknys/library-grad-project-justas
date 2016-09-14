using LibraryGradProject.Context;
using LibraryGradProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryGradProject.Repos
{
    public class RatingRepository
    {
        public void Add(BookRating entity)
        {
            using (var context = new LibraryContext())
            {
                context.BookRatings.Add(entity);
                context.SaveChanges();
            }
        }

        public IEnumerable<BookRating> GetAll()
        {
            using (var context = new LibraryContext())
            {
                return context.BookRatings.ToList();
            }
        }

        public List<BookRating> Get(int id)
        {
            using (var context = new LibraryContext())
            {
                List<BookRating> list = context.BookRatings.Where(br => br.UserId == id).ToList();
                return list;
            }
        }

        public void Update(BookRating bookRating)
        {
            using (var context = new LibraryContext())
            {
                BookRating result = context.BookRatings.Where(br => br.BookId == bookRating.BookId && br.UserId == bookRating.UserId).SingleOrDefault();

                if (result != null)
                {
                    result.Rating = bookRating.Rating;
                    context.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Book not found");
                }
            }
        }
    }
}