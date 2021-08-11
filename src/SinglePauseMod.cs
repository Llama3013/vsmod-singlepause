using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using System.Reflection;
using Vintagestory.API.Client;

[assembly: ModInfo("SinglePause",
    Description = "A quick mod made to pause the game when you open WorldMap on single player.",
    Website = "https://github.com/Llama3013/vsmod-singlepause",
    Authors = new[] { "llama3013" })]

namespace SinglePause
{

    public class API
    {
        public static ICoreAPI api;
        public static ClientCoreAPI capi;
        public static EnumDialogType dialogType = EnumDialogType.HUD;
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

    [HarmonyPatch(typeof(WorldMapManager), "ToggleMap")]
    public class Patch_WorldMapManager_ToggleMap
    {
        static void Postfix(EnumDialogType asType)
        {
            if (API.api != null && API.capi.IsSinglePlayer)
            {
                WorldMapManager worldMapMan = API.capi.ModLoader.GetModSystem<WorldMapManager>();
                bool isDlgOpened = worldMapMan.worldMapDlg != null && worldMapMan.worldMapDlg.IsOpened();
                API.api.Logger.Debug("[pause] {0}", isDlgOpened);
                if (isDlgOpened)
                {
                    if (asType == EnumDialogType.Dialog)
                    {
                        Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                        clientMain.PauseGame(true);
                        API.api.Logger.Debug("should pause 1");
                    }
                    else
                    {
                        Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                        clientMain.PauseGame(false);
                        API.capi.Logger.Debug("should unpause 1");
                    }
                } else {
                    if (asType == EnumDialogType.Dialog)
                    {
                        Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                        clientMain.PauseGame(false);
                        API.api.Logger.Debug("should unpause 2");
                    }
                    else
                    {
                        Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                        clientMain.PauseGame(true);
                        API.capi.Logger.Debug("should pause 2");
                    }
                }
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