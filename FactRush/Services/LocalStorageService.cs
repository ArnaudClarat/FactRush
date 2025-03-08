using Microsoft.JSInterop;
using System.Text.Json;

namespace FactRush.Services
{
    public class LocalStorageService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime JsRuntime = jsRuntime;

        public async Task SetItemAsync<T>(string key, T item)
        {
            var json = JsonSerializer.Serialize(item);
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            var json = await JsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async Task RemoveItemAsync(string key)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}