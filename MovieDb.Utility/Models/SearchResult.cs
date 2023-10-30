using Newtonsoft.Json;

namespace MovieDb.Utility.Models
{
    public class SearchResult
    {
        [JsonProperty("Search")]
        public IList<Movie> Movies { get; set; }
        public string TotalResults { get; set; }
        public string Response { get; set; }
    }
}
