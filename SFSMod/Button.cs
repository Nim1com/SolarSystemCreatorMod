using HarmonyLib;
using System.Collections.Generic;
using SFS.Input;
using SFS.IO;
using System.Linq;
using SFS.Platform;
using SFS.Translations;
using SFS.UI;
using SFS.UI.ModGUI;
using UnityEngine;
using System.Reflection;
//using UITools;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using SFS.Parts;
using System;
using SFS.Variables;
using Type = SFS.UI.ModGUI.Type;
using Button = SFS.UI.ModGUI.Button;
using Object = UnityEngine.Object;
using static SFS.Input.KeybindingsPC;
using UnityEngine.Windows;
using SFS.Input;
using SFS.UI;
using SFS.UI.ModGUI;
using UnityEngine;
using UnityEngine.UI;


namespace SSCMod
{
    [HarmonyPatch(typeof(CreateWorldMenu), nameof(CreateWorldMenu.OpenSelectSolarSystemMenu))]
    class AddCustomButton
    {

        public KeybindingsPC.Key Cancel = KeyCode.Escape;

        
       
        static bool Prefix(CreateWorldMenu __instance)
        {
            Traverse traverse = Traverse.Create(__instance);

            SizeSyncerBuilder.Carrier horizontal;
            List<MenuElement> buttons = new List<MenuElement>
            {
                TextBuilder.CreateText().Text(() => Loc.main.Select_Solar_System),
                ElementGenerator.VerticalSpace(30),
                new SizeSyncerBuilder(out horizontal)
            };
            AddButton(Loc.main.Default_Solar_System, "");

            if (DevSettings.FullVersion && FileLocations.SolarSystemsFolder.FolderExists())
            {
                foreach (FolderPath item in FileLocations.SolarSystemsFolder.GetFoldersInFolder(recursively: false))
                {
                    if (item.FolderName != "Example")
                    {
                        AddButton(Loc.main.Custom_Solar_System.Inject(item.FolderName, "name"), item.FolderName);
                    }
                }
            }
           

            buttons.Add(ElementGenerator.VerticalSpace(60)); // Seperates the button from the rest of the solar systems.
            buttons.Add(ButtonBuilder.CreateButton(horizontal, () => "Create Solar System", delegate
            {
                ModUI.CreateUIWindow();
            }, CloseMode.Current));
            

            MenuGenerator.OpenMenu(CancelButton.Cancel, CloseMode.Current, buttons.ToArray());

            void AddButton(string text, string solarSystemName)
            {
                buttons.Add(ButtonBuilder.CreateButton(horizontal, () => text, delegate
                {
                    traverse.Method("SetSolarSystem", solarSystemName);
                }, CloseMode.Current));
            }


            string name = "Creator";
            Scene scene = SceneManager.CreateScene(name);
            SceneManager.LoadScene(name);

            // Skip orginal code, since it's already been run here.
            return false;
        }
    }
}