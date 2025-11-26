using System.Diagnostics;
using System.Text.Json;
using DeepseekOcrExperiments;
using Microsoft.Extensions.AI;
using OllamaSharp;
using SkiaSharp;

var imagePath = "./taxi_receipt.jpg";

Console.WriteLine($"Starting OCR of {imagePath}...");
var model = "deepseek-ocr:latest";
var prompt = "<|grounding|>Convert the document to markdown.";
var uri = new Uri("http://localhost:11434");
using IChatClient ollamaChatClient = new OllamaApiClient(uri, model);
var responseStream = ollamaChatClient.GetStreamingResponseAsync([
    new ChatMessage(ChatRole.User, [
        new TextContent(prompt),
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

Console.WriteLine($"Parsing detected bounding boxes...");
var boundingBoxes = BoundingBox.Parse(ocrResponse);
foreach (var box in boundingBoxes)
{
    Console.WriteLine(JsonSerializer.Serialize(box));
}

Console.WriteLine($"Drawing detected bounding boxes...");
using var image = SKBitmap.Decode(imagePath);
using var canvas = new SKCanvas(image);
foreach (var box in boundingBoxes)
{
    canvas.Draw(box, image.Width, image.Height);
}
var outputPath = imagePath + ".annotated.jpg";
await using var output = File.OpenWrite(outputPath);
image.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(output);
Process.Start("open", outputPath);

Console.WriteLine("Done.");
