using System.Text.RegularExpressions;

namespace DeepseekOcrExperiments;

public record BoundingBox(int X1, int Y1, int X2, int Y2);

public record BoundingBoxItem(BoundingBox Box, string Type, string? Value = null);

public static class BoundingBoxParser
{
    private static readonly Regex ItemPattern = new(
        @"<\|ref\|>(?<type>\w+)<\|/ref\|><\|det\|>\[\[(?<x1>\d+),\s*(?<y1>\d+),\s*(?<x2>\d+),\s*(?<y2>\d+)\]\]<\|/det\|>\s*(?<content>.*?)(?=<\|ref\||$)", 
        RegexOptions.Compiled | RegexOptions.Singleline);
    
    public static List<BoundingBoxItem> Parse(string ocrResponse)
    {
        var matches = ItemPattern.Matches(ocrResponse);
        
        return matches.Select(match =>
        {
            var type = match.Groups["type"].Value;
            var box = new BoundingBox(
                int.Parse(match.Groups["x1"].Value),
                int.Parse(match.Groups["y1"].Value),
                int.Parse(match.Groups["x2"].Value),
                int.Parse(match.Groups["y2"].Value)
            );
            
            var text = match.Groups["content"].Value.Trim();
            var value = type == "text" && !string.IsNullOrWhiteSpace(text) ? text : null;
            
            return new BoundingBoxItem(box, type, value);
        }).ToList();
    }
}
