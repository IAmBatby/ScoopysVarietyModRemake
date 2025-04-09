using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ScoopysVarietyMod
{
    internal static class Assets
    {
        internal static AssetBundle LethalBundle {  get; private set; }
        internal static ExtendedMod ExtendedMod { get; private set; }
        internal static AssetManifest AssetManifest { get; set; }

        internal static void OnExtendedModLoaded(ExtendedMod extendedMod)
        {
            ExtendedMod = extendedMod;
            InitializeMod();
        }

        internal static void OnLethalBundleLoaded(AssetBundle assetBundle)
        {
            LethalBundle = assetBundle;
            AssetManifest[] manifests = assetBundle.LoadAllAssets<AssetManifest>();
            if (manifests.Length != 0)
                AssetManifest = manifests[0];
            else
                Plugin.LogError("Failed To Find Asset Manifest!");

            InitializeMod();
        }

        private static void InitializeMod()
        {
            if (LethalBundle == null || ExtendedMod == null || AssetManifest == null) return;

            //InteriorManager.Initialize();

            Plugin.Log("Initialized Mod!");
        }
    }
}
