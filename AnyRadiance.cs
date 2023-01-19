using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Modding;
using UnityEngine;

namespace AnyRadiance {
    public class AnyRadiance : Mod, ITogglableMod, IMod, Modding.ILogger {
        public static readonly List<Sprite> Sprites = new List<Sprite>();
        public static readonly List<byte[]> SpriteBytes = new List<byte[]>();
        public static readonly Dictionary<string, GameObject> PreloadedGameObjects = new Dictionary<string, GameObject>();
        public static global::AnyRadiance.AnyRadiance Instance { get; private set; }

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>();
        }

        public override string GetVersion()
        {
            return "69.420";
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedGameObjects)
        {
            Instance = this;
            Log("Initalizing.");
            ModHooks.AfterSavegameLoadHook += AfterSaveGameLoad;
            ModHooks.NewGameHook += AddComponent;
            ModHooks.LanguageGetHook += LangGet;
            int num = 0;
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
            foreach (string text in manifestResourceNames)
            {
                if (!text.EndsWith(".png"))
                {
                    continue;
                }
                using Stream stream = executingAssembly.GetManifestResourceStream(text);
                if (stream != null)
                {
                    byte[] array = new byte[stream.Length];
                    stream.Read(array, 0, array.Length);
                    stream.Dispose();
                    Texture2D texture2D = new Texture2D(1, 1);
                    texture2D.LoadImage(array, markNonReadable: true);
                    SpriteBytes.Add(array);
                    Sprites.Add(Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f)));
                    Log("Created sprite from embedded image: " + text + " at index " + num);
                    num++;
                }
            }
        }

        private static string LangGet(string key, string sheettitle, string orig)
        {   
            if (key != null) {
                switch (key) {
                    case "ABSOLUTE_RADIANCE_SUPER": return "Any";
                    case "GG_S_RADIANCE": return "God of meme.";
                    case "GODSEEKER_RADIANCE_STATUE": return "Ok.";
                    default: return Language.Language.GetInternal(key, sheettitle);
                }
            }
            return orig;
        }

        private static void AfterSaveGameLoad(SaveGameData data)
        {
            AddComponent();
        }

        private static void AddComponent()
        {
            GameManager.instance.gameObject.AddComponent<RadianceFinder>();
        }

        public void Unload()
        {
            ModHooks.AfterSavegameLoadHook -= AfterSaveGameLoad;
            ModHooks.NewGameHook -= AddComponent;
            ModHooks.LanguageGetHook -= LangGet;
            RadianceFinder radianceFinder = GameManager.instance?.gameObject.GetComponent<RadianceFinder>();
            if (radianceFinder != null)
            {
                UnityEngine.Object.Destroy(radianceFinder);
            }
        }
    }
}