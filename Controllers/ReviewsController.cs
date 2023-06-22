using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using database_web.Data;
using database_web.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;

namespace database_web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(int anunc)
        {

            var reviewsNovas = await _context.review
            .Where(m => m.AnuncioFK == anunc)
             .ToListAsync();
            return View(reviewsNovas);
        }

        public async Task<IActionResult> ReviewsPessoais()
        {

            var userId = User.Identity.Name;
            if (userId != null)
            {
                var comprador = await _context.comprador
                 .FirstOrDefaultAsync(m => m.email == userId);

                var reviewsNovas = await _context.review
                .Where(m => m.CompradorFK == comprador.login)
                .ToListAsync();

                return View(reviewsNovas);

            }
            return RedirectToAction("Index","Home ");
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.review == null)
            {
                return NotFound();
            }

            var review = await _context.review
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create(int anunc)
        {
            ViewBag.AnuncioFK = anunc;
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,conteudo,AnuncioFK")] Review review)
        {
            var anuncId = review.AnuncioFK;
            var userId = User.Identity.Name;
            if (userId != null)
            {
                var comprador = await _context.comprador
                 .FirstOrDefaultAsync(m => m.email == userId);
                review.CompradorFK = comprador.login;

                review.anuncio = await _context.anuncio
                 .FirstOrDefaultAsync(m => m.Id == review.AnuncioFK);
                anuncId = review.anuncio.Id;
                

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", new { anunc = anuncId }); 

            }
            

            return RedirectToAction("index", new { anunc = anuncId }); 
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.review == null)
            {
                return NotFound();
            }

            var review = await _context.review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,conteudo,CompradorFK,AnuncioFK,ModeradorFK")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = User.Identity.Name;
            if (userId != null)
            {
                var comprador = await _context.vendedor
                 .FirstOrDefaultAsync(m => m.email == userId);
                if (comprador != null)
                {
                    if (id == null || _context.review == null)
                    {
                        return NotFound();
                    }

                    var review = await _context.review
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (review == null)
                    {
                        return NotFound();
                    }

                   return RedirectToAction("Index", "Home");

                }
                return RedirectToAction("noPerms");
            }
            return RedirectToAction("noPerms");
        }

        public IActionResult noPerms()
        {
            return View();
        }

         

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.review == null)
            {
                return Problem("Entity set 'ApplicationDbContext.review'  is null.");
            }
            var review = await _context.review.FindAsync(id);
            if (review != null)
            {
                _context.review.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index" ,"Home");
        }

        private bool ReviewExists(int id)
        {
          return (_context.review?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
