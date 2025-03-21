﻿namespace FactRush.Services
{
    public interface ILocalStorageService
    {
        Task SetItemAsync<T>(string key, T item);
        Task<T?> GetItemAsync<T>(string key);
        Task RemoveItemAsync(string key);
    }
}
