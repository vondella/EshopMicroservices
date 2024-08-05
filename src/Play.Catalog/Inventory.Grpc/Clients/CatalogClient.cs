using Inventory.Grpc.Domain;

namespace Inventory.Grpc.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient _httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public  async Task<InventoryItem> GetAsync()
        {
            var items = await _httpClient.GetFromJsonAsync<InventoryItem>("/items");
            return items;
        }
    }
}
