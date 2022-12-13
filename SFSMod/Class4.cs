using SFS.Builds;
using SFS;
using ModLoader;
using ModLoader.Helpers;
using UnityEngine;
using static SFS.Input.KeybindingsPC;
using UITools;

namespace SSCMod
{
    public class Keybind : ModKeybindings
    {
        #region Keys
        public Key Close_Window = KeyCode.Escape;
        #endregion

        static Keybind main;

        public static void LoadKeybindings()
        {
            main = SetupKeybindings<Keybind>(MyMod.Main);

            AddStaticKeybindings();
        }

        static void AddStaticKeybindings()
        {
            AddOnKeyDown(main.Close_Window, holder.SetActive(false));
        }

        public override void CreateUI()
        {
        }
    }
}