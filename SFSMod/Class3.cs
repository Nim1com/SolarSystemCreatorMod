using SFS.Builds;
using SFS;
using ModLoader;
using ModLoader.Helpers;
using UnityEngine;
using static SFS.Input.KeybindingsPC;
using UITools;

namespace SSCMain
{
    public class My_Keybindings : ModKeybindings
    {
        #region Keys
        public Key Close_Window = KeyCode.Escape;
        #endregion

        static SCC_Keybindings Main;

        public static void LoadKeybindings()
        {
            main = SetupKeybindings<My_Keybindings>(Main.main);

            AddStaticKeybindings();
        }

        static void AddStaticKeybindings()
        {
            AddOnKeyDown(main.Close_Window, windowHolder.SetActive(false));
        }

        public override void CreateUI()
        {
        }
    }
}