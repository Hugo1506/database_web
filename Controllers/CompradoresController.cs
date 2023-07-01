using database_web.Data;
using database_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace database_web.Controllers
{
    public class CompradoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CompradoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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




   

        [Route("compradores/test")]
        [HttpPost]
        public async Task<IActionResult> Test([FromBody] Dictionary<string, string> credentials)
        {
            if (credentials.TryGetValue("login", out var login) && credentials.TryGetValue("password", out var password))
            {
                var loginReceived = login;
                var pass = password;

                var compradorTest = await _context.comprador
                 .FirstOrDefaultAsync(m => m.login == loginReceived);

                if (compradorTest == null)
                {
                    return BadRequest("User não existe");
                }

                var passwordHasher = new PasswordHasher<Comprador>();

                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, compradorTest.password, pass);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    return Ok("O user existe");
                }

                return BadRequest("Password incorreta");
            }
            else
            {
                return BadRequest("formato errado");
            }
        }

        [Route("compradores/createComprador")]
        [HttpPost]
        public async Task<IActionResult> createComprador([FromBody] Dictionary<string, string> credentials) {
            var emailReceived = credentials.TryGetValue("email", out var email);
            var passwordReceived = credentials.TryGetValue("password", out var password);
            var password_confReceived = credentials.TryGetValue("password_conf", out var password_conf);
            var loginReceived = credentials.TryGetValue("login", out var login);
            var nomeReceived = credentials.TryGetValue("nome", out var nome);
            var telefoneReceived = credentials.TryGetValue("telefone", out var telefone);
            var dinheiroReceived = credentials.TryGetValue("dinheiro", out var dinheiro);

            var userExists = await _context.Users
                 .FirstOrDefaultAsync(m => m.Email == email);

            if (userExists != null)
            {
                return Ok("Uma conta com este email já existe");
            }

            if(password != password_conf)
            {
                return Ok("passwords não coecidem");
            }


            if (emailReceived && passwordReceived)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var newuser = await _context.Users
                 .FirstOrDefaultAsync(m => m.Email == email);

                    var comprador = new Comprador()
                    {
                        email = email,
                        login = login,
                        password = newuser.PasswordHash,
                        nome = nome,
                        telefone = telefone,
                        dinheiro = decimal.Parse(dinheiro)
                    };

                    

                    comprador.UserId = newuser.Id;

                    _context.Add(comprador);
                    await _context.SaveChangesAsync();

                    return Ok("criado");
                }
                else
                {
                    return Ok("A password tem de conter: 6 caracteres, 1 simbolo, 1 letra maiscula e um número");
                }
                
            }
            else
            {
                return BadRequest("error");
            }

        }
    }
}
