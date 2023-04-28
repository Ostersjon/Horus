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

public class MoviesController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _web;
    private readonly IHelpers _helpers;

    public MoviesController(IDBContext context, IMapper mapper, IWebHostEnvironment web, IHelpers helpers)
    {
        _context = context;
        _mapper = mapper;
        _web = web;
        _helpers = helpers;
    }

    public async Task<IActionResult> Index()
    {
        var myContext = _context.Movie.Include(m => m.productions);
        return View(await myContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Movie == null) return NotFound();
        var movie = await _context.Movie
            .Include(m => m.productions)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return NotFound();
        return View(movie);
    }

    public IActionResult Create()
    {
        ViewData["productionsid"] = new SelectList(_context.Productions, "Id", "name");
        ViewData["Categories"] = new SelectList(_context.Categories, "id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieDTO movie)
    {
        var mapped = _mapper.Map<Movie>(movie);
        mapped.Poster = _helpers.ImgToStr(movie.Poster);
        mapped.Cover = _helpers.ImgToStr(movie.Cover);
        mapped.Title = $"فيلم  {movie.Title}";
        mapped.Discriminator = "Movie";
        await _context.Movie.AddAsync(mapped);
        await _context.SaveChangesAsync();
        var id = mapped.Id;
        return Redirect($"~/MediaGenres/Create?mediaid={id}");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Movie == null) return NotFound();
        var movie = await _context.Movie.FindAsync(id);
        if (movie == null) return NotFound();
        ViewData["productionsid"] = new SelectList(_context.Productions, "Id", "name", movie.productionsid);
        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Video,Id,Title,about,Poster,Language,MovieCountry,Year,productionsid,Categoryid")] Movie movie)
    {
        var Movie = _context.Movie.FirstOrDefaultAsync(x => x.Id == id);
        if (Movie == null) return NotFound();
        _context.Movie.Update(movie);
        await _context.SaveChangesAsync();
        return View(movie);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Movie == null) return NotFound();
        var movie = await _context.Movie.Include(m => m.productions).FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return NotFound();
        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null) _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool MovieExists(int id) => (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
}