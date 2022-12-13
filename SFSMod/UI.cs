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

        
        public static GameObject holder;
        public static bool includeDefaultPlanets;
        public static bool includeDefaultHeightmaps;
        public static bool includeDefaultTextures;
        public static bool hideStarsInAtmosphere;
        public static string height;
        public static string horizontalPosition;
        public static string adress;
        public static string version;
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
                Window window = Builder.CreateWindow(holder.transform, MainWindowID, 1600, 1500, 0, 750, false, true, 0.95f, "Solar System Creator");
                window.CreateLayoutGroup(Type.Vertical);
                window.EnableScrolling(Type.Vertical);

                Container inputContainer = Builder.CreateContainer(window);

                inputContainer.CreateLayoutGroup(Type.Horizontal, spacing: 10f);;

                Builder.CreateToggleWithLabel(window, 500, 50, () => !includeDefaultPlanets, () => includeDefaultPlanets ^= true, 0, 0, "Include Default Planets");
                Builder.CreateToggleWithLabel(window, 500, 50, () => !includeDefaultHeightmaps, () => includeDefaultHeightmaps ^= true, 0, 0, "Include Default Heightmaps");
                Builder.CreateToggleWithLabel(window, 500, 50, () => !includeDefaultTextures, () => includeDefaultTextures ^= true, 0, 0, "Include Default Textures");
                Builder.CreateToggleWithLabel(window, 500, 50, () => !hideStarsInAtmosphere, () => hideStarsInAtmosphere ^= true, 0, 0, "Hide Stars In Atmosphere");
                InputWithLabel address = Builder.CreateInputWithLabel(window, 500, 50, labelText: "Main Planet", inputText: "Earth");
                InputWithLabel angle = Builder.CreateInputWithLabel(window, 750, 50, labelText: "angle | Experimental", inputText: "90");
                InputWithLabel horizontalPosition = Builder.CreateInputWithLabel(window, 1100, 50, labelText: "LaunchPad Horizontal | Experimental", inputText: "365.0");
                InputWithLabel height = Builder.CreateInputWithLabel(window, 1050, 50, labelText: "LaunchPad Height | Experimental", inputText: "26.2");
                version = "1.5.8.5";
                
                //category



                ButtonBuilder.CreateButton(horizontal, () => "Save Solar System", delegate
                {
                /*
                adress = ;
                horizontalPosition = horizontalPosition.input.text;
                height = height.input.text;
                if (adress == "")
                {
                    adress = "Earth";
                }
                if (horizontalPosition == "")
                {
                    horizontalPosition = "365.0";
                }
                if (height == "")
                {
                    height = "26.2";
                }
                if (angle.input.text == "")
                {
                    angle.input.text = "90";
                }
                if (adress == "Earth")
                {
                    includeDefaultPlanets = true;
                    includeDefaultHeightmaps = true;
                    includeDefaultTextures = true;
                }
                else
                {
                    includeDefaultPlanets = false;
                    includeDefaultHeightmaps = false;
                    includeDefaultTextures = false;
                }
                if (adress == "Earth" || adress == "Mars" || adress == "Moon" || adress == "Venus" || adress == "Mercury" || adress == "Jupiter" || adress == "Saturn" || adress == "Uranus" || adress == "Neptune" || adress == "Pluto")
                {
                    includeDefaultPlanets = true;
                    includeDefaultHeightmaps = true;
                    includeDefaultTextures = true;
                }
                else
                {
                    includeDefaultPlanets = false;
                    includeDefaultHeightmaps = false;
                    includeDefaultTextures = false;
                }
                if (adress == "Earth" || adress == "Mars" || adress == "Moon" || adress == "Venus" || adress == "Mercury" || adress == "Jupiter" || adress == "Saturn" || adress == "Uranus" || adress == "Neptune" || adress == "Pluto")
                {
                    includeDefaultPlanets = true;
                    includeDefaultHeightmaps = true;
                    includeDefaultTextures = true;
                }
                else
                {
                    includeDefaultPlanets = false;
                    includeDefaultHeightmaps = false;
                    includeDefaultTextures = false;
                }
                if (adress == "Earth" || adress == "Mars" || adress == "Moon" || adress == "Venus" || adress == "Mercury" || adress == "Jupiter" || adress == "Saturn" || adress == "Uranus" || adress == "Neptune" || adress == "Pluto")
                {
                    includeDefaultPlanets = true;
                */
                }, CloseMode.Current);

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