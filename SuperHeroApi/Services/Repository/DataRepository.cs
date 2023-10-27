using Microsoft.EntityFrameworkCore;

namespace SuperHeroApi.Services.SuperHeroService
{
    public class DataRepository : IDataRepository
    {
        private readonly SuperHeroDbContext _context;
        public DataRepository(SuperHeroDbContext context)
        {
            _context = context;
        }

        

        public async Task<List<SuperHero>?> AddSuperHero(SuperHero newSuperHero)
        {
             _context.SuperHeros.Add(newSuperHero);
            await _context.SaveChangesAsync();
            return await _context.SuperHeros.ToListAsync();
        }

        public async Task<List<SuperHero>?> DeleteSuperHeroById(int id)
        {
            var hero = await _context.SuperHeros.FindAsync(id);
            if (hero is null)
            {
                return null;
            }

            _context.SuperHeros.Remove(hero);
            

            return await _context.SuperHeros.OrderBy(c => c.SuperHeroId).ToListAsync(); ;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public async Task<List<SuperHero>?> GetAllSuperHeros()
        {
            var allHeros = await _context.SuperHeros.ToListAsync();

            var allVillains = await _context.SuperVillains.ToListAsync();

            var joinResult = allHeros.Join(allVillains,
                allheros => allheros.SuperHeroId,
                allvillains => allvillains.SuperHeroId,
                (allheros, allvillains) => new
                {
                    allheros.HeroName,
                    allvillains.VillainName
                });
            return allHeros;
        }

        public async Task<SuperHero?> GetSuperHeroById(int id)
        {
            var hero = await _context.SuperHeros.FindAsync(id);
            if (hero is null)
            {
                return null;
            }
            return hero;
        }

        public async Task<List<SuperHero>?> UpdateSuperHeroById(int id, SuperHero newSuperHero)
        {
            var hero = _context.SuperHeros.Where(x => x.SuperHeroId.Equals(id)).FirstOrDefault();

            if (hero is null)
            {
                return null;
            }

            hero.HeroName = newSuperHero.HeroName;
            hero.FirstName = newSuperHero.FirstName;
            hero.LastName = newSuperHero.LastName;
            hero.SuperPowers = newSuperHero.SuperPowers;
            hero.City = newSuperHero.City;

            _=_context.SaveChangesAsync();

            return await _context.SuperHeros.ToListAsync();
        }
    }
}
