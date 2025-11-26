namespace DeepseekOcrExperiments;

[TestClass]
public sealed class BoundingBoxParserTest
{    
    [TestMethod]
    public async Task UnderstandsBoundingBoxes()
    {
        Assert.Contains("[[127, 120, 499, 253]]", OllamaTest.TaxiReceiptOcrResponse);
    }
    
    [TestMethod]
    public void ParsesBoundingBoxes()
    {
        var boundingBoxes = BoundingBox.Parse(OllamaTest.TaxiReceiptOcrResponse);
        
        Assert.IsNotEmpty(boundingBoxes, "Should parse at least one item");
        
        // Check first image item
        var firstImage = boundingBoxes[0];
        Assert.AreEqual("image", firstImage.Type);
        Assert.AreEqual(127, firstImage.Coordinates.X1);
        Assert.AreEqual(120, firstImage.Coordinates.Y1);
        Assert.AreEqual(499, firstImage.Coordinates.X2);
        Assert.AreEqual(253, firstImage.Coordinates.Y2);
        Assert.IsNull(firstImage.Value);
        
        // Check first text item
        var firstText = boundingBoxes[1];
        Assert.AreEqual("text", firstText.Type);
        Assert.AreEqual(575, firstText.Coordinates.X1);
        Assert.AreEqual(152, firstText.Coordinates.Y1);
        Assert.AreEqual(782, firstText.Coordinates.X2);
        Assert.AreEqual(192, firstText.Coordinates.Y2);
        Assert.AreEqual("**Fahrpreisquittung**", firstText.Value);
        
        // Check that text values are extracted
        var textItems = boundingBoxes.Where(i => i.Type == "text" && i.Value != null).ToList();
        Assert.IsNotEmpty(textItems, "Should have text items with values");
        Assert.IsTrue(textItems.Any(t => t.Value!.Contains("30,00")), "Should find the price");
        Assert.IsTrue(textItems.Any(t => t.Value!.Contains("Heiltsbergmoss Hotel")), "Should find the destination");
        
        // Check last item (text at end of string)
        var lastTextItem = boundingBoxes.Where(i => i.Type == "text").Last();
        Assert.AreEqual("Datum Taxi Nr.", lastTextItem.Value, "Should capture text from last item");
    }
}
