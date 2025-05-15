using AutoMapper;
using Books.DTO;
using Books.Interface;
using Books.model;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookInterface _bookInterface;
        private readonly IMapper _mapper;

        public BookController(IBookInterface bookInterface, IMapper mapper)
        {
            _bookInterface = bookInterface;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public IActionResult GetBooks()
        {
           var books = _mapper.Map<List<BookDto>>(_bookInterface.GetBooks());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(books);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]

        public IActionResult GetBook(int id)
        {
            var book = _mapper.Map<BookDto>(_bookInterface.GetBook(id));

            if (!ModelState.IsValid) return BadRequest();

            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateBook([FromBody] BookDto bookCreate)
        {
            if (bookCreate == null)
                return BadRequest(ModelState);
            var book = _bookInterface.GetBooks()
                .Where(b => b.Title.Trim().ToUpper() == bookCreate.Title.Trim().ToUpper())
                .FirstOrDefault();
            if (book != null)
            {
                ModelState.AddModelError("", "Book already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var bookMap = _mapper.Map<Book>(bookCreate);

            if (!_bookInterface.CreateBook(bookMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            } 

            return Ok("Successfully Created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(202)]
        [ProducesResponseType(204)]

        public IActionResult UpdateBook(int id, [FromBody] BookDto bookUpdate)
        {
            if(bookUpdate == null)
                return BadRequest(ModelState);

            if(id != bookUpdate.Id)
                return BadRequest(ModelState);
            if(!_bookInterface.BookExists(id))
                return NotFound();

            var book = _mapper.Map<Book>(_bookInterface.GetBooks());

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (!_bookInterface.UpdateBook(book))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteBook(int id) 
        {
            if (!_bookInterface.BookExists(id))
            {
                return NotFound();
            }

            var BooktoDelete = _mapper.Map<Book>(_bookInterface.GetBook(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookInterface.DeleteBook(BooktoDelete))
            {
                ModelState.AddModelError("", "Someting went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
