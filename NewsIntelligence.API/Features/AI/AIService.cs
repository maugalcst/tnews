using System.Text.Json;
using NewsIntelligence.API.Infrastructure;

namespace NewsIntelligence.API.Features.AI
{
    public record OllamaRequest(
        string Model, 
        string Prompt, 
        bool Stream
    );

    public record OllamaResponse(
        string Response
    );

    public class AIService
    {
        private readonly AppDbContext _context;

        public AIService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateSummaryAsync(string articleText)
        {
            string summaryPrompt = $"You are an AI agent specialized in creating highly accurate news summaries. Your only objective is to read the provided article and extract the most critical information in a 100% impartial and objective manner. STRICT RULES: The summary must be in English. The maximum length must be exactly 2 sentences (maximum 40 words). DO NOT include greetings, introductions such as 'Here is the summary', or closing remarks. Return ONLY AND EXCLUSIVELY the summary text. This is the article you will summarize: {articleText}";

            var requestData = new OllamaRequest("llama3", summaryPrompt, false);

            string jsonContent = JsonSerializer.Serialize(requestData);

            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            client.Timeout = TimeSpan.FromMinutes(10);

            var response = await client.PostAsync("http://localhost:11434/api/generate", httpContent);

            string responseStringJson = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseStringJson))
                throw new ArgumentNullException(responseStringJson);
                
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            OllamaResponse responseObj = JsonSerializer.Deserialize<OllamaResponse>(responseStringJson, options);

            return responseObj.Response;

        }
    }
}