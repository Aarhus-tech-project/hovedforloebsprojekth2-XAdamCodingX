using Dapper;
using ArenaStats.Shared.Models;

namespace ArenaStats.API.Data;

// ── Heroes ────────────────────────────────────────────────────────────────────
public interface IHeroRepository
{
    Task<IEnumerable<HeroDto>> GetAllAsync(string? role = null);
    Task<HeroDto?> GetByIdAsync(int id);
}

public class HeroRepository(IDbConnectionFactory db) : IHeroRepository
{
    public async Task<IEnumerable<HeroDto>> GetAllAsync(string? role = null)
    {
        using var conn = db.Create();
        var sql = @"
            SELECT
                h.Id,
                h.Name,
                h.Role,
                h.TotalPicks,
                h.TotalBans,
                h.PickRate,
                h.BanRate,
                ROUND(h.PickRate + h.BanRate, 2) AS PresenceRate
            FROM Heroes h
            WHERE (@Role IS NULL OR h.Role = @Role)
            ORDER BY h.PickRate DESC";
        return await conn.QueryAsync<HeroDto>(sql, new { Role = role });
    }

    public async Task<HeroDto?> GetByIdAsync(int id)
    {
        using var conn = db.Create();
        var sql = @"
            SELECT Id, Name, Role, TotalPicks, TotalBans, PickRate, BanRate,
                   ROUND(PickRate + BanRate, 2) AS PresenceRate
            FROM Heroes WHERE Id = @Id";
        return await conn.QueryFirstOrDefaultAsync<HeroDto>(sql, new { Id = id });
    }
}

// ── Maps ──────────────────────────────────────────────────────────────────────
public interface IMapRepository
{
    Task<IEnumerable<MapDto>> GetAllAsync();
}

public class MapRepository(IDbConnectionFactory db) : IMapRepository
{
    public async Task<IEnumerable<MapDto>> GetAllAsync()
    {
        using var conn = db.Create();
        var sql = @"
            SELECT Id, Name, TimesPlayed, PlayRate
            FROM Maps
            ORDER BY TimesPlayed DESC";
        return await conn.QueryAsync<MapDto>(sql);
    }
}

// ── Matches ───────────────────────────────────────────────────────────────────
public interface IMatchRepository
{
    Task<IEnumerable<MatchDto>> GetAllAsync(string? region = null);
    Task<MatchDto?> GetByIdAsync(int id);
    Task<IEnumerable<MatchHeroDto>> GetHeroesForMatchAsync(int matchId);
    Task<IEnumerable<MatchMapDto>> GetMapsForMatchAsync(int matchId);
}

public class MatchRepository(IDbConnectionFactory db) : IMatchRepository
{
    public async Task<IEnumerable<MatchDto>> GetAllAsync(string? region = null)
    {
        using var conn = db.Create();
        var sql = @"
            SELECT Id, Name, Region, Stage, TotalPicks, TotalBans
            FROM Matches
            WHERE (@Region IS NULL OR Region = @Region)
            ORDER BY Id";
        return await conn.QueryAsync<MatchDto>(sql, new { Region = region });
    }

    public async Task<MatchDto?> GetByIdAsync(int id)
    {
        using var conn = db.Create();
        return await conn.QueryFirstOrDefaultAsync<MatchDto>(
            "SELECT Id, Name, Region, Stage, TotalPicks, TotalBans FROM Matches WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<IEnumerable<MatchHeroDto>> GetHeroesForMatchAsync(int matchId)
    {
        using var conn = db.Create();
        var sql = @"
            SELECT mh.MatchId, mh.HeroId, h.Name AS HeroName, h.Role, mh.Phase, mh.Count
            FROM MatchHeroes mh
            JOIN Heroes h ON h.Id = mh.HeroId
            WHERE mh.MatchId = @MatchId
            ORDER BY mh.Phase, mh.Count DESC";
        return await conn.QueryAsync<MatchHeroDto>(sql, new { MatchId = matchId });
    }

    public async Task<IEnumerable<MatchMapDto>> GetMapsForMatchAsync(int matchId)
    {
        using var conn = db.Create();
        var sql = @"
            SELECT mm.MatchId, mm.MapId, m.Name AS MapName, mm.Count
            FROM MatchMaps mm
            JOIN Maps m ON m.Id = mm.MapId
            WHERE mm.MatchId = @MatchId
            ORDER BY mm.Count DESC";
        return await conn.QueryAsync<MatchMapDto>(sql, new { MatchId = matchId });
    }
}
