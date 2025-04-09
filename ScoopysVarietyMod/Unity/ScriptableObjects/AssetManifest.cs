using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScoopysVarietyMod
{
    [CreateAssetMenu(fileName = "AssetManifest", menuName = "ScriptableObjects/Scoopys/AssetManifest", order = 1)]
    public class AssetManifest : ScriptableObject
    {
        [field: SerializeField] internal ExtendedDungeonFlow ExtendedCastleFlow { get; private set; }
        [field: SerializeField] internal ExtendedDungeonFlow ExtendedSewerFlow { get; private set; }

        [field: SerializeField] internal AdjustableLightBehaviour LightBehaviourPrefab { get; private set; }

        [field: SerializeField] internal List<Material> CastleBrickMaterials { get; private set; } = new List<Material>();
        [field: SerializeField] internal List<Material> CastleWoodMaterials { get; private set; } = new List<Material>();
    }
}
