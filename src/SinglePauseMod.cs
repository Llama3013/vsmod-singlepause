using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using System.Reflection;
using Vintagestory.API.Server;
using Vintagestory.API.Config;

[assembly: ModInfo("SinglePause",
    Description = "A quick mod made to pause the game when you open handbook on single player.",
    Website = "https://github.com/Llama3013/vsmod-singlepause",
    Authors = new[] { "Llama3013" },
    RequiredOnClient = true,
    IconPath = "modicon.png"
    )]

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

        ICoreServerAPI api;
        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
            this.api = api;

            api.RegisterCommand("pause", "Pauses the game", "/pause", PauseCommand);
            //api.RegisterCommand("resume", "Resumes the game", "/resume", ResumeCommand);
        }

        public void PauseCommand(IServerPlayer player, int groupId, CmdArgs args)
        {
            player.SendMessage(GlobalConstants.GeneralChatGroup, "Your game is paused, open & close the escape menu to resume game", EnumChatType.Notification);
            if (API.api != null && !API.capi.IsGamePaused && API.capi.IsSinglePlayer)
            {
                Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                clientMain.PauseGame(true);
                //API.api.Logger.Debug("should pause");
            }
        }

        //commands don't work when the game is paused
        /*public void ResumeCommand(IServerPlayer player, int groupId, CmdArgs args)
        {
            player.SendMessage(GlobalConstants.GeneralChatGroup, "Your game is resumed", EnumChatType.Notification);
            if (API.api != null && API.capi.IsGamePaused && API.capi.IsSinglePlayer)
            {
                Vintagestory.Client.NoObf.ClientMain clientMain = (Vintagestory.Client.NoObf.ClientMain)API.api.World;
                clientMain.PauseGame(false);
                //API.capi.Logger.Debug("should unpause");
            }
        }*/
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

    //This causes the game to crash on startup
    /*[HarmonyPatch(typeof(GuiDialogCreateCharacter), "OnGuiOpened")]
    public class Patch_GuiDialogCreateCharacter_OnGuiOpened
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

    [HarmonyPatch(typeof(GuiDialogCreateCharacter), "OnGuiClosed")]
    public class Patch_GuiDialogCreateCharacter_OnGuiClosed
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
    }*/

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