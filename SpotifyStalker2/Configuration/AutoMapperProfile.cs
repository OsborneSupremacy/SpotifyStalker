using AutoMapper;
using Spotify.Model;
using Spotify.Object;

namespace SpotifyStalker2.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Artist, ArtistModel>();
        }
    }
}
