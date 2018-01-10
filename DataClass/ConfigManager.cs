public class ConfigManager
{
        public string ConnectionString { get; set; }
        public string ChannelAccessToken { get; set; }
}

public static class ConfigProvider
{
        public static string ConnectionString { get; set; }
        public static string ChannelAccessToken { get; set; }
}