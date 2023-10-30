using MovieDb.Utility.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace MovieDb.Utility.Services.OmdbApi
{
    public class OmdbApiService : IOmdbApiService
    {
        public async Task<SearchResult?> GetMoviesByTitle(string title, int page = 1)
        {
            UriBuilder builder = new UriBuilder(Constants.OmdbApi.BaseUri);
            builder.Query = GetParameterQuery(title, builder.Query, page).ToString();
            builder.Port = -1;
            string url = builder.ToString();
            return await GetResponse<SearchResult>(url);
        }

        public async Task<MovieDetails?> GetDetailedDescription(string imdbId)
        {
            UriBuilder builder = new UriBuilder(Constants.OmdbApi.BaseUri);
            builder.Query = GetParameterQuery(imdbId, builder.Query, isSearch: false).ToString();
            builder.Port = -1;
            string url = builder.ToString();
            return await GetResponse<MovieDetails>(url);
        }

        private NameValueCollection GetParameterQuery(string movieIdentifier, string builderQuery, int pageNumber = 1, bool isSearch = true, string imdbId = "")
        {
            NameValueCollection parameterQuery = HttpUtility.ParseQueryString(builderQuery);
            parameterQuery[Constants.OmdbApi.Parameters.ApiKey] = Constants.OmdbApi.ParameterValues.ApiKey;
            if (isSearch)
            {
                parameterQuery[Constants.OmdbApi.Parameters.MovieTitleToSearch] = movieIdentifier;
                parameterQuery[Constants.OmdbApi.Parameters.PageNumber] = pageNumber.ToString();
                parameterQuery[Constants.OmdbApi.Parameters.Type] = Constants.OmdbApi.ParameterValues.Type;
            }
            else
            {
                parameterQuery[Constants.OmdbApi.Parameters.ImdbId] = movieIdentifier;
            }
            return parameterQuery;
        }

        private async Task<T?> GetResponse<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    string jsonString = response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : string.Empty;

                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        T result = JsonConvert.DeserializeObject<T>(jsonString);
                        return result;
                    }
                }
                catch
                {
                    return default;
                }
                return default;
            }
        }
    }
}
