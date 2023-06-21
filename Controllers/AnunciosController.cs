using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using database_web.Data;
using database_web.Models;
using Microsoft.Build.Construction;


namespace database_web.Controllers
{
    public class AnunciosController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AnunciosController(ApplicationDbContext context )
        {
            _context = context;


        }

        // GET: Anuncios
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.anuncio.Include(a => a.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Anuncios/Details/5
        public async Task<IActionResult> Compra(int? id)
        {
            if (id == null || _context.anuncio == null)
            {
                return NotFound();
            }

            var anuncio = await _context.anuncio
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            var userId = User.Identity.Name;

            var comprador = await _context.comprador.FirstOrDefaultAsync(m => m.email == userId);

            var vendedor = await _context.vendedor.FirstOrDefaultAsync(m => m.login == anuncio.VendedorFK);

            var vendedorCompradorTask = _context.vendedor.FirstOrDefaultAsync(m => m.email == comprador.email);

            var vendedorComprador = await vendedorCompradorTask;
            if (comprador.dinheiro >= anuncio.preco)
            {
                vendedor.dinheiro += anuncio.preco;
                _context.Entry(vendedor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                comprador.dinheiro -= anuncio.preco;
                _context.Entry(comprador).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                if(vendedorComprador != null)
                {
                    vendedorComprador.dinheiro += anuncio.preco; 
                    _context.Entry(vendedorComprador).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return View(anuncio);
                }

            }
            

            return View(anuncio);
        }

        // GET: Anuncios/Create
        public IActionResult Create()
        {
            ViewData["ProdutoFK"] = new SelectList(_context.produto, "Id", "nome");
            return View();
        }

        // POST: Anuncios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,preco,ProdutoFK")] Anuncio anuncio)
        {


            if (anuncio.ProdutoFK ==null || anuncio.preco == null)
            {
                return View(anuncio);
            }
            var userId = User.Identity.Name;
            if (userId != null)
            {
                var vendedor = await _context.vendedor
                 .FirstOrDefaultAsync(m => m.email == userId);
                anuncio.vendedor = vendedor;
                if (anuncio.vendedor != null)
                {
                    anuncio.VendedorFK = anuncio.vendedor.login;
                }
                
            }
                _context.Add(anuncio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            ViewData["ProdutoFK"] = new SelectList(_context.produto, "Id", "nome", anuncio.ProdutoFK);
            return View(anuncio);
        }

        // GET: Anuncios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.anuncio == null)
            {
                return NotFound();
            }

            var anuncio = await _context.anuncio.FindAsync(id);
            if (anuncio == null)
            {
                return NotFound();
            }
            ViewData["ProdutoFK"] = new SelectList(_context.produto, "Id", "nome", anuncio.ProdutoFK);
            return View(anuncio);
        }

        // POST: Anuncios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,preco,ProdutoFK")] Anuncio anuncio)
        {
            if (id != anuncio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anuncio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnuncioExists(anuncio.Id))
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
            ViewData["ProdutoFK"] = new SelectList(_context.produto, "Id", "nome", anuncio.ProdutoFK);
            return View(anuncio);
        }

        // GET: Anuncios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.anuncio == null)
            {
                return NotFound();
            }

            var anuncio = await _context.anuncio
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
           
            if (anuncio == null)
            {
                return NotFound();
            }

            return View(anuncio);
        }

        // POST: Anuncios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.anuncio == null)
            {
                return Problem("Entity set 'ApplicationDbContext.anuncio'  is null.");
            }
            var anuncio = await _context.anuncio.FindAsync(id);
            if (anuncio != null)
            {
                _context.anuncio.Remove(anuncio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnuncioExists(int id)
        {
          return (_context.anuncio?.Any(e => e.Id == id)).GetValueOrDefault();
        }

     

    }
}
