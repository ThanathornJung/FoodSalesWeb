using FoodSalesAPI.Models;
using FoodSalesWebApp.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FoodSalesWebApp.Services
{
    public class FoodSalesApiService
    {
        private readonly HttpClient _httpClient;

        public FoodSalesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7248/");
        }

        public async Task<List<FoodSale>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7248/api/FoodSales");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<FoodSale>>();
        }

        public async Task<FoodSale> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<FoodSale>($"https://localhost:7248/api/FoodSales/{id}");
        }

        public async Task CreateAsync(FoodSale newSale)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7248/api/FoodSales", newSale);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int row, FoodSale updatedSale)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7248/api/FoodSales/{row}", updatedSale);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int row)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7248/api/FoodSales/{row}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<FoodSale>> FilterByDateAsync(DateOnly orderDate)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7248/api/FoodSales/filter?orderDate={orderDate:MM-dd-yyyy}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<FoodSale>>();
        }
    }
}