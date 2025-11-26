using System.Text.RegularExpressions;

namespace DeepseekOcrExperiments;

public static class BoundingBoxParser
{
    private static readonly Regex ItemPattern = new(
        @"<\|ref\|>(?<type>\w+)<\|/ref\|><\|det\|>\[\[(?<x1>\d+),\s*(?<y1>\d+),\s*(?<x2>\d+),\s*(?<y2>\d+)\]\]<\|/det\|>\s*(?<content>.*?)(?=<\|ref\||$)",
        RegexOptions.Compiled | RegexOptions.Singleline);

    extension(BoundingBox)
    {
        public static List<BoundingBox> Parse(string ocrResponse)
        {
            var matches = ItemPattern.Matches(ocrResponse);

            return [.. matches.Select(match =>
            {
                var type = match.Groups["type"].Value;
                var box = new BoundingBox.BoxCoordinates(
                    int.Parse(match.Groups["x1"].Value),
                    int.Parse(match.Groups["y1"].Value),
                    int.Parse(match.Groups["x2"].Value),
                    int.Parse(match.Groups["y2"].Value)
                );

                var text = match.Groups["content"].Value.Trim();
                var value = type == "text" && !string.IsNullOrWhiteSpace(text) ? text : null;

                return new BoundingBox(box, type, value);
            })];
        }
    }
}
