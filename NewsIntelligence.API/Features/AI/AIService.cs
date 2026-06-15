using System.Text.Json;
using System.Text.RegularExpressions;
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
        private readonly HttpClient _client;

        public AIService(AppDbContext context, HttpClient client)
        {
            _context = context;
            _client = client;
            _client.Timeout = TimeSpan.FromMinutes(10);
        }

        public async Task<string> GenerateSummaryAsync(string articleText)
        {
            string summaryPrompt = $"""
            You are an AI agent specialized in creating highly accurate news summaries. 
            Your only objective is to read the provided article and extract the most critical information in a 100% impartial and objective manner. 
            STRICT RULES: The summary must be in English. The maximum length must be exactly 2 sentences (maximum 40 words). 
            Return ONLY AND EXCLUSIVELY the summary text. An example of the transformation of the information would be:

            ----------------------------
            ## Article to summarize ##
            GlobalTech Inc. announced today the release of their new quantum processor, the QX-100, during their annual summit in Tokyo. The CEO stated this chip is 50 times faster than current models and will revolutionize cloud computing. It will be available for enterprise customers starting next November for $5,000 per unit.

            ## AI summary ##
            GlobalTech Inc. unveiled the QX-100 quantum processor in Tokyo, which promises speeds 50 times faster than current models. The chip will be available to enterprise customers next November for $5,000.
            ----------------------------

            ## Article to summarize ##
            {articleText}

            ## AI summary ##
            """;

            var requestData = new OllamaRequest("llama3", summaryPrompt, false);
            string jsonContent = JsonSerializer.Serialize(requestData);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("http://localhost:11434/api/generate", httpContent);
            string responseStringJson = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseStringJson))
                throw new ArgumentNullException(nameof(responseStringJson));
                
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            OllamaResponse? responseObj = JsonSerializer.Deserialize<OllamaResponse>(responseStringJson, options);

            if (responseObj?.Response is null)
                throw new InvalidOperationException("Invalid response from AI service.");

            var cleanResponse = Regex.Replace(responseObj.Response, @"^(Here is.*?summary:?\s*|Summary:?\s*)", "", RegexOptions.IgnoreCase).Trim();

            return cleanResponse;

        }
    }
}