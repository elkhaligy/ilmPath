using IlmPath.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers
{
    public class RouterApiRequestMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    // Represents the full request body sent to the Hugging Face Router's /chat/completions endpoint.
    // This structure directly mirrors the OpenAI Chat Completions API.
    public class RouterApiCompletionRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } // The name of the model to use (e.g., "baidu/ernie-4.5-21b-a3b")

        [JsonPropertyName("messages")]
        public List<RouterApiRequestMessage> Messages { get; set; } // A list of chat messages

        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false; // Indicates if the response should be streamed (set to false for non-streaming)
    }

    // --- DTOs for the OpenAI-compatible API Response (received from Hugging Face Router) ---

    // Represents the 'message' object within a 'choice' in the API response.
    // Example: { "role": "assistant", "content": "Hello! How can I help you?" }
    public class RouterApiResponseMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    // Represents a single 'choice' object within the 'choices' array of the API response.
    public class RouterApiChoice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; } // The index of the choice in the list

        [JsonPropertyName("message")]
        public RouterApiResponseMessage Message { get; set; } // The actual message from the AI

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } // The reason the model stopped generating (e.g., "stop", "length")
    }

    // Represents the full response body received from the Hugging Face Router's /chat/completions endpoint.
    public class RouterApiCompletionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } // Unique ID for the completion

        [JsonPropertyName("object")]
        public string Object { get; set; } // Type of object (e.g., "chat.completion")

        [JsonPropertyName("created")]
        public long Created { get; set; } // Timestamp of creation

        [JsonPropertyName("model")]
        public string Model { get; set; } // The model that generated the response

        [JsonPropertyName("choices")]
        public List<RouterApiChoice> Choices { get; set; } // List of generated choices

        // You can uncomment and add a 'usage' property if the API returns token usage details.
        // [JsonPropertyName("usage")]
        // public object Usage { get; set; }
    }

    // --- DTO for Frontend Request ---
    // This DTO models the request body that is sent from your Angular frontend
    // to this ASP.NET Core backend proxy.
    public class FrontendInferenceRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } // The AI model name requested by the frontend (e.g., "baidu/ernie-4.5-21b-a3b")

        [JsonPropertyName("inputs")] // This property name matches the 'inputs' field from your Angular frontend's body
        public string Inputs { get; set; } // The user's message/prompt as a simple string
    }


    [ApiController] // Marks this class as an API controller
    [Route("api/[controller]")] // Defines the base route for this controller (e.g., /api/AiChat)
    public class AiChatController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory; // Used to create HttpClient instances
        private readonly string _huggingFaceApiKey; // Stores the Hugging Face API token securely

        // Constructor for dependency injection.
        // IHttpClientFactory is injected to create HttpClients.
        // IConfiguration is injected to access application settings (like API keys).
        public AiChatController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            // Retrieve the Hugging Face API key from your application's configuration.
            // This key should be stored securely (e.g., in User Secrets for development,
            // or environment variables for production).
            _huggingFaceApiKey = config["HuggingFace:ApiKey"];

            // Basic validation: If the API key is missing, throw an exception on startup
            // to prevent runtime errors later.
            if (string.IsNullOrEmpty(_huggingFaceApiKey))
            {
                throw new InvalidOperationException("HuggingFace:ApiKey configuration is missing or empty. Please ensure it's set correctly.");
            }
        }

        [HttpPost] // This action method handles HTTP POST requests to /api/AiChat
        public async Task<IActionResult> Post([FromBody] FrontendInferenceRequestDto requestDto)
        {
            // Validate the incoming request from the Angular frontend.
            // Ensure the model name and user input are provided.
            if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.Model) || string.IsNullOrWhiteSpace(requestDto.Inputs))
            {
                return BadRequest("Invalid request body. 'model' and 'inputs' are required and should be strings.");
            }

            // Create an HttpClient instance using the factory.
            var httpClient = _httpClientFactory.CreateClient();

            // Define the target URL for the Hugging Face Inference Router's OpenAI-compatible endpoint.
            var routerApiUrl = "https://router.huggingface.co/novita/v3/openai/chat/completions";

            // Construct the request body that will be sent to the Hugging Face Router.
            // This must match the RouterApiCompletionRequest DTO structure.
            var routerRequestBody = new RouterApiCompletionRequest
            {
                Model = requestDto.Model, // Use the model name from the frontend request
                Messages = new List<RouterApiRequestMessage>
            {
                new RouterApiRequestMessage { Role = "user", Content = requestDto.Inputs } // Use the user's message
            },
                Stream = false // We are expecting a non-streaming response
            };

            // Serialize the C# request object to a JSON string.
            var requestBodyJson = JsonSerializer.Serialize(routerRequestBody);

            // Create the HttpRequestMessage that will be sent to the Hugging Face Router.
            var request = new HttpRequestMessage(HttpMethod.Post, routerApiUrl)
            {
                Content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json")
            };

            // Set the Authorization header with the Hugging Face API key as a Bearer token.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _huggingFaceApiKey);

            try
            {
                // Send the HTTP request to the Hugging Face Router.
                var response = await httpClient.SendAsync(request);
                // Read the full response content as a string.
                var content = await response.Content.ReadAsStringAsync();

                // Check if the response from the Hugging Face Router was successful (2xx status code).
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the successful JSON response into our C# DTOs.
                    var completionResponse = JsonSerializer.Deserialize<RouterApiCompletionResponse>(content);

                    // Extract the generated text from the first choice of the AI's response.
                    var generatedText = completionResponse?.Choices?.FirstOrDefault()?.Message?.Content;

                    // Handle cases where the AI might not return any content.
                    if (string.IsNullOrEmpty(generatedText))
                    {
                        return Ok(new { generatedText = "AI did not provide a clear response." });
                    }

                    // Return the generated text to the Angular frontend in the expected format.
                    return Ok(new { generatedText = generatedText });
                }
                else
                {
                    // If the router returned an error (e.g., 400, 401, 500), log it and forward the status code and content.
                    Console.WriteLine($"Hugging Face Router API Error: {response.StatusCode} - {content}");
                    try
                    {
                        // Attempt to deserialize the error content as JSON for better frontend debugging.
                        var errorJson = JsonSerializer.Deserialize<JsonElement>(content);
                        return StatusCode((int)response.StatusCode, errorJson);
                    }
                    catch
                    {
                        // If content is not valid JSON, return it as plain text.
                        return StatusCode((int)response.StatusCode, content);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Catch network-level errors (e.g., DNS resolution failure, connection timeout to router).
                Console.WriteLine($"HttpRequestException to Hugging Face Router: {ex.Message}");
                return StatusCode(503, "Service Unavailable: Unable to connect to AI provider.");
            }
            catch (JsonException ex)
            {
                // Catch errors during JSON deserialization of the router's response.
                // This happens if the router returns malformed JSON.
                Console.WriteLine($"JSON Deserialization Error from Router: {ex.Message}");
                return StatusCode(500, "Internal Server Error: Failed to parse AI response format.");
            }
            catch (Exception ex)
            {
                // Catch any other unexpected internal server errors.
                Console.WriteLine($"An unexpected internal server error occurred: {ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
