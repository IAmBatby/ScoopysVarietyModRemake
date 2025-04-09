using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScoopysVarietyMod
{
    [CreateAssetMenu(fileName = "TileOpeningPreset", menuName = "ScriptableObjects/Scoopys/TileOpeningPreset", order = 1)]
    public class TileOpeningPreset : ScriptableObject
    {
        [field: SerializeField] public ConnectionTag Tag { get; private set; }
        [field: SerializeField] public LayerMask OverlapCastMask { get; private set; }
        [field: SerializeField] public int MaxConnectionsPerTile { get; private set; }  
        [field: SerializeField, Range(0f,1f)] public float SpawnChance { get; private set; }
        [field: SerializeField] public List<GameObject> RandomReplacements { get; private set; }
        [field: SerializeField] public Color DebugColor { get; private set; }
    }
}
