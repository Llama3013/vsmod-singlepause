using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.API.Config;
using System.Reflection;

[assembly: ModInfo("SinglePause",
    Description = "A quick mod made to pause the game when you open handbook on single player.",
    Website = "https://github.com/Llama3013/vsmod-singlepause",
    Authors = new[] { "llama3013" })]

namespace SinglePause
{

    public class API
    {
        public static ICoreAPI api;
        public static ClientCoreAPI capi;
    }

    public class SinglePauseMod : ModSystem
    {
        private ModConfig config;

        public override void Start(ICoreAPI api)
        {
            API.api = api;
            API.capi = api as ClientCoreAPI;
            api.Logger.Debug("[Pause] Start");
            base.Start(api);

            config = ModConfig.Load(api);

            var harmony = new Harmony("llama3013.SinglePause");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }
    }


    [HarmonyPatch(typeof(GuiDialogHandbook), "OnGuiOpened")]
    public class Patch_GuiDialogHandbook_OnGuiOpened
    {
        static void Postfix()
        {
            if (API.api != null && !API.capi.IsGamePaused && API.capi.IsSinglePlayer)
            {
                Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                clientMain.PauseGame(true);
                //API.api.Logger.Debug("should pause");
            }
        }
    }

    [HarmonyPatch(typeof(GuiDialogHandbook), "OnGuiClosed")]
    public class Patch_GuiDialogHandbook_OnGuiClosed
    {
        static void Postfix()
        {
            if (API.api != null && API.capi.IsGamePaused && API.capi.IsSinglePlayer)
            {
                Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                clientMain.PauseGame(false);
                //API.capi.Logger.Debug("should unpause");
            }
        }
    }

    /*[HarmonyPatch(typeof(GuiDialogBlockEntity), "OnGuiOpened")]
    public class Patch_GuiDialogBlockEntity_OnGuiOpened
    {
        static void Postfix()
        {
            if (API.api != null && API.api.Side.IsClient() && !API.capi.IsGamePaused && API.capi.IsSinglePlayer)
            {
                Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                clientMain.PauseGame(true);
                API.api.Logger.Debug("should pause");
            }
        }
    }

    [HarmonyPatch(typeof(GuiDialogBlockEntity), "OnGuiClosed")]
    public class Patch_GuiDialogBlockEntity_OnGuiClosed
    {
        static void Postfix()
        {
            if (API.api != null && API.api.Side.IsClient() && API.capi.IsGamePaused && API.capi.IsSinglePlayer)
            {
                Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                clientMain.PauseGame(false);
                API.capi.Logger.Debug("should unpause");
            }
        }
    }*/
}