using HarmonyLib;
using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.TerrainUtils;
using static Unity.Audio.Handle;

namespace ScoopysVarietyMod
{
    internal static class Patches
    {
        [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Awake)), HarmonyPrefix]
        internal static void GameNetworkManagerAwake_Prefix()
        {
            InteriorManager.Setup();
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake)), HarmonyPrefix]
        internal static void StartOfRoundAwake_Prefix()
        {
            InteriorManager.Initialize();
        }
    }
}
