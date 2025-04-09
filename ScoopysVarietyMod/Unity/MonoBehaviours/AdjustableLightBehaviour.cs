using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace ScoopysVarietyMod
{
    public class AdjustableLightBehaviour : NetworkBehaviour
    {
        [field: SerializeField] public Light PrimaryLight { get; private set; }
        [field: SerializeField] public List<GameObject> AssociatedObjects { get; private set; } = new List<GameObject>();
    }
}
