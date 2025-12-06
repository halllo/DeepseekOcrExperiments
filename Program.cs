using System.Diagnostics;
using System.Text.Json;
using DeepseekOcrExperiments;
using Microsoft.Extensions.AI;
using OllamaSharp;
using SkiaSharp;

//var imagePath = "./taxi_receipt.jpg";
var imagePath = "./recipe.jpg";

// Initialize Ollama client
Console.WriteLine($"Starting OCR of {imagePath}...");
using IChatClient ollamaChatClient = new OllamaApiClient(
    uri: new Uri("http://localhost:11434"),
    defaultModel: "deepseek-ocr:latest");

// OCR image
var responseStream = ollamaChatClient.GetStreamingResponseAsync([
    new ChatMessage(ChatRole.User, [
        new TextContent((string?)"<|grounding|>Convert the document to markdown."),
        new DataContent(File.ReadAllBytes(imagePath), "image/jpeg")
    ])
]);

List<ChatResponseUpdate> responseUpdates = [];
await foreach (ChatResponseUpdate update in responseStream)
{
    responseUpdates.Add(update);
    foreach (var content in update.Contents)
    {
        switch (content)
        {
            case TextContent textContent:
                Console.Write(textContent.Text);
                break;
            default:
                Console.Write($"[Unsupported content type: {content.GetType().Name}]");
                break;
        }
    }
}
var chatResponse = responseUpdates.ToChatResponse();
var ocrResponse = chatResponse.Messages.Single().Text;

// Parse bounding boxes
Console.WriteLine($"Parsing detected bounding boxes...");
var boundingBoxes = BoundingBox.Parse(ocrResponse);
foreach (var box in boundingBoxes)
{
    Console.WriteLine(JsonSerializer.Serialize(box));
}

// Draw bounding boxes
Console.WriteLine($"Drawing detected bounding boxes...");
using var image = SKBitmap.Decode(imagePath);
using var canvas = new SKCanvas(image);
foreach (var box in boundingBoxes)
{
    canvas.Draw(box, image.Width, image.Height);
}

// Save output image
var outputPath = imagePath + ".annotated.jpg";
await using var output = File.OpenWrite(outputPath);
image.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(output);
Process.Start("open", outputPath);

Console.WriteLine("Done.");
