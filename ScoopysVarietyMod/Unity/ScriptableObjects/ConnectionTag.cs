using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScoopysVarietyMod
{
    [CreateAssetMenu(fileName = "ConnectionTag", menuName = "ScriptableObjects/Scoopys/ConnectionTag", order = 1)]
    public class ConnectionTag : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}
