using Books.Interface;
using Books.model;

namespace Books.Repository
{
    public class BookRepository : IBookInterface
    {
      private readonly DataContext _context;
      public BookRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Book> GetBooks()
        {
            return _context.Books.OrderBy(b => b.Id).ToList();   
        }

        public Book GetBook(int id)
        {
            return _context.Books.Where(b => b.Id == id).FirstOrDefault();
        }

        public bool CreateBook(Book book)
        {
            _context.Add(book);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateBook(Book book)
        {
            _context.Update(book);
            return Save();
        }

        public bool BookExists(int id)
        {
            return _context.Books.Any(b => b.Id == id);
        }

        public bool DeleteBook(Book book)
        {
            _context.Remove(book);
            return Save();
        }
    }
}
