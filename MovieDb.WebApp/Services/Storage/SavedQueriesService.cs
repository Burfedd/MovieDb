using Blazored.LocalStorage;

namespace MovieDb.WebApp.Services.Storage
{
    public class SavedQueriesService : ISavedQueriesService
    {
        private ILocalStorageService _storageService;

        public SavedQueriesService(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<IList<string>> GetLastSearchQueriesAsync()
        {
            IList<string> result = await _storageService.GetItemAsync<IList<string>>(Constants.LocalStorage.Key);
            if (result != null)
            {
                if (result.Count > 5)
                {
                    return result.Take(5).ToList();
                }
                return result;
            }
            else
            {
                return new List<string>();
            }
        }

        public async Task PushNewSearchQueryAsync(string query)
        {
            IList<string> queries = await _storageService.GetItemAsync<IList<string>>(Constants.LocalStorage.Key);
            if (queries != null)
            {
                queries.Insert(0, query);
                if (queries.Count > 5)
                {
                    queries = queries.Take(5).ToList();
                }
                await _storageService.SetItemAsync(Constants.LocalStorage.Key, queries);
            }
            else
            {
                IList<string> newQueries = new List<string> { query };
                await _storageService.SetItemAsync(Constants.LocalStorage.Key, newQueries);
            }
        }
    }
}
