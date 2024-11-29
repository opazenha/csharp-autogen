using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using BiblicalTriviaApi.Models;

namespace BiblicalTriviaApi.Services
{
    public class TriviaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<TriviaService> _logger;

        public TriviaService(string apiKey, ILogger<TriviaService> logger)
        {
            _apiKey = apiKey;
            _logger = logger;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1/")
            };
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
        }

        public async Task<TriviaQuestion> GenerateQuestionAsync(string? category = null, string? difficulty = null)
        {
            var prompt = CreatePrompt(category, difficulty);
            
            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7f,
                    topP = 0.8f,
                    topK = 40,
                    maxOutputTokens = 1024
                }
            };

            var response = await _httpClient.PostAsync(
                "models/gemini-1.5-flash:generateContent",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Raw response: {JsonResponse}", jsonResponse);

            try
            {
                // Parse the nested JSON response to get the actual question JSON
                using var doc = JsonDocument.Parse(jsonResponse);
                var generatedText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // Strip out code block markers if present
                if (generatedText.StartsWith("```json"))
                {
                    generatedText = generatedText.Substring(7).Trim();
                }
                if (generatedText.EndsWith("```"))
                {
                    generatedText = generatedText.Substring(0, generatedText.Length - 3).Trim();
                }

                _logger.LogInformation("Generated text: {GeneratedText}", generatedText);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var question = JsonSerializer.Deserialize<TriviaQuestion>(generatedText, options);
                if (question == null)
                {
                    throw new Exception("Failed to deserialize question");
                }

                return question;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse trivia question");
                throw new Exception($"Failed to parse trivia question: {ex.Message}");
            }
        }

        private string CreatePrompt(string? category, string? difficulty)
        {
            return $@"Generate a biblical trivia question in the following JSON format:
{{
    ""question"": ""[Question text]"",
    ""options"": [""[Option 1]"", ""[Option 2]"", ""[Option 3]"", ""[Option 4]""],
    ""correctAnswerIndex"": [0-3],
    ""explanation"": ""[Explanation of the correct answer]"",
    ""category"": ""{category ?? "General"}"",
    ""difficulty"": ""{difficulty ?? "Medium"}"",
    ""reference"": ""[Biblical reference]""
}}

Make sure the question is {difficulty ?? "medium"} difficulty and related to {category ?? "any biblical topic"}. 
The question should be engaging and educational. 
The explanation should provide context and biblical references.
Ensure all options are plausible but only one is correct.";
        }
    }
}
