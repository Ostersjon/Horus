using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Castle.Areas.Identity.Data;
using Castle.Dtos;
using Castle.Models;
using Castle.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp1.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDBContext _context;
    private readonly UserManager<IUser> _manger;

    public HomeController(ILogger<HomeController> logger, IDBContext context, UserManager<IUser> manger)
    {
        _logger = logger;
        _context = context;
        _manger = manger;
    }

    public async Task<IActionResult> Index()
    {
        var AllCategories = await _context.Categories.ToListAsync();
        ViewBag.Geners = await _context.Genre.ToListAsync();
        var Array = new List<HomeItemsDTO>();
        foreach (var category in AllCategories)
        {
            var Homeitem = new HomeItemsDTO()
            {
                Category = category,
                Media = await _context.Media.Where(x => x.Categoryid == category.id).Take(24).ToListAsync(),
            };
            Array.Add(Homeitem);
        }
        ViewBag.HomeItems = Array;
        ViewBag.productions = await _context.Productions.Take(10).ToListAsync();
        return View();

    }
    public async Task<IActionResult> General()
    {
        var boolpro = int.TryParse(Request.Query["production"],out int production);ViewBag.boolpro = boolpro;
        var boolcat = int.TryParse(Request.Query["category"],out int Categroryid);ViewBag.boolcat = boolcat;
        var boolgen = int.TryParse(Request.Query["genre"],out int genereid); ViewBag.boolcat = boolcat;
        var page = Convert.ToInt32(Request.Query["page"]);
        var type = Request.Query["type"][0];
        var genre = await _context.MediaGenres.Where(x => x.Genreid == genereid).Select(x => x.Mediaid).ToListAsync();
        var items = await _context.Media.Where(x => (type != "Any"? x.Type == type:x.Id !=0) && (boolpro?
        x.productionsid == production : x.Id !=0 ) && (boolcat? x.Categoryid == Categroryid:x.Id != 0) 
        && (boolgen ? genre.Contains(x.Id) : x.Id != 0)).ToListAsync();
        ViewBag.Medias = items.Skip((page - 1) * 36).Take(36);
        ViewBag.pages =Math.Ceiling(Convert.ToDouble(items.Count()+1) / 36.00);
        ViewBag.AllCategories = await _context.Categories.ToListAsync();
        ViewBag.AllGenres = await _context.Genre.ToListAsync();
        ViewBag.productioniD = await _context.Productions.ToListAsync();
        return View();
    }
    
    public async Task<IActionResult> Media()
    {
        var MediaID = Convert.ToInt32(Request.Query["id"]);
        var Genere = await _context.Media.Where(x => x.Id == MediaID).Include(x => x.MediaGenres)
            .ThenInclude(x => x.Genre).FirstOrDefaultAsync();
        var rating = await _context.Rating.Where(x => x.MediaId == Genere.Id && x.Type == "Media").Select(x => x.rating).ToListAsync();
        ViewBag.AvRate = 0;
        if(rating.Count() != 0) ViewBag.AvRate = String.Format("{0:0.0}", Convert.ToDouble(Convert.ToDouble(rating.Sum(x => x)) /Convert.ToDouble(rating.Count())));
        if(User.Identity.IsAuthenticated) ViewBag.IsFav = await _context.WishList
        .Where(x => x.IuserId ==_manger.GetUserAsync(User).Result.Id && x.MediaId == MediaID && x.Type == "Media")
        .FirstOrDefaultAsync() == null;
        if (User.Identity.IsAuthenticated) ViewBag.IsRated = await _context.Rating
        .Where(x => x.IUserId == _manger.GetUserAsync(User).Result.Id && x.MediaId == MediaID).FirstOrDefaultAsync();

        if (Genere is null) return RedirectToAction("Index");

        if (Genere.Discriminator == "Movie"){
            var Media = await _context.Movie.Where(x => x.Id == Genere.Id).Include(x => x.MediaGenres)
            .Include(x => x.MediaGenres).Include(x => x.productions).FirstOrDefaultAsync();
            ViewBag.Media = Media;
        }else {
            var Media = await _context.Media.Where(x => x.Id == Genere.Id).Include(x => x.seasons)
            .Include(x => x.MediaGenres).Include(x => x.productions).FirstOrDefaultAsync();
            ViewBag.Media = Media;
        }
        return View();
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Media(WishList req)
    {
        var MediaID = req.MediaId;
        var UserID = _manger.GetUserAsync(User).Result.Id;
        if (MediaID == null || UserID == null) return View("Something Went Wrong");
        var wish = new WishList()
        {
            IuserId = UserID,
            MediaId = MediaID,
            Type="Media"
        };
        await _context.WishList.AddAsync(wish);
        await _context.SaveChangesAsync();
        return Redirect($"~/Home/Media?id={MediaID}");
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveFav(WishList req)
    {
        var MediaID = req.MediaId;
        var UserID = _manger.GetUserAsync(User).Result.Id;
        var wishitem = await _context.WishList.Where(x => x.IuserId==UserID&& x.MediaId == MediaID && x.Type == "Media").FirstOrDefaultAsync();
        if (wishitem == null) return View("Something Went Wrong");
        _context.WishList.Remove(wishitem);
        await _context.SaveChangesAsync();
        return Redirect($"~/Home/Media?id={MediaID}");
    }
    public IActionResult Privacy() => View();
    public async Task<IActionResult> Seasons()
    {

        var MediaID = Convert.ToInt32(Request.Query["Mediaid"]);
        var SeasonID = Convert.ToInt32(Request.Query["id"]);
        var rating = await _context.Rating.Where(x => x.MediaId == MediaID && x.Type == "Season").Select(x => x.rating).ToListAsync();
        ViewBag.AvRate = 0;
        if (rating.Count() != 0) ViewBag.AvRate = String.Format("{0:0.0}", Convert.ToDouble(Convert.ToDouble(rating.Sum(x => x)) / Convert.ToDouble(rating.Count())));
        if (User.Identity.IsAuthenticated) ViewBag.IsFav = await _context.WishList
        .Where(x => x.IuserId == _manger.GetUserAsync(User).Result.Id && x.MediaId == SeasonID && x.Type == "Season")
        .FirstOrDefaultAsync() == null;

        if (User.Identity.IsAuthenticated) ViewBag.IsRated = await _context.Rating
        .Where(x => x.IUserId == _manger.GetUserAsync(User).Result.Id && x.MediaId == SeasonID && x.Type == "Season")
        .FirstOrDefaultAsync();
        ViewBag.Media  = await _context.Media.Where(x => x.Id == MediaID).Include(x => x.MediaGenres)
            .ThenInclude(x => x.Genre).Include(x => x.productions).FirstOrDefaultAsync();
        ViewBag.Season = await _context.Seasons.FirstOrDefaultAsync(x => x.id == SeasonID);
        ViewBag.Episodes = await _context.Episodes.Where(x => x.seasonsid == SeasonID).ToListAsync();
        return View();
    }

    public async Task<IActionResult> Episode()
    {
        var EpsID = Convert.ToInt32(Request.Query["Epsid"]);
        var Episode = await _context.Episodes.FirstOrDefaultAsync(x => x.id == EpsID);
        if (User.Identity.IsAuthenticated) ViewBag.IsFav = await _context.WishList
        .Where(x => x.IuserId == _manger.GetUserAsync(User).Result.Id && x.MediaId == EpsID && x.Type == "Episode")
        .FirstOrDefaultAsync() == null;
        ViewBag.AvRate = 0;
        if (User.Identity.IsAuthenticated) ViewBag.IsRated = await _context.Rating
        .Where(x => x.IUserId == _manger.GetUserAsync(User).Result.Id && x.MediaId == EpsID).FirstOrDefaultAsync();
        if (Episode == null) return NotFound();
        ViewBag.Episode = Episode;
        ViewBag.Ep = Convert.ToInt32(Request.Query["ep"]);
        ViewBag.Prev = await _context.Episodes.OrderByDescending(x => x.id).FirstOrDefaultAsync(x => x.seasonsid == Episode.seasonsid && x.id < Episode.id);
        ViewBag.Next = await _context.Episodes.FirstOrDefaultAsync(x => x.seasonsid == Episode.seasonsid && x.id > Episode.id);
        var Season = await _context.Seasons.FirstOrDefaultAsync(x => x.id == Episode.seasonsid);
        ViewBag.NMedia = await _context.Media.FirstOrDefaultAsync(x => x.Id == Season.Mediaid);
        ViewBag.NSeason = Season;
        return View();
    }
    [Authorize]
    public async Task<IActionResult> Favourite()
    {
        var UserID = _manger.GetUserAsync(User).Result.Id;
        var movies = await _context.WishList.Where(x => x.IuserId == UserID && x.Type == "Media").Select(x => x.MediaId).ToListAsync();
        ViewBag.MFav = await _context.Media.Where(x => movies.Contains(x.Id)).ToListAsync();
        var seasons = await _context.WishList.Where(x => x.IuserId == UserID && x.Type == "Season").Select(x => x.MediaId).ToListAsync();
        var episodes = await _context.WishList.Where(x => x.IuserId == UserID && x.Type == "Episode").Select(x => x.MediaId).ToListAsync();
        ViewBag.SFav = await _context.Seasons.Where(x =>seasons.Contains(x.id)).ToListAsync();
        ViewBag.EFav = await _context.Episodes.Where(x => episodes.Contains(x.id)).ToListAsync();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Rate(int mediaid,byte rate)
    {
        var UserID = _manger.GetUserAsync(User).Result.Id;
        if (UserID == null || mediaid == null || rate == null) return BadRequest();
        var rating = new Rating()
        {
            MediaId = mediaid,
            IUserId = UserID,
            rating = rate
        };
        await _context.Rating.AddAsync(rating);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteFav(int id)
    {
        if (id == null || _context.WishList == null) return NotFound();
        var UserID = _manger.GetUserAsync(User).Result.Id;
        if (UserID == string.Empty || UserID == null) return BadRequest();
        var wishList = await _context.WishList.Where(x => x.IuserId == UserID && x.MediaId == id).FirstOrDefaultAsync();
        if (wishList == null) return NotFound();
        _context.WishList.Remove(wishList);
        await _context.SaveChangesAsync();
        return Ok();
    }

    public async Task<IActionResult> Search()
    {
        var searched = Request.Query["searched"];
        var page = Convert.ToInt32(Request.Query["page"]);
        if (searched == "") return BadRequest();

        var items = await _context.Media.AsNoTracking().Where(x => x.Title.Contains(searched)).Include(x => x.seasons).ToListAsync();
        ViewBag.Medias = items.Skip((page-1)*36).Take(36);
        ViewBag.pages = Math.Ceiling(Convert.ToDouble(items.Count() + 1) / 36.00);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });   
}