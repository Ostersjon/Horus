using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Castle.Areas.Identity.Data;
using Castle.Dtos;
using Castle.Helpers;
using Castle.Models;
using Castle.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApp1.Controllers;

[Authorize]
public class SeasonsController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;
    private readonly IHelpers _helpers;

    public SeasonsController(IDBContext context, IMapper mapper, IHelpers helpers)
    {
        _context = context;
        _mapper = mapper;
        _helpers = helpers;
    }

    public async Task<IActionResult> Index()
    {
        var myContext = _context.Seasons.Include(s => s.Media);
        return View(await myContext.ToListAsync());
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Seasons == null) return NotFound();
        var seasons = await _context.Seasons.Include(s => s.Media).FirstOrDefaultAsync(m => m.id == id);
        if (seasons == null) return NotFound();
        return View(seasons);
    }
    public IActionResult Create()
    {
        ViewData["Seriesid"] = new SelectList(_context.Media.Where(x => x.Discriminator != "Movie").OrderBy(x => x.Id).Reverse(), "Id", "Title");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeasonDTO dto)
    {
        var media = _context.Media.FirstOrDefaultAsync(x => x.Id == dto.Mediaid).Result.Title;
        var nums = new string[] { "", "الاول", "الثاني", "الثالث", "الرابع", "الخامس", "السادس", "السابع", "الثامن", "التاسع", "العاشر" };
        var mapped = _mapper.Map<Seasons>(dto);
        mapped.Poster = _helpers.ImgToStr(dto.Poster);
        mapped.name = media + " " + "الموسم" + " " + nums[Convert.ToInt32(dto.name)];
        await _context.Seasons.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> GetSeasomncount(int mediaid)
    {
        var seasons = await _context.Seasons.Where(x => x.Mediaid == mediaid).ToListAsync();
        return Ok(seasons.Count + 1);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Seasons == null) return NotFound();
        var seasons = await _context.Seasons.FindAsync(id);
        if (seasons == null) return NotFound();
        ViewData["Seriesid"] = new SelectList(_context.Media, "Id", "Discriminator", seasons.Mediaid);
        return View(seasons);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("id,name,Seriesid")] Seasons seasons)
    {
        if (id != seasons.id) return NotFound();
        try
        {
            _context.Update(seasons);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SeasonsExists(seasons.id)) return NotFound(); else throw;
        }
        return RedirectToAction(nameof(Index));
    }
    public IActionResult GetSeasonsof(int mediaid)
    {
        var seasons = _context.Seasons.Where(x => x.Mediaid == mediaid).ToList();
        return Ok(seasons.Count() + 1);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Seasons == null) return NotFound();
        var seasons = await _context.Seasons.Include(s => s.Media).FirstOrDefaultAsync(m => m.id == id);
        if (seasons == null) return NotFound();
        return View(seasons);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Seasons == null) return Problem("Entity set 'MyContext.Seasons'  is null.");
        var seasons = await _context.Seasons.FindAsync(id);
        if (seasons != null) _context.Seasons.Remove(seasons);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool SeasonsExists(int id) => (_context.Seasons?.Any(e => e.id == id)).GetValueOrDefault();
}