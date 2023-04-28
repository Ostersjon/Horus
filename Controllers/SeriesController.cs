using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Castle.Dtos;
using Castle.Helpers;
using Castle.Models;
using Castle.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApp1.Controllers;

[Authorize]
public class SeriesController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _web;
    private readonly IHelpers _helpers;

    public SeriesController(IDBContext context, IMapper mapper, IWebHostEnvironment web, IHelpers helpers)
    {
        _context = context;
        _mapper = mapper;
        _web = web;
        _helpers = helpers;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var myContext = _context.Series.Include(t => t.productions);
        return View(await myContext.ToListAsync());
    }
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Series == null) return NotFound();
        var Series = await _context.Series.Include(t => t.productions).FirstOrDefaultAsync(m => m.Id == id);
        if (Series == null) return NotFound();
        return View(Series);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["productionsid"] = new SelectList(_context.Productions, "Id", "name");
        ViewData["Categories"] = new SelectList(_context.Categories, "id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeriesDTO tvshow)
    {
        var mapped = _mapper.Map<Series>(tvshow);
        mapped.Title = "مسلسل" + " " + tvshow.Title;
        mapped.Poster = _helpers.ImgToStr(tvshow.Poster);
        mapped.Cover = _helpers.ImgToStr(tvshow.Cover);
        mapped.Discriminator = "Series";
        await _context.Media.AddAsync(mapped);
        await _context.SaveChangesAsync();
        var id = mapped.Id;
        return Redirect($"~/MediaGenres/Create?mediaid={id}");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Series == null) return NotFound();
        var tVShows = await _context.Series.FindAsync(id);
        if (tVShows == null) return NotFound();
        ViewData["productionsid"] = new SelectList(_context.Productions, "Id", "name", tVShows.productionsid);
        return View(tVShows);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,about,Poster,Language,MovieCountry,Year,productionsid,Categoryid")] Series tVShows)
    {
        if (id != tVShows.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tVShows);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TVShowsExists(tVShows.Id)) return NotFound();else throw; 
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tVShows);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Series == null) return NotFound();
        var tVShows = await _context.Series.Include(t => t.productions).FirstOrDefaultAsync(m => m.Id == id);
        if (tVShows == null) return NotFound();
        return View(tVShows);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Series == null) return Problem("Entity set 'MyContext.TVShows'  is null.");
        var tVShows = await _context.Series.FindAsync(id);
        if (tVShows != null) _context.Series.Remove(tVShows);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool TVShowsExists(int id) => (_context.Series?.Any(e => e.Id == id)).GetValueOrDefault();
}
