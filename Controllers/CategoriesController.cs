using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Castle.Models;
using Castle.Data;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Castle.Dtos;

namespace WebApp1.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(IDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        return _context.Categories != null ?View(await _context.Categories.ToListAsync()) :
        Problem("Entity set 'MyContext.Categories'  is null.");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Categories == null) return NotFound();
        var category = await _context.Categories.FirstOrDefaultAsync(m => m.id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryDTO category)
    {
        if (ModelState.IsValid)
        {
            var mapped = _mapper.Map<Category>(category);
            await _context.Categories.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Categories == null) return NotFound();
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("id,Name")] Category category)
    {
        if (id != category.id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.id)) return NotFound(); else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Categories == null) return NotFound();
        var category = await _context.Categories.FirstOrDefaultAsync(m => m.id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Categories == null) return Problem("Entity set 'MyContext.Categories'  is null.");
        var category = await _context.Categories.FindAsync(id);
        if (category != null) _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool CategoryExists(int id) => (_context.Categories?.Any(e => e.id == id)).GetValueOrDefault();
}