using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace ScoopysVarietyMod
{
    internal static class InteriorManager
    {
        internal static ExtendedDungeonFlow CastleInterior => Assets.AssetManifest.ExtendedCastleFlow;
        internal static ExtendedDungeonFlow SewerInterior => Assets.AssetManifest.ExtendedSewerFlow;

        private static CastleController castleControllerPrefab;
        internal static CastleController CastleController { get; set; }

        internal static void Setup()
        {
            castleControllerPrefab = CreateInteriorController<CastleController>(CastleInterior);
        }

        internal static void Initialize()
        {
            SpawnInteriorController(castleControllerPrefab);
        }

        private static T CreateInteriorController<T>(ExtendedDungeonFlow dungeonFlow) where T : InteriorController
        {
            GameObject controllerPrefab;
            T interiorController;
            NetworkManager networkManager = UnityEngine.Object.FindObjectOfType<NetworkManager>();
            if (networkManager != null)
            {
                Debug.Log("Creating Network InteriorController!");
                controllerPrefab = LethalLevelLoader.PrefabHelper.CreateNetworkPrefab(typeof(T).Name);
                interiorController = controllerPrefab.AddComponent<T>();
                interiorController.SetExtendedDungeonFlow(dungeonFlow);
                NetworkObject networkObject = controllerPrefab.GetComponent<NetworkObject>();
                networkObject.DontDestroyWithOwner = true;
                networkObject.SceneMigrationSynchronization = true;
                networkObject.DestroyWithScene = false;
                LethalLevelLoader.LethalLevelLoaderNetworkManager.RegisterNetworkPrefab(controllerPrefab);
            }
            else
            {
                Debug.Log("Creating Local InteriorController!");
                controllerPrefab = new GameObject(nameof(T));
                controllerPrefab.SetActive(false);
                GameObject.DontDestroyOnLoad(controllerPrefab);
                interiorController = controllerPrefab.AddComponent<T>();
                interiorController.SetExtendedDungeonFlow(dungeonFlow);
                controllerPrefab.SetActive(true);
                interiorController.Start();
            }

            return (interiorController);
        }

        private static void SpawnInteriorController(InteriorController prefab)
        {
            Debug.Log("Spawning InteriorController!");
            if (NetworkManager.Singleton != null)
            {
                if (NetworkManager.Singleton.IsServer == false) return;
                GameObject instancedController = GameObject.Instantiate(prefab.gameObject);
                if (instancedController.TryGetComponent(out NetworkObject networkObject))
                    networkObject.Spawn(destroyWithScene: false);
            }
            else
            {
                //GameObject instancedController = GameObject.Instantiate(prefab.gameObject);
            }
        }

    }
}
