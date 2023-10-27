using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.Services.SuperHeroService;
using SuperHeroApi.ViewModels;

namespace SuperHeroApi.Controllers
{
    //[Authorize]
    [EnableCors("SuperHeroOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IDataRepository _superHeros;
        private readonly IMapper _mapper;

        public SuperHeroController(IDataRepository superHeros, IMapper mapper)
        {
            _superHeros = superHeros;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroNames()
        {
             return Ok(await _superHeros.GetAllSuperHeros());
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroDto>> GetSingleHeroName(int id)
        { 
            var hero = await _superHeros.GetSuperHeroById(id);
            if (hero is null) { return NotFound("Sorry, the hero doesn't exist...!"); }
            return Ok(_mapper.Map<SuperHeroDto>(hero)); 
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHeroDto>>> AddHeroToList(SuperHero hero)
        {

            var result = await _superHeros.AddSuperHero(hero);

            return Ok(_mapper.Map<IEnumerable<SuperHeroDto>>(result));
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHeroDto>>> UpdateHeroList(int id, SuperHero request)
        {
            var hero = await _superHeros.UpdateSuperHeroById(id, request);

            

            if (hero is null)
            {
                return NotFound("Required hero doesn't exist..!");
            }

             return Ok(_mapper.Map<IEnumerable<SuperHeroDto>>(hero)); ;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHeroDto>>> DeleteHero(int id)
        {
            var result = await _superHeros.DeleteSuperHeroById(id);
            return Ok(_mapper.Map<IEnumerable<SuperHeroDto>>(result));
        }
    }
}

