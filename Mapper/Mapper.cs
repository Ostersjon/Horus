using AutoMapper;
using Castle.Dtos;
using Castle.Models;

namespace Castle.Mapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Movie, MovieDTO>().ForMember(x => x.Poster, opt => opt.Ignore());
        CreateMap<MovieDTO, Movie>().ForMember(x => x.Poster, opt => opt.Ignore());

        CreateMap<SeriesDTO, Series>().ForMember(x => x.Poster, opt => opt.Ignore()).ForMember(x => x.Cover,o => o.Ignore());
        CreateMap<Series, SeriesDTO>().ForMember(x => x.Poster, opt => opt.Ignore()).ForMember(x => x.Cover, o => o.Ignore());

        CreateMap<Productions, ProductionsDTO>().ForMember(x => x.photo, opt => opt.Ignore());
        CreateMap<ProductionsDTO, Productions>().ForMember(x => x.photo, opt => opt.Ignore());

        CreateMap<Seasons, SeasonDTO>().ForMember(x => x.Poster, opt => opt.Ignore());
        CreateMap<SeasonDTO, Seasons>().ForMember(x => x.Poster, opt => opt.Ignore());

        CreateMap<Episode, EpisodeDTO>().ForMember(x => x.Thumbnail, opt => opt.Ignore());
        CreateMap<EpisodeDTO, Episode>().ForMember(x => x.Thumbnail, opt => opt.Ignore());

        CreateMap<Genre, GenreDTO>().ForMember(x => x.Photo, opt => opt.Ignore());
        CreateMap<GenreDTO, Genre>().ForMember(x => x.Photo, opt => opt.Ignore());

        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryDTO, Category>();
    }
}