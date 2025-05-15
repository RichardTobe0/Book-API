using Books.model;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Books.Interface
{
    public interface IBookInterface
    {
        ICollection<Book> GetBooks();

        Book GetBook(int id);

        bool CreateBook(Book book);

        bool BookExists(int id);
        bool UpdateBook(Book book);
        bool DeleteBook(Book book);
        bool Save();
    }
}
