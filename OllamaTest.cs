using Microsoft.Extensions.AI;
using OllamaSharp;

namespace DeepseekOcrExperiments;

[TestClass]
public sealed class OllamaTest
{
    readonly Uri ollamaUrl = new("http://localhost:11434");

    [TestMethod]
    public async Task HasModel()
    {
        using var ollama = new OllamaApiClient(ollamaUrl);
        var models = await ollama.ListLocalModelsAsync();
        Assert.IsTrue(models.Any(m => m.Name.Contains("deepseek-ocr:latest")));
    }

    [TestMethod]
    public async Task DetectsTaxiReceiptContent()
    {
        var model = "deepseek-ocr:latest";
        var prompt = "<|grounding|>Convert the document to markdown.";
        var image = "/Users/manuelnaujoks/Downloads/IMG_0384.JPEG";

        var uri = new Uri("http://localhost:11434");
        using IChatClient ollamaChatClient = new OllamaApiClient(uri, model);
        var responseStream = ollamaChatClient.GetStreamingResponseAsync([
            new ChatMessage(ChatRole.User, [ 
                new TextContent(prompt),
                new DataContent(File.ReadAllBytes(image), "image/jpeg")
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

        Assert.Contains(@"**info@taxieco/unterschleissheim.de**", ocrResponse);
    }
}
