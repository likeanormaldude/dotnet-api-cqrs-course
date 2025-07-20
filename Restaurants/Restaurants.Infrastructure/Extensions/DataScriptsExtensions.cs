using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Extensions;

public static class DataScriptsExtensions
{
    /// <summary>
    /// Based on one script name, checks if the script has already been run.
    /// </summary>
    /// <param name="historyDbSet"></param>
    /// <param name="scriptName"></param>
    /// <returns></returns>
    public static bool IsScriptRun(this DbSet<DataScriptHistory> historyDbSet, string scriptName) =>
        historyDbSet.Any(x => x.ScriptName.ToUpperInvariant() == scriptName.ToUpperInvariant());
}
