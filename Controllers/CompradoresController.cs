using database_web.Data;
using database_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace database_web.Controllers
{
    public class CompradoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompradoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compradores
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            
            return _context.comprador != null ?
                        View(await _context.comprador.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.comprador'  is null.");
        }

        // GET: Compradores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.comprador == null)
            {
                return NotFound();
            }

            var comprador = await _context.comprador
                .FirstOrDefaultAsync(m => m.login == id);
            if (comprador == null)
            {
                return NotFound();
            }
            ViewData["CompradorDinheiro"] = comprador.dinheiro;
            return View(comprador);
        }

        // GET: Compradores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("login,password,nome,telefone,email,dinheiro")] Comprador comprador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comprador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompradorDinheiro"] = comprador.dinheiro;
            return View(comprador);
        }

        // GET: Compradores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.comprador == null)
            {
                return NotFound();
            }

            var comprador = await _context.comprador.FindAsync(id);
            if (comprador == null)
            {
                return NotFound();
            }
            return View(comprador);
        }

        // POST: Compradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("login,password,nome,telefone,email,dinheiro")] Comprador comprador)
        {
            if (id != comprador.login)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comprador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompradorExists(comprador.login))
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
            return View(comprador);
        }

        // GET: Compradores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.comprador == null)
            {
                return NotFound();
            }

            var comprador = await _context.comprador
                .FirstOrDefaultAsync(m => m.login == id);
            if (comprador == null)
            {
                return NotFound();
            }

            return View(comprador);
        }

        // POST: Compradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.comprador == null)
            {
                return Problem("Entity set 'ApplicationDbContext.comprador'  is null.");
            }
            var comprador = await _context.comprador.FindAsync(id);
            if (comprador != null)
            {
                _context.comprador.Remove(comprador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompradorExists(string id)
        {
            return (_context.comprador?.Any(e => e.login == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> meusProdutos()
        {
            var userId = User.Identity.Name;
            if(userId != null)
            {
                var compradorAtual =  await _context.comprador.FirstOrDefaultAsync(m => m.email == userId);
                var produtosComprados = _context.comprador_produto.Include(a => a.produto).Include(a => a.comprador)
                                            .Where(a => a.CompradorLogin == compradorAtual.login);
                return View(await produtosComprados.ToListAsync());
            }
            return RedirectToAction("Index", "Home");
            
           
        }

        public async Task<IActionResult> verCompradores(int id)
        {
            var compra = await _context.comprador_produto.FindAsync(id);
            var compradoresProduto = _context.comprador_produto.Include(a => a.produto).Include(a => a.comprador)
                                            .Where(a => a.ProdutoId == compra.ProdutoId);
            return View(await compradoresProduto.ToListAsync());
        }


    }
}
