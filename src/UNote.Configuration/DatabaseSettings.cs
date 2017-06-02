using U.Settings;

namespace UNote.Configuration
{
    [USettingsPathArribute("DatabaseSettings.json", "/Config/UNote/")]
    public class DatabaseSettings : USettings<DatabaseSettings>
    {
        public string SqlConnectionString { get; set; }
    }
}
