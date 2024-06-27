using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ReversiMvcApp.Services
{
    public interface IReCaptchaValidatorService
    {
        Task<(bool isValid, string errorMessage)> TryValidateReCaptchaResponse(HttpContext context, string recaptchaResponse);
    }

    public class ReCaptchaValidatorService(IConfiguration configuration, ILogger<ReCaptchaValidatorService> logger) : IReCaptchaValidatorService
    {
        private static readonly HttpClient client = new();
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger logger = logger;

        public async Task<(bool isValid, string errorMessage)> TryValidateReCaptchaResponse(HttpContext context, string recaptchaResponse)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                    {
                        {"secret", configuration["reCAPTCHA:SecretKey"]},
                        {"response", recaptchaResponse},
                        {"remoteip", context.Connection.RemoteIpAddress.ToString()}
                    };

                HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);

                if (apiJson.success != true)
                {
                    return (false, "There was an unexpected problem processing this request. Please try again.");
                }

            } catch (Exception ex)
            {
                logger.LogWarning(ex.Message);
                return (false, "There was an unexpected problem processing this request. Please try again.");
            }

            return (true, null);
        }
    }
}
