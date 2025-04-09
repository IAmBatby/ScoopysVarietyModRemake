using DunGen;
using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace ScoopysVarietyMod
{
    [DefaultExecutionOrder(-50)]
    public class SimulatedLethalCompanyController : MonoBehaviour
    {
        [SerializeField] private AssetManifest assetManifest;
        [SerializeField] private ExtendedDungeonFlow CurrentExtendedDungeonFlow;
        [field: SerializeField] internal RuntimeDungeon DungeonGenerator { get; private set; }

        private void Awake()
        {
            Debug.Log("Simulating Lethal Company!");

            SimulatedPlugin();
            Debug.Break();
            SimulatedMainMenu();
            Debug.Break();
            SimulatedLobby();
            Debug.Break();
            SimulatedLevelLoad();
        }

        private void SimulatedPlugin()
        {
            Debug.Log("Simulated Plugin Awake.");
            Assets.AssetManifest = assetManifest;
        }

        private void SimulatedMainMenu()
        {
            Debug.Log("Simulated Main Menu Load.");
            InteriorManager.Setup();
        }

        private void SimulatedLobby()
        {
            Debug.Log("Simulated Ship Lobby Load.");
            InteriorManager.Initialize();
        }

        private void SimulatedLevelLoad()
        {
            Debug.Log("Simulated Moon Load.");
            DungeonGenerator.Generator.DungeonFlow = CurrentExtendedDungeonFlow.DungeonFlow;
            //CurrentExtendedDungeonFlow.DungeonEvents.onBeforeDungeonGenerate.Invoke(null);
            InteriorManager.CastleController.OnBeforeDungeonGenerate(null);
            DungeonGenerator.Generator.Generate();
        }
    }
}
