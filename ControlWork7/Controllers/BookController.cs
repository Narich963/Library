using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlWork7.Models;
using ControlWork7.ViewModels;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Reflection.Metadata.BlobBuilder;

namespace ControlWork7.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryContext _context;

        public BookController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index(int page = 1)
        {
            List<Book> books = await _context.Books.Include(b => b.Category).Include(b => b.UsersAndBooks).ThenInclude(b => b.User).ToListAsync();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            ViewBag.User = user;

            int pagesize = 2;
            var count = books.Count();
            var items = books.Skip((page - 1) * pagesize).Take(pagesize).ToList();


            PageViewModel pageViewModel = new PageViewModel(count, page, pagesize);
            IndexViewModel viewModel = new()
            {
                PageViewModel = pageViewModel,
                Books = items
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Cabinet(string email, int page=1)
        {
            User? user = await _context.Users.Include(u => u.UsersAndBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                var uab = user.UsersAndBooks.Where(u => u.ReturnBookDate == null).ToList();
                List<Book> books = new();
                foreach (var b in uab)
                {
                    books.Add(b.Book);
                }

                ViewBag.User = user;

                int pagesize = 3;
                var count = books.Count();
                var items = books.Skip((page - 1) * pagesize).Take(pagesize).ToList();


                PageViewModel pageViewModel = new PageViewModel(count, page, pagesize);
                IndexViewModel viewModel = new()
                {
                    PageViewModel = pageViewModel,
                    Books = items
                };

                return View(viewModel);
            }
            return NotFound();
        }
        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.UsersAndBooks).ThenInclude(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,PhotoUrl,PublishedDate,Description,Created,Status")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
        public async Task<IActionResult> TakeBook(int? id)
        {
            if (id != null)
            {
                var book = await _context.Books.Include(b => b.UsersAndBooks).ThenInclude(u => u.User).FirstOrDefaultAsync(e => e.Id == id);
                if (book != null)
                {
                    User? user = await _context.Users.Include(u => u.UsersAndBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                    if (user != null)
                    {
                        if (user.UsersAndBooks.Where(u => u.ReturnBookDate == null).Count() > 3)
                        {
                            ViewBag.Error = "Нельзя добавить больше чем 3 книги";
                            return View();
                        }

                        UserAndBook uab = new()
                        {
                            User = user,
                            Book = book,
                            UserId = user.Id,
                            BookId = book.Id,
                            GetBookDate = DateTime.UtcNow,
                            ReturnBookDate = null
                        };
                        _context.Add(uab);
                        book.Status = "Выдана";
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> ReturnBook(int? id)
        {
            if (id != null)
            {
                Book book = await _context.Books.Include(b => b.UsersAndBooks).ThenInclude(u => u.User).FirstOrDefaultAsync(b => b.Id == id);
                if (book != null)
                {
                    UserAndBook? uab = book.UsersAndBooks.LastOrDefault(u => u.User.Email == User.Identity.Name && u.BookId == book.Id);
                    if (uab != null)
                    {
                        uab.ReturnBookDate = DateTime.UtcNow;
                        book.Status = "В наличии";
                        
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
