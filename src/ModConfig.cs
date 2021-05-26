using Vintagestory.API.Common;

namespace SinglePause
{
    public class ModConfig
    {
        public bool TODO = true;
        public static string filename = "SinglePause.json";
        public static ModConfig Load(ICoreAPI api)
        {
            var config = api.LoadModConfig<ModConfig>(filename);
            if (config == null)
            {
                config = new ModConfig();
                Save(api, config);
            }
            return config;
        }
        public static void Save(ICoreAPI api, ModConfig config)
        {
            api.StoreModConfig(config, filename);
        }
    }
}
