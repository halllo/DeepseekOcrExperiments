using System.Diagnostics;
using SkiaSharp;

namespace DeepseekOcrExperiments;

[TestClass]
public sealed class BoundingBoxDrawerTest
{
    [TestMethod]
    public async Task DrawOneBoundingBox()
    {
        // //<|ref|>text<|/ref|><|det|>[[575, 152, 782, 192]]<|/det|>
        // **Fahrpreisquittung**
        var boundingBox = new BoundingBox(new BoundingBox.BoxCoordinates(575, 152, 782, 192), "text", "**Fahrpreisquittung**");

        using var image = SKBitmap.Decode("./taxi_receipt.jpg");
        using var canvas = new SKCanvas(image);

        canvas.Draw(boundingBox, image.Width, image.Height);

        var outputPath = "./annotated-1.jpg";
        await using var output = File.OpenWrite(outputPath);
        image.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(output);
        Process.Start("open", outputPath);
    }
    
    [TestMethod]
    public async Task DrawAllBoundingBoxes()
    {
        var boundingBoxes = BoundingBox.Parse(OllamaTest.TaxiReceiptOcrResponse);

        using var image = SKBitmap.Decode("./taxi_receipt.jpg");
        using var canvas = new SKCanvas(image);
        foreach (var box in boundingBoxes)
        {
            canvas.Draw(box, image.Width, image.Height);
        }

        var outputPath = "./annotated-all.jpg";
        await using var output = File.OpenWrite(outputPath);
        image.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(output);
        Process.Start("open", outputPath);
    }
}
