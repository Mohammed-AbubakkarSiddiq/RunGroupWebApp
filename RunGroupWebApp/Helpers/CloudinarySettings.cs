namespace RunGroupWebApp.Helpers
{
    //Photo upload feature step 3: Create a helper to keep cloud configuration.
    public class CloudinarySettings
    {
        public string CloudName { get; set; } = string.Empty;
        public string APIKey { get; set; } = string.Empty;
        public string APISecret { get; set; } = string.Empty;
    }
}
