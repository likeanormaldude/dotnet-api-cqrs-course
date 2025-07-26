namespace Restaurants.Domain.Entities;

public class DataScriptHistory
{
    public int Id { get; set; }
    public string ScriptName { get; set; } = default!;
    public DateTime RanAt { get; set; }
}
