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
public class ProductionsController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;
    private readonly IHelpers _helpers;

    public ProductionsController(IDBContext context, IMapper mapper, IHelpers helpers)
    {
        _context = context;
        _mapper = mapper;
        _helpers = helpers;
    }

    public async Task<IActionResult> Index()
    {
        return _context.Productions != null ?View(await _context.Productions.ToListAsync()) :
        Problem("Entity set 'MyContext.Productions'  is null.");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Productions == null) return NotFound();
        var productions = await _context.Productions.FirstOrDefaultAsync(m => m.Id == id);
        if (productions == null) return NotFound();
        return View(productions);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductionsDTO productions)
    {
        var mapped = _mapper.Map<Productions>(productions);
        mapped.photo = _helpers.ImgToStr(productions.photo);
        await _context.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Productions == null) return NotFound();
        var productions = await _context.Productions.FindAsync(id);
        if (productions == null) return NotFound();
        return View(productions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,name,photo")] Productions productions)
    {
        if (id != productions.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(productions);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionsExists(productions.Id)) return NotFound(); else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(productions);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Productions == null) return NotFound();
        var productions = await _context.Productions.FirstOrDefaultAsync(m => m.Id == id);
        if (productions == null) return NotFound();
        return View(productions);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Productions == null) return Problem("Entity set 'MyContext.Productions'  is null.");
        var productions = await _context.Productions.FindAsync(id);
        if (productions != null) _context.Productions.Remove(productions);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool ProductionsExists(int id)=> (_context.Productions?.Any(e => e.Id == id)).GetValueOrDefault();
}