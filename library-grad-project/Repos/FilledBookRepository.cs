using LibraryGradProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryGradProject.Repos
{
    public class FilledBookRepository : IRepository<Book>
    {
        private List<Book> _bookColection = new List<Book>();

        public FilledBookRepository()
        {
            Add(new Book("Justas Miknys", "Justas Programopedia", "What is ISBN?", "Today"));
            Add(new Book("Justas Miknys", "How to become genius", "What is ISBN?", "Tomorrow"));
        }

        public void Add(Book entity)
        {
            entity.Id = _bookColection.Count;
            _bookColection.Add(entity);
        }

        public IEnumerable<Book> GetAll()
        {
            return _bookColection;
        }

        public Book Get(int id)
        {
            return _bookColection.Where(book => book.Id == id).SingleOrDefault();
        }

        public void Remove(int id)
        {
            Book bookToRemove = Get(id);
            _bookColection.Remove(bookToRemove);
        }

        /*
        public void Update(int id, Book entity)
        {

        }
        */
    }
}