using LibraryGradProject.Context;
using LibraryGradProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryGradProject.Repos
{
    public class BookRepository : IRepository<Book>
    {
        private List<Book> _bookColection = new List<Book>();

        public void Add(Book entity)
        {
            if (String.IsNullOrEmpty(entity.Author) || String.IsNullOrEmpty(entity.Title))
            {
                throw new ArgumentException("Book must at least have title and author");
            }

            using (var context = new LibraryContext())
            {
                context.Books.Add(entity);
                context.SaveChanges();
            }
        }

        public IEnumerable<Book> GetAll()
        {
            using (var context = new LibraryContext())
            {
                return context.Books.ToList();
            }
        }

        public Book Get(int id)
        {
            using (var context = new LibraryContext())
            {
                Book b = context.Books.Where(book => book.Id == id).SingleOrDefault();
                if (b == null)
                {
                    throw new ArgumentException("Book not found.");
                }

                return b;
            }
        }

        public void Remove(int id)
        {
            using (var context = new LibraryContext())
            {
                Book bookToRemove = Get(id);
                if (bookToRemove == null)
                {
                    throw new ArgumentException("Book not found.");
                }
                context.Books.Attach(bookToRemove);
                context.Books.Remove(bookToRemove);
                context.SaveChanges();
            }        
        }

        public void Update(int id, Book book)
        {
            using (var context = new LibraryContext())
            {
                Book result = context.Books.Where(b => b.Id == id).SingleOrDefault();

                if (result != null)
                {
                    result.Author = book.Author;
                    result.ISBN = book.ISBN;
                    result.PublishDate = book.PublishDate;
                    result.Title = book.Title;
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