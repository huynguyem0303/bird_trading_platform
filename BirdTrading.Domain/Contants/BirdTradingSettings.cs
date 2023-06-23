using System.Text.Json;

#nullable disable warnings
namespace BirdTrading.Domain.Contants
{
    public static class BirdTradingSettings
    {
        public static Dictionary<string, object> Settings { get; set; }

        static BirdTradingSettings()
        {
            string parentPath = Path.GetDirectoryName(typeof(BirdTradingSettings).Assembly.Location);
            string path = Path.Combine(parentPath, "Contants", "BirdTradingSettings.json");
            string dict = File.ReadAllText(path);
            Settings = JsonSerializer.Deserialize<Dictionary<string, object>>(dict);
        }
    }
}
