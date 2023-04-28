namespace Castle.Helpers;

public class Helpers:IHelpers
{
    private readonly IWebHostEnvironment _env;
    public Helpers(IWebHostEnvironment environment)
    {
        _env = environment;
    }

    public string ImgToStr(IFormFile img)
    {
        var ImageName = Guid.NewGuid().ToString() + ".png";
        var foldername = Path.Combine(_env.WebRootPath, "images");
        var FullPath = Path.Combine(foldername, ImageName);
        img.CopyTo(new FileStream(FullPath, FileMode.Create));
        return ImageName;
    }
    public string VideoToStr(IFormFile vid)
    {
        var VideoName = Guid.NewGuid().ToString() + ".mp4";
        var foldername = Path.Combine(_env.WebRootPath, "videos");
        var FullPath = Path.Combine(foldername, VideoName);
        vid.CopyTo(new FileStream(FullPath, FileMode.Create));
        return VideoName;
    }
}