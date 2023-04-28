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
public class EpisodesController : Controller
{
    private readonly IDBContext _context;
    private readonly IMapper _mapper;
    private readonly IHelpers _helpers;

    public EpisodesController(IDBContext context, IMapper mapper, IHelpers helpers)
    {
        _context = context;
        _mapper = mapper;
        _helpers = helpers;
    }
    public async Task<IActionResult> Index()
    {
        var myContext = _context.Episodes.Include(e => e.seasons);
        return View(await myContext.ToListAsync());
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Episodes == null) return NotFound();
        var episode = await _context.Episodes.Include(e => e.seasons).FirstOrDefaultAsync(m => m.id == id);
        if (episode == null) return NotFound();
        return View(episode);
    }

    public IActionResult GetSeasonsof(int mediaid)
    {
        var seasons = _context.Seasons.Where(x => x.Mediaid == mediaid).ToList();
        return Ok(seasons);
    }
    public IActionResult GetEpisodesof(int seasonid)
    {
        var eps = _context.Episodes.Where(x => x.seasonsid == seasonid).ToList();
        return Ok(eps.Count() + 1);
    }
    public IActionResult GetFirstEps(int mediaid)
    {
        var seasonid = _context.Seasons.FirstOrDefaultAsync(x => x.Mediaid == mediaid).Result.id;
        var eps = _context.Episodes.Where(x => x.seasonsid == seasonid).ToList();
        return Ok(eps.Count() + 1);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["medias"] = new SelectList(_context.Media.Where(x => x.Discriminator != "Movie").OrderBy(x => x.Id).Reverse(), "Id", "Title");
        ViewBag.Media = await _context.Media.Where(x => x.Discriminator != "Movie").OrderBy(x => x.Id).Reverse().ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EpisodeDTO episode)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(x => x.id == episode.seasonsid);
        var media = await _context.Media.FirstOrDefaultAsync(x => x.Id == season.Mediaid);
        var mapped = _mapper.Map<Episode>(episode);
        mapped.name = season.name + " الحلقة " + episode.name;
        mapped.Thumbnail = _helpers.ImgToStr(episode.Thumbnail);
        _context.Episodes.Add(mapped);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Episodes == null) return NotFound();
        var episode = await _context.Episodes.FindAsync(id);
        if (episode == null) return NotFound();
        ViewData["seasonsid"] = new SelectList(_context.Seasons, "id", "id", episode.seasonsid);
        return View(episode);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("id,seasonsid,Video")] Episode episode)
    {
        if (id != episode.id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(episode);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EpisodeExists(episode.id)) return NotFound(); else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(episode);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Episodes == null) return NotFound();
        var episode = await _context.Episodes
            .Include(e => e.seasons)
            .FirstOrDefaultAsync(m => m.id == id);
        if (episode == null) return NotFound();
        return View(episode);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Episodes == null) return Problem("Entity set 'MyContext.Episodes'  is null.");
        var episode = await _context.Episodes.FindAsync(id);
        if (episode != null) _context.Episodes.Remove(episode);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool EpisodeExists(int id) => (_context.Episodes?.Any(e => e.id == id)).GetValueOrDefault();
}