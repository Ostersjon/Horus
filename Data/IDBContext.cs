using Castle.Areas.Identity.Data;
using Castle.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Castle.Data;
public class IDBContext : IdentityDbContext<IUser>
{
    public IDBContext(DbContextOptions<IDBContext> options): base(options)
    {
    }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Episode> Episodes { get; set; }
    public virtual DbSet<Series> Series { get; set; }
    public virtual DbSet<Movie> Movie { get; set; }
    public virtual DbSet<IUser> IUser { get; set; }
    public virtual DbSet<WishList> WishList { get; set; }
    public virtual DbSet<Seasons> Seasons { get; set; }
    public virtual DbSet<MediaCategories> MediaCategories { get; set; }
    public virtual DbSet<Rating> Rating { get; set; }
    public virtual DbSet<Media> Media { get; set; }
    public virtual DbSet<Productions> Productions { get; set; } = default!;
    public virtual DbSet<Genre> Genre { get; set; } = default!;
    public virtual DbSet<MediaGenres> MediaGenres { get; set; } = default!;
}