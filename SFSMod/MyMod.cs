
using HarmonyLib;
using ModLoader;
using ModLoader.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UITools;
using SFS.IO;
using SFS.Variables;
using System;
using SFS.UI;

namespace SSCMod
{
    /**
     * You only need to implement the Mod class once in your mod. The Mod class is how 
     * you tell the mod loader what it needs to load and execute.
     */

    /**
    [Serializable]
    public class Data
    {
        // Bool_Local and Float_Local are classes in SFS.Variables that let you detect onChange events.
        public Bool_Local myBool = new Bool_Local();
        public Float_Local myFloat = new Float_Local();
    }
    */



    public class MyMod : Mod, IUpdatable
    {
        public static MyMod Main;

        // this ModNameID can be whatever you want
        public override string ModNameID => "SCC";

        public override string DisplayName => "SolarSystem Creator";

        public override string Author => "0xNim";

        public override string MinimumGameVersionNecessary => "1.5.7";

        // I recommend use MAJOR.MINOR.PATCH Semantic versioning. 
        // Reference link: https://semver.org/ 
        public override string ModVersion => "v0.0.1";

        public override string Description => "A mod to create your own Solar System!";

        /// <summary>Icon</summary>
        //public override string IconLink => "https://i.imgur.com/r7rCmJT.jpg";

        // With this variable you can define if your mods needs the others mods to work
        public override Dictionary<string, string> Dependencies
        {
            get
            {
                new Dictionary<string, string> { { "UITools", "1.1.1" } };
                return this._dependencies;
            }
        }

        // Here you can specify which mods and version you need
        private Dictionary<string, string> _dependencies = new Dictionary<string, string>() { };

        public static FolderPath modFolder;

        /// <summary>Default constructor</summary>


        public override void Early_Load()
        {
            // This is for a singleton pattern. you can see more about singleton here https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/
            Main = this;

            /*
            ModFolder is an existing variable in the base class. It cannot be accessed by other classes by
            default, so we copy it to the var.
            */
            modFolder = new FolderPath(ModFolder);

            // Make use to use Debug.log from UnityEngine
            Debug.Log("Running Early load code");

            // Use early load to use Harmony and patch function
            Harmony harmony = new Harmony(ModNameID);
            // I use ModNameID in Harmony, because you need to pass string ID to create an instance of Harmony.

            // This function finds all the patches you have created and runs them
            harmony.PatchAll();

            // you can subscribe to scene changes
            ConfigurationMenu.Initialize();
        }

        public override async void Load()
        {
            Debug.Log("Running Load code");

            await ModsUpdater.UpdateAll();

            Keybind.LoadKeybindings();


        }

        Harmony patcher;
        void PatchAll() => (patcher ??= new Harmony("SSC")).PatchAll();

        // When the world scene is loaded

        public Dictionary<string, FilePath> UpdatableFiles => new() { { "https://github.com/Nim1com/SolarSystemCreatorMod/releases/latest/download/SolarSystemCreator.dll", new FolderPath(ModFolder).ExtendToFile("UITools.dll") } };


        // When the Build scene is loaded



    }
}
