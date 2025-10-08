using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using Louisepizdon.Platform;
using Louisepizdon.Tracing;

namespace Louisepizdon.Artificial;

public interface IAIAgent
{
    Task<string> AnalyzeImageAsync(string imageUrl);
}

public class VisionAgent : IAIAgent
{
    private readonly AIAgent _agent;
    private readonly AppConfig _config;
    private readonly IAppLogger _logger;

    public VisionAgent(AppConfig config, IAppLogger logger)
    {
        _config = config;
        _logger = logger;

        try
        {
            _logger.Info("Initializing OpenAI client with model: {Model}", config.Artificial.ChatModel);
            
            var client = new OpenAIClient(config.Artificial.OpenAIToken);
            var chatClient = client.GetChatClient(config.Artificial.ChatModel);

            var instructions = config.Artificial.GetVisionPrompt();
            
            _agent = chatClient.CreateAIAgent(
                name: "VisionAgent",
                instructions: instructions
            );

            _logger.Info("Vision agent initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to initialize vision agent", ex);
            throw;
        }
    }

    public async Task<string> AnalyzeImageAsync(string imageUrl)
    {
        try
        {
            _logger.Info("Analyzing image: {ImageUrl}", imageUrl);

            var message = new ChatMessage(ChatRole.User, [
                new TextContent("Проанализируй это изображение и составь ценовой разбор."),
                new UriContent(imageUrl, "image/jpeg")
            ]);

            var result = await _agent.RunAsync(message);
            
            _logger.Info("Image analysis completed successfully");
            
            return result.Text ?? "Не удалось проанализировать изображение.";
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to analyze image", ex);
            throw;
        }
    }
}