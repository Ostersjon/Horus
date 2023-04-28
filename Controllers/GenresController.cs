using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Castle.Dtos;
using Castle.Helpers;
using Castle.Models;
using Castle.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApp1.Controllers;

[Authorize]
public class GenresController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;
    private readonly IHelpers _helpers;

    public GenresController(IDBContext context, IMapper mapper, IHelpers helpers)
    {
        _context = context;
        _mapper = mapper;
        _helpers = helpers;
    }
    public async Task<IActionResult> Index()=>_context.Genre != null ?View(await _context.Genre.ToListAsync()):Problem("Entity set 'MyContext.Genre'  is null.");
    
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Genre == null) return NotFound();
        var genre = await _context.Genre
            .FirstOrDefaultAsync(m => m.id == id);
        if (genre == null) return NotFound();
        return View(genre);
    }
    public IActionResult Create() => View();
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GenreDTO genre)
    {
        var mapped = _mapper.Map<Genre>(genre);
        mapped.Photo = _helpers.ImgToStr(genre.Photo);
        await _context.Genre.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Genre == null) return NotFound();
        var genre = await _context.Genre.FindAsync(id);
        if (genre == null) return NotFound();
        return View(genre);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("id,Name")] Genre genre)
    {
        if (id != genre.id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.id)) return NotFound();else throw;   
            }
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Genre == null) return NotFound();
        var genre = await _context.Genre.FirstOrDefaultAsync(m => m.id == id);
        if (genre == null) return NotFound();
        return View(genre);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Genre == null) return Problem("Entity set 'MyContext.Genre'  is null.");
        var genre = await _context.Genre.FindAsync(id);
        if (genre != null) _context.Genre.Remove(genre);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool GenreExists(int id) => (_context.Genre?.Any(e => e.id == id)).GetValueOrDefault();
}
