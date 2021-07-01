namespace BlockchainDemonstratorApi.Models.Classes
{
    /// <summary>
    /// This method is used to currently determine the URL of the RestApiUrl.
    /// This is because this URL can change between the debug and release environment.
    /// This class is used to easily determine the what URL to use, instead of having to change it manually when switching between the environments.
    /// </summary>
    /// <remarks>Other URL's can be added if need be when they change between the debug and release environment.</remarks>
    public static class Config
    {
        public static string RestApiUrl { get; set; }
        public static string ServerIp { get; set; }    
    }
}
