using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using database_web.Data;
using database_web.Models;
using Microsoft.AspNetCore.Authorization;

namespace database_web.Controllers
{
    [Authorize]
    public class CarrinhosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarrinhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carrinhos
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;

            var comprador = await _context.comprador.FirstOrDefaultAsync(m => m.email == userId);

            var carrinhosPessoais = await _context.carrinho
            .Where(c => c.CompradorFK == comprador.login)
            .ToListAsync();

            return View(carrinhosPessoais);
           
        }

        // GET: Carrinhos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.carrinho == null)
            {
                return NotFound();
            }

            var carrinho = await _context.carrinho
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrinho == null)
            {
                return NotFound();
            }

            return View(carrinho);
        }

        // GET: Carrinhos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carrinhos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nome,preco,CompradorFK")] Carrinho carrinho)
        {

            var userId = User.Identity.Name;
            if (userId != null)
            {
                var comprador = await _context.comprador.FirstOrDefaultAsync(m => m.email == userId);
                var carrinhoNovo = new Carrinho
                {
                    nome = carrinho.nome,
                    preco = 0,
                    CompradorFK = comprador.login,
                };

                carrinhoNovo.comprador = comprador;
                _context.Add(carrinhoNovo);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
                 return View(carrinho);
        }

        // GET: Carrinhos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.carrinho == null)
            {
                return NotFound();
            }

            var carrinho = await _context.carrinho.FindAsync(id);
            if (carrinho == null)
            {
                return NotFound();
            }
            return View(carrinho);
        }

        // POST: Carrinhos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nome,preco,CompradorFK")] Carrinho carrinho)
        {
            if (id != carrinho.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrinho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarrinhoExists(carrinho.Id))
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
            return View(carrinho);
        }

        // GET: Carrinhos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.carrinho == null)
            {
                return NotFound();
            }

            var carrinho = await _context.carrinho
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrinho == null)
            {
                return NotFound();
            }

            return View(carrinho);
        }

        // POST: Carrinhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.carrinho == null)
            {
                return Problem("Entity set 'ApplicationDbContext.carrinho'  is null.");
            }
            var carrinho = await _context.carrinho.FindAsync(id);
            if (carrinho != null)
            {
                _context.carrinho.Remove(carrinho);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarrinhoExists(int id)
        {
          return (_context.carrinho?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
