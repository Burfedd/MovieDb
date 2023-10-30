namespace MovieDb.WebApp.Services.Storage
{
    public interface ISavedQueriesService
    {
        Task<IList<string>> GetLastSearchQueriesAsync();
        Task PushNewSearchQueryAsync(string query);
    }
}
