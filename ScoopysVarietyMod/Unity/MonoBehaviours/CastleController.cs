using DunGen;
using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace ScoopysVarietyMod
{
    internal class CastleController : InteriorController
    {
        [SerializeField] private int MatchAttempts = 5;
        private List<TileOpeningConnector> windows = new List<TileOpeningConnector>();
        private Dictionary<Tile, List<TileOpeningConnector>> windowsDict = new Dictionary<Tile, List<TileOpeningConnector>>();

        private Dictionary<Tile, int> tilesSpawnedOpeningsCountDict = new Dictionary<Tile, int>();

        private Random _random;
        private Random Random
        {
            get
            {
                if (_random == null)
                {
                    if (StartOfRound.Instance != null)
                        _random = new Random(StartOfRound.Instance.randomMapSeed);
                    else
                        _random = new Random();
                }
                return (_random);
            }
        }

        protected override void OnInitialized()
        {
            InteriorManager.CastleController = this;
        }

        internal override void OnBeforeDungeonGenerate(RoundManager _)
        {
            base.OnBeforeDungeonGenerate(null);
            Debug.Log("CastleController OnLoaded, Registering PostProcessStep.");
            DungeonGenerator.Generator.RegisterPostProcessStep(InitializeTileOpenings, 50, PostProcessPhase.BeforeBuiltIn);
        }

        private void InitializeTileOpenings(DungeonGenerator _)
        {
            Debug.Log("Initializing Tile Openings.");
            Physics.SyncTransforms();
            windows = DungeonGenerator.Generator.Root.GetComponentsInChildren<TileOpeningConnector>().ToList();

            foreach (TileOpeningConnector window in windows)
            {
                for (int i = 0; i < MatchAttempts; i++)
                    window.TryFindMatch();

                if (windowsDict.TryGetValue(window.Tile, out List<TileOpeningConnector> list))
                    list.Add(window);
                else
                    windowsDict.Add(window.Tile, new List<TileOpeningConnector>(windows));
            }

            Debug.Log("Tried To Find Matches Between #" + windows.Count + " Windows.");

            CreateOpenings();
        }


        private void CreateOpenings()
        {
            List<TileOpeningConnector> spawningConnectors = new List<TileOpeningConnector>();
            List<TileOpeningConnector> discardedConnectors = new List<TileOpeningConnector>();

            foreach (TileOpeningConnector connector in windows)
            {
                if (connector.IsValidPairing == false) continue;
                if (spawningConnectors.Contains(connector) || spawningConnectors.Contains(connector.ConnectedOpening)) continue;
                if (discardedConnectors.Contains(connector) || discardedConnectors.Contains(connector.ConnectedOpening)) continue;
                spawningConnectors.Add(connector);
                discardedConnectors.Add(connector.ConnectedOpening);
            }

            int successfulCreations = 0;
            foreach (TileOpeningConnector connector in spawningConnectors)
            {
                if (connector.Preset.RandomReplacements.Count == 0) continue;
                if (tilesSpawnedOpeningsCountDict.TryGetValue(connector.Tile, out int connectorCount) && connectorCount == connector.Preset.MaxConnectionsPerTile) continue;
                if (tilesSpawnedOpeningsCountDict.TryGetValue(connector.ConnectedOpening.Tile, out int connectorPairCount) && connectorPairCount == connector.ConnectedOpening.Preset.MaxConnectionsPerTile) continue;
                float randomRoll = Random.NextFloat(0, 1);
                if (connector.Preset.SpawnChance > randomRoll) continue;


                Vector3 positionToSpawn = Vector3.Lerp(connector.transform.position, connector.ConnectedOpening.transform.position, 0.5f);
                Quaternion rotationToSpawn = connector.transform.rotation;
                GameObject prefab = connector.Preset.RandomReplacements.First();
                Tile tile = connector.Tile;
                Tile pairedTile = connector.ConnectedOpening.Tile;
                GameObject.Destroy(connector.ConnectedOpening.gameObject);
                GameObject.Destroy(connector.gameObject);

                GameObject newObject = GameObject.Instantiate(prefab, tile.transform.position, rotationToSpawn);
                newObject.transform.position = positionToSpawn;

                if (tilesSpawnedOpeningsCountDict.ContainsKey(tile))
                    tilesSpawnedOpeningsCountDict[tile]++;
                else
                    tilesSpawnedOpeningsCountDict.Add(tile, 1);

                if (tilesSpawnedOpeningsCountDict.ContainsKey(pairedTile))
                    tilesSpawnedOpeningsCountDict[pairedTile]++;
                else
                    tilesSpawnedOpeningsCountDict.Add(pairedTile, 1);

                successfulCreations++;
            }

            Debug.Log("Created #" + successfulCreations + " Tile Opening Connections.");
        }
    }
}
