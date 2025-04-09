using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Product.Service.Interfaces;

namespace Product.Service
{
    public class BrasilApiCpfCnpjService : ICpfCnpjService
    {
        private readonly HttpClient _httpClient;

        public BrasilApiCpfCnpjService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://brasilapi.com.br/api/");
        }

        public bool Validar(string cpfCnpj)
        {
            try
            {
                var response = _httpClient.GetAsync($"cpfcnpj/{cpfCnpj}").Result;

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return false;

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<BrasilApiResponse>(content);

                return result?.Status == "OK";
            }
            catch
            {
                return false;
            }
        }

        public string Formatar(string cpfCnpj)
        {
            return cpfCnpj?.Trim()
                .Replace(".", "")
                .Replace("-", "")
                .Replace("/", "");
        }

        private class BrasilApiResponse
        {
            public string Status { get; set; }
        }
    }
}
