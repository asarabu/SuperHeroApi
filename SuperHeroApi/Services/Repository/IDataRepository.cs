
namespace SuperHeroApi.Services.SuperHeroService
{
    public interface IDataRepository : IDisposable
    {
        Task<List<SuperHero>?> GetAllSuperHeros();
        Task<SuperHero?> GetSuperHeroById(int id);
        Task<List<SuperHero>?> AddSuperHero(Models.SuperHero newSuperHero);
        Task<List<SuperHero>?> UpdateSuperHeroById(int id, Models.SuperHero newSuperHero);
        Task<List<SuperHero>?> DeleteSuperHeroById(int id);
    }
}
