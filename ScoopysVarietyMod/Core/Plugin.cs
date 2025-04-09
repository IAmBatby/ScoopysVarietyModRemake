using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalLevelLoader;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ScoopysVarietyMod
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency(LethalLevelLoader.Plugin.ModGUID)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "Scoopy-ScoopysVarietyMod";
        public const string ModName = "Scoopy's Variety Mod";
        public const string ModVersion = "1.3.0";

        internal const string AssetBundleName = "scoopysvarietymod.lethalbundle";

        internal static Plugin Instance;

        internal static readonly Harmony Harmony = new Harmony(ModGUID);

        internal static ManualLogSource logger;



        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            logger = Logger;

            //AssetBundleLoader.AddOnExtendedModLoadedListener(Assets.OnExtendedModLoaded, "Scoopy");
            //AssetBundleLoader.AddOnLethalBundleLoadedListener(Assets.OnLethalBundleLoaded, AssetBundleName);


            Harmony.PatchAll(typeof(Patches));

            Log("Plugin Awake Occured");
        }

        public static void Log(string log)
        {
            logger.LogInfo(log);
        }

        public static void LogError(string log)
        {
            logger.LogError(log);
        }
    }

    public class AssetBundleLoader : MonoBehaviour
    {
        public static AssetBundle AssetBundle { get; private set; }
        public static AssetBundleLoader Instance { get; private set; }

        public static void Create(string assetBundlePath)
        {
            GameObject assetBundleObject = new GameObject("AssetBundleLoader");
            Instance = assetBundleObject.AddComponent<AssetBundleLoader>();
            Instance.StartCoroutine(Instance.LoadAssetBundle(assetBundlePath));
        }

        private IEnumerator LoadAssetBundle(string assetBundlePath)
        {
            string combinedPath = Path.Combine(Application.streamingAssetsPath, assetBundlePath);
            AssetBundleCreateRequest activeLoadRequest = AssetBundle.LoadFromFileAsync(combinedPath);
            yield return activeLoadRequest;
            if (activeLoadRequest.isDone && activeLoadRequest.assetBundle != null)
            {
                AssetBundle = activeLoadRequest.assetBundle;
                OnAssetBundleLoaded();
            }
        }

        private void OnAssetBundleLoaded()
        {

        }
    }
}