using Microsoft.AspNetCore.Mvc;
using ArenaStats.API.Data;

namespace ArenaStats.API.Controllers;

// ── Heroes ────────────────────────────────────────────────────────────────────
[ApiController]
[Route("api/[controller]")]
public class HeroesController(IHeroRepository repo) : ControllerBase
{
    // GET /api/heroes
    // GET /api/heroes?role=tank
    // GET /api/heroes?role=damage
    // GET /api/heroes?role=support
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? role)
        => Ok(await repo.GetAllAsync(role));

    // GET /api/heroes/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var hero = await repo.GetByIdAsync(id);
        return hero is null ? NotFound() : Ok(hero);
    }
}

// ── Maps ──────────────────────────────────────────────────────────────────────
[ApiController]
[Route("api/[controller]")]
public class MapsController(IMapRepository repo) : ControllerBase
{
    // GET /api/maps
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await repo.GetAllAsync());
}

// ── Matches ───────────────────────────────────────────────────────────────────
[ApiController]
[Route("api/[controller]")]
public class MatchesController(IMatchRepository repo) : ControllerBase
{
    // GET /api/matches
    // GET /api/matches?region=EUROPE
    // GET /api/matches?region=NORTH%20AMERICA
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? region)
        => Ok(await repo.GetAllAsync(region));

    // GET /api/matches/3
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var match = await repo.GetByIdAsync(id);
        if (match is null) return NotFound();

        var heroes = await repo.GetHeroesForMatchAsync(id);
        var maps   = await repo.GetMapsForMatchAsync(id);

        return Ok(new { Match = match, Heroes = heroes, Maps = maps });
    }
}
