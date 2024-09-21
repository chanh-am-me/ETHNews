using System.Text.RegularExpressions;

namespace Infrastructure.Extensions;

public static partial class RegexExtension
{
    public static readonly Regex TenMilionSubpplyRegex = TenMilionSubpplyRg();

    [GeneratedRegex("(Supply(\\s*):)(\\s*)(10,000,000)(\\s*)((\\+(\\s*)9decimals))")]
    private static partial Regex TenMilionSubpplyRg();
}
