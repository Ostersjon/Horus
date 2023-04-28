using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Castle.Models;
using Castle.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApp1.Controllers;

[Authorize]
public class MediaGenresController : Controller
{
    private readonly IDBContext _context;

    public MediaGenresController(IDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var myContext = _context.MediaGenres.Include(m => m.Genre).Include(m => m.Media);
        return View(await myContext.ToListAsync());
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.MediaGenres == null) return NotFound();
        var mediaGenres = await _context.MediaGenres
            .Include(m => m.Genre)
            .Include(m => m.Media)
            .FirstOrDefaultAsync(m => m.Mediaid == id);
        if (mediaGenres == null) return NotFound();
        return View(mediaGenres);
    }

    public async Task<IActionResult> Create()
    {
        var mediaid = Convert.ToInt32(Request.Query["mediaid"]);
        TempData["student"] = mediaid;
        var AllGeneres = _context.Genre.ToList();
        var AvillableGenere = await _context.MediaGenres.Where(x => x.Mediaid == mediaid).Select(x => x.Genreid).ToListAsync();
        ViewBag.Geners =await  _context.Genre.Where(x => !AvillableGenere.Contains(x.id)).ToListAsync();
        ViewBag.Mediaid =  _context.Media.FirstOrDefault(x => x.Id == mediaid).Title;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int[] numms)
    {
        var mediaid = Convert.ToInt32(TempData["student"]);
        foreach (var genre in numms)
        {
            var genere = new MediaGenres()
            {
                Genreid = genre,
                Mediaid = mediaid,
            };
            await _context.MediaGenres.AddAsync(genere);
        }
        await _context.SaveChangesAsync();
        return Redirect("~/Movies");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.MediaGenres == null) return NotFound();
        var mediaGenres = await _context.MediaGenres.FindAsync(id);
        if (mediaGenres == null) return NotFound();
        ViewData["Genreid"] = new SelectList(_context.Genre, "id", "Name", mediaGenres.Genreid);
        ViewData["Mediaid"] = new SelectList(_context.Media, "Id", "Discriminator", mediaGenres.Mediaid);
        return View(mediaGenres);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Mediaid,Genreid")] MediaGenres mediaGenres)
    {
        if (id != mediaGenres.Mediaid) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(mediaGenres);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaGenresExists(mediaGenres.Mediaid))  return NotFound();else{throw;}
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Genreid"] = new SelectList(_context.Genre, "id", "Name", mediaGenres.Genreid);
        ViewData["Mediaid"] = new SelectList(_context.Media, "Id", "Discriminator", mediaGenres.Mediaid);
        return View(mediaGenres);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.MediaGenres == null)
        return NotFound();

        var mediaGenres = await _context.MediaGenres
            .Include(m => m.Genre)
            .Include(m => m.Media)
            .FirstOrDefaultAsync(m => m.Mediaid == id);
        if (mediaGenres == null) return NotFound();

        return View(mediaGenres);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.MediaGenres == null) return Problem("Entity set 'MyContext.MediaGenres'  is null.");
     
        var mediaGenres = await _context.MediaGenres.FindAsync(id);
        if (mediaGenres != null) _context.MediaGenres.Remove(mediaGenres); 

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool MediaGenresExists(int id) => (_context.MediaGenres?.Any(e => e.Mediaid == id)).GetValueOrDefault();
}