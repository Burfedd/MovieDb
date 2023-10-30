using MovieDb.Utility.Models;

namespace MovieDb.Utility.Services.OmdbApi
{
    public interface IOmdbApiService
    {
        Task<SearchResult?> GetMoviesByTitle(string title, int page = 1);
        Task<MovieDetails?> GetDetailedDescription(string imdbId);
    }
}
