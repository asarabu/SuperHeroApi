using AutoMapper;

namespace SuperHeroApi.Profiles
{
    public class SuperHeroProfile : Profile
    {
        public SuperHeroProfile()
        {
            CreateMap<SuperHero, ViewModels.SuperHeroDto>();
            CreateMap<SuperVillain, ViewModels.SuperVillainDto>();
            CreateMap<CharacterStory, ViewModels.CharacterStoryDto>();
        }
    }
}
