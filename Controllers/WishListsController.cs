using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Castle.Data;
using Castle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Castle.Areas.Identity.Data;

namespace Castle.Controllers;
[Authorize]
public class WishListsController : Controller
{
    private readonly IDBContext _context;
    private readonly UserManager<IUser> _userManager;

    public WishListsController(IDBContext context, UserManager<IUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Add(string Type , int id)
    {
        var wish = new WishList()
        {
            IuserId = _userManager.GetUserId(User),
            MediaId = id,
            Type = Type
        };
        await _context.WishList.AddAsync(wish);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string Type, int id)
    {
        var wish = await _context.WishList.FirstOrDefaultAsync
            (x => x.IuserId == _userManager.GetUserId(User) && x.MediaId == id && x.Type == Type);
        if (wish is null) return BadRequest();
        _context.WishList.Remove(wish);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
