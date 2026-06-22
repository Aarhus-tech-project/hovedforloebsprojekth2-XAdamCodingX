namespace ArenaStats.Shared.Models;

public class HeroDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Role { get; set; } = "";
    public int TotalPicks { get; set; }
    public int TotalBans { get; set; }
    public double PickRate { get; set; }
    public double BanRate { get; set; }
    public double PresenceRate { get; set; }
}

public class MapDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int TimesPlayed { get; set; }
    public double PlayRate { get; set; }
}

public class MatchDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Region { get; set; } = "";
    public string Stage { get; set; } = "";
    public int TotalPicks { get; set; }
    public int TotalBans { get; set; }
}

public class MatchHeroDto
{
    public int MatchId { get; set; }
    public int HeroId { get; set; }
    public string HeroName { get; set; } = "";
    public string Role { get; set; } = "";
    public string Phase { get; set; } = "";
    public int Count { get; set; }
}

public class MatchMapDto
{
    public int MatchId { get; set; }
    public int MapId { get; set; }
    public string MapName { get; set; } = "";
    public int Count { get; set; }
}