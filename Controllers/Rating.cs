using Castle.Areas.Identity.Data;
using Castle.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Castle.Controllers;
public class Rating : Controller
{
    private readonly IDBContext _context;
    private readonly UserManager<IUser> _user;
    public Rating(IDBContext context, UserManager<IUser> user)
    {
        _context = context;
        _user = user;
    }
    public async Task<IActionResult> Add(string Type,int id,byte Rate)
    {
        var rate = new Castle.Models.Rating()
        {
            IUserId = _user.GetUserId(User),
            MediaId = id,
            Type = Type,
            rating = Rate
        };
        if (Type == null || id == null) return BadRequest();
        await _context.AddAsync(rate);
        await _context.SaveChangesAsync();
        return Ok();
    }

    public async Task<IActionResult> Remove(string Type, int id)
    {
        var rate = await _context.Rating.FirstOrDefaultAsync(x => x.IUserId == _user.GetUserId(User)
        && x.Type == Type && x.MediaId == id);
        if (rate == null) return BadRequest();
        _context.Rating.Remove(rate);
        await _context.SaveChangesAsync();
        return Ok();
    }
}