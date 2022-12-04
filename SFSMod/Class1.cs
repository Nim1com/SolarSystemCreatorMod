using HarmonyLib;
using System.Collections.Generic;
using SFS.Input;
using SFS.IO;
using SFS.Platform;
using SFS.Translations;
using SFS.UI;
using SFS.UI.ModGUI;
using UnityEngine;
using System.Reflection;
using UITools;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using SFS.Parts;
using System;
using SFS.Variables;
using Type = SFS.UI.ModGUI.Type;

namespace SSCMod
{
    [HarmonyPatch(typeof(CreateWorldMenu), nameof(CreateWorldMenu.OpenSelectSolarSystemMenu))]
    class AddCustomButton
    {
        public static GameObject holder;
        public static bool snapping;
        public static readonly int MainWindowID = Builder.GetRandomID();
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
                // Custom solar system creator button code is called here. Just using Debug.Log as an example.
                // Tip: You can pass __instance to your custom solar system creator method if you want to set the new 
                // world's solar system to the custom one using:
                holder = Builder.CreateHolder(Builder.SceneToAttach.CurrentScene, "SCC GUI Holder");
                Window window = Builder.CreateWindow(holder.transform, MainWindowID, 1600, 1500, 0, 750, true, true, 0.95f, "Solar System Creator");
                window.CreateLayoutGroup(Type.Vertical);

                Container inputContainer = Builder.CreateContainer(window);

                InputWithLabel percentInput = Builder.CreateInputWithLabel(window, 380, 50, labelText: "something", inputText: "1");
                inputContainer.CreateLayoutGroup(Type.Horizontal, spacing: 10f);;
                Builder.CreateToggleWithLabel(window, 380, 50, () => !snapping, () => snapping ^= true, 0, 0,"Snap to Parts");




                //SFS.UI.ModGUI.Window();
                // Traverse traverse = Traverse.Create(__instance);
                // traverse.Method("SetSolarSystem", solarSystemName);

                Debug.Log("Time to open the solar system creator!");
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