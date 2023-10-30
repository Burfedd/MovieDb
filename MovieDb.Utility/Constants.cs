namespace MovieDb.Utility
{
    public struct Constants
    {
        public struct OmdbApi
        {
            public const string BaseUri = "https://www.omdbapi.com";
            
            public struct Parameters
            {
                public const string ApiKey = "apikey";
                public const string MovieTitleToSearch = "s";
                public const string PageNumber = "page";
                public const string Type = "type";
                public const string ImdbId = "i";
            }

            public struct ParameterValues
            {
                public const string ApiKey = "baad3474";
                public const string Type = "movie";
            }
        }
    }
}
