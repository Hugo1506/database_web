﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace database_web.Controllers
{
    [Authorize]
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
            if(anunc == 0)
            {
                return RedirectToAction("Index", "Anuncios");
            }
            var reviewsNovas = await _context.review.Include(m=>m.anuncio)
            .Where(m => m.AnuncioFK == anunc)
            .ToListAsync();
            
            return View(reviewsNovas);
        }


        //Reviews feitas pelo utilizador
        public async Task<IActionResult> ReviewsPessoais()
        {
            //user que está logged in 
            var userId = User.Identity.Name;
            if (userId != null)
            {
                //comprador que está logged in 
                var comprador = await _context.comprador
                 .FirstOrDefaultAsync(m => m.email == userId);

                //reviews que têm como comprador o utilizador que está logged in
                var reviewsNovas = await _context.review.Include(m => m.anuncio.Produto).Include(m => m.anuncio.vendedor)
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
            //chave forasteira para o anuncio
            var anuncId = review.AnuncioFK;
            //user que está logged in 
            var userId = User.Identity.Name;
            if (userId != null)
            {
                //comprador que está logged in
                var comprador = await _context.comprador
                 .FirstOrDefaultAsync(m => m.email == userId);
                //chave foreasteira para o comprador
                review.CompradorFK = comprador.login;

                //anuncio a que a review está associada
                review.anuncio = await _context.anuncio
                 .FirstOrDefaultAsync(m => m.Id == review.AnuncioFK);
                anuncId = review.anuncio.Id;
                
                //adiciona a review à base de dados
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

            //comprador que está login
            var userId = User.Identity.Name;
            var comprador = await _context.comprador.FirstOrDefaultAsync(c => c.email == userId);

            //se o comprador não for o autor da review ele irá receber uma pagina de erro
            if (comprador.login != review.CompradorFK)
            {
                return View("noPerms");
            }
            //continua para a pagina de edit
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,conteudo")] Review novaReview)
        {

            var review = await _context.review.FirstOrDefaultAsync(r => r.Id == novaReview.Id);

            if (id != novaReview.Id)
            {
                return NotFound();
            }

                try
                {
                    review.conteudo = novaReview.conteudo;
                    _context.Entry(review).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("index", new { anunc = review.AnuncioFK }); ;
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
            
               
           
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //review
            var rev = await _context.review.FirstOrDefaultAsync(r => r.Id == id);
            //user que está logged in 
            var userId = User.Identity.Name;
            if (userId != null)
            {
                //comprador que está logged in 
                var moderador = await _context.moderador
                 .FirstOrDefaultAsync(m => m.email == userId);

                var comprador = await _context.comprador
                 .FirstOrDefaultAsync(m => m.email == userId);
                if (moderador != null || comprador.login == rev.CompradorFK)
                {
                    //review a ser apagada
                    var review = await _context.review
                        .FirstOrDefaultAsync(m => m.Id == id);

                    //apaga a review
                    await DeleteConfirmed(review.Id);
                    

                   return RedirectToAction("Index", "Home");

                }
                return RedirectToAction("noPerms");
            }
            return RedirectToAction("noPerms");
        }


        //pagina que mostra o error de falta de permições
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

        public async Task<IActionResult> verAnuncio(int id)
        {
            var reviewAtual = await _context.review
                        .FirstOrDefaultAsync(m => m.Id == id);

            return RedirectToAction("Index", "Anuncios", new { review = reviewAtual.AnuncioFK });
        }
    }
}
