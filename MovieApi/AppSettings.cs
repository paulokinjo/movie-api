namespace MovieApi
{
    public class AppSettings
    {
        public string ApiSecret { get; set; }

        public AppSettings()
        {
            ApiSecret = string.Empty;
        }
    }
}
