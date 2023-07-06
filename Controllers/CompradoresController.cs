using database_web.Data;
using database_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;


namespace database_web.Controllers
{
    public class CompradoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public CompradoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
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
            if (userId != null)
            {
                var compradorAtual = await _context.comprador.FirstOrDefaultAsync(m => m.email == userId);
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






        [Route("compradores/login")]
        [HttpPost]
        public async Task<IActionResult> login([FromBody] Dictionary<string, string> credentials)
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
                    var comprador = await _context.comprador.FirstOrDefaultAsync(m => m.login == login);

                    var user = await _userManager.FindByEmailAsync(comprador.email);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                        if (result.Succeeded)
                        {
                            return Ok(user.Id);
                        }
                    }
                    return Ok("O user existe");
                }

                return Ok("Password incorreta");
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

            if (password != password_conf)
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
        [Route("compradores/createProdutos")]
        [HttpPost]
        public async Task<IActionResult> createProdutos([FromBody] Dictionary<string, string> Produto)
        {
            var nomeReceived = Produto.TryGetValue("nome", out var nome);
            var descricaoReceived = Produto.TryGetValue("descricao", out var descricao);


            var novoProduto = new Produto()
            {
                nome = nome,
                descricao = descricao

            };

            _context.Add(novoProduto);
            await _context.SaveChangesAsync();
            return Ok("Produto criado com sucesso");

        }

        [Route("compradores/getProdutos")]
        [HttpGet]
        public async Task<IActionResult> getProdutos()
        {
            var produtos = await _context.produto.ToListAsync();
            return Json(produtos);
        }

        [Route("compradores/createAnuncio")]
        [HttpPost]
        public async Task<IActionResult> createAnuncio([FromBody] Dictionary<string, string> Anuncio)
        {
            var produtoIdReceived = Anuncio.TryGetValue("produto_id", out var produto_id);
            var precoReceived = Anuncio.TryGetValue("preco", out var preco);
            var userIdReceived = Anuncio.TryGetValue("userId", out var userId);

            preco = preco.Replace(".", ",");

            var comprador = await _context.comprador.FirstOrDefaultAsync(m => m.UserId == userId);
            var vendedor = await _context.vendedor.FirstOrDefaultAsync(m => m.email == comprador.email);
            if (vendedor == null)
            {



                var vendedorNovo = new Vendedor
                {
                    login = comprador.login,
                    password = comprador.password,
                    nome = comprador.nome,
                    telefone = comprador.telefone,
                    email = comprador.email,
                    dinheiro = comprador.dinheiro
                };

                _context.Add(vendedorNovo);
                await _context.SaveChangesAsync();
                vendedor = vendedorNovo;
            }

            if (produto_id != null && preco != null)
            {
                var anuncioNovo = new Anuncio()
                {
                    ProdutoFK = int.Parse(produto_id),
                    vendedor = vendedor,
                    VendedorFK = vendedor.login,
                    preco = Decimal.Parse(preco)
                };

                _context.Add(anuncioNovo);
                await _context.SaveChangesAsync();
                return Ok("anuncio criado com sucesso");
            }

            return Ok("Erro na criação do anuncio");
        }

        [Route("compradores/getAnuncios")]
        [HttpGet]
        public async Task<IActionResult> getAuncios()
        {
            var anuncios = await _context.anuncio.ToListAsync();
            return Json(anuncios);
        }

        [Route("compradores/getSaldo")]
        [HttpGet]
        public async Task<IActionResult> getSaldo([FromQuery] string userId)
        {
            var comprador = await _context.comprador.FirstOrDefaultAsync(c => c.UserId == userId);
            var saldo = comprador.dinheiro;

            return Json(saldo);

        }

        [Route("compradores/postComprar")]
        [HttpPost]
        public async Task<IActionResult> postComprar([FromBody] Dictionary<string, string> compra)
        {
            var anuncioReceived = compra.TryGetValue("anuncio", out var anuncio);
            var userReceived = compra.TryGetValue("user", out var user);

            var comprador = await _context.comprador.FirstOrDefaultAsync(c => c.UserId == user);
            
            var anunc = await _context.anuncio.FirstOrDefaultAsync(a => a.Id == int.Parse(anuncio));

            if(comprador.dinheiro>= anunc.preco)
            {
                comprador.dinheiro -= anunc.preco;
                _context.Entry(comprador).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var vendedor = await _context.vendedor.FirstOrDefaultAsync(v => v.login == anunc.VendedorFK);

                if (vendedor != null)
                {
                    vendedor.dinheiro += anunc.preco;
                    _context.Entry(vendedor).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                return Ok(comprador.dinheiro.ToString());
            }

            return Ok(comprador.dinheiro.ToString());
            
        }

        [Route("compradores/getReviews")]
        [HttpGet]    
        public async Task<IActionResult> getReviews([FromQuery] int anunc)
        {

            var reviews = await _context.review.Where(r => r.AnuncioFK == anunc).ToListAsync();
            return Json(reviews);
        }

    }
    
}
