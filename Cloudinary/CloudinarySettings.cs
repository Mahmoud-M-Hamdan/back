namespace Api.Cloudinary
{
    public class CloudinarySettings
    {
        // public CloudinarySettings(string cloudName, string aPISecret, string aPIKey)
        // {
        //     this.CloudName = cloudName;
        //     this.APISecret = aPISecret;
        //     this.APIKey = aPIKey;

        // }
        public CloudinarySettings()
        {}
        public CloudinarySettings(string cloudName, string aPISecret, string aPIKey)
        {
            this.CloudName = cloudName;
            this.APISecret = aPISecret;
            this.APIKey = aPIKey;

        }
        public string CloudName { get; set; }
        public string APIKey { get; set; }
        public string APISecret { get; set; }
    }
}