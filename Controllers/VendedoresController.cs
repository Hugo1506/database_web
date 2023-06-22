using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using database_web.Data;
using database_web.Models;
using Microsoft.AspNetCore.Http;


namespace database_web.Controllers
{
    public class VendedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendedores
        public async Task<IActionResult> Index()
        {
            //user que está logged in 
            var userId = User.Identity.Name;
            if (userId != null)
            {
                //vendedor com o email igual ao userId do user que está logged in 
                var vendedor = await _context.vendedor
                 .FirstOrDefaultAsync(m => m.email == userId);
                //se o vendedor não existir
                if(vendedor == null)
                {
                    //comprador que está logged in
                    var comprador = await _context.comprador.FirstOrDefaultAsync(m => m.email == userId);

                    //criação do novo vendedor a partir do comprador que está logged in
                    var vendedorNovo = new Vendedor
                    {
                        login = comprador.login, 
                        password = comprador.password, 
                        nome = comprador.nome, 
                        telefone = comprador.telefone, 
                        email = comprador.email, 
                        dinheiro = comprador.dinheiro 
                    };

                    //adicção do novo vendedor à base de dados
                    _context.Add(vendedorNovo);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("Create", "Anuncios");

            }
            return _context.vendedor != null ? 
                          View(await _context.vendedor.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.vendedor'  is null.");
        }

        // GET: Vendedores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.vendedor == null)
            {
                return NotFound();
            }

            var vendedor = await _context.vendedor
                .FirstOrDefaultAsync(m => m.login == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

        // GET: Vendedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("login,password,nome,telefone,email,dinheiro")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendedor);
        }

        // GET: Vendedores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.vendedor == null)
            {
                return NotFound();
            }

            var vendedor = await _context.vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("login,password,nome,telefone,email,dinheiro")] Vendedor vendedor)
        {
            if (id != vendedor.login)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendedorExists(vendedor.login))
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
            return View(vendedor);
        }

        // GET: Vendedores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.vendedor == null)
            {
                return NotFound();
            }

            var vendedor = await _context.vendedor
                .FirstOrDefaultAsync(m => m.login == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

        // POST: Vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.vendedor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.vendedor'  is null.");
            }
            var vendedor = await _context.vendedor.FindAsync(id);
            if (vendedor != null)
            {
                _context.vendedor.Remove(vendedor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedorExists(string id)
        {
          return (_context.vendedor?.Any(e => e.login == id)).GetValueOrDefault();
        }
    }
}
