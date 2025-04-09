using DunGen;
using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace ScoopysVarietyMod
{
    internal class InteriorController : NetworkBehaviour
    {
        [field:SerializeField] internal ExtendedDungeonFlow ExtendedDungeonFlow { get; private set; }

        private RuntimeDungeon dungeon;
        protected RuntimeDungeon DungeonGenerator
        {
            get
            {
                if (dungeon == null)
                {
                    if (RoundManager.Instance != null)
                        dungeon = RoundManager.Instance.dungeonGenerator;
                    else
                        dungeon = UnityEngine.Object.FindFirstObjectByType<RuntimeDungeon>();
                }
                return (dungeon);
            }
        }

        internal void SetExtendedDungeonFlow(ExtendedDungeonFlow extendedDungeonFlow)
        {
            ExtendedDungeonFlow = extendedDungeonFlow;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            //OnLoaded();
        }

        internal void Start()
        {
            if (ExtendedDungeonFlow == null)
            {
                Debug.LogError(gameObject.name + " Has Null ExtendedDungeonFlow!");
                return;
            }
            ExtendedDungeonFlow.DungeonEvents.onBeforeDungeonGenerate.AddListener(OnBeforeDungeonGenerate);
            OnInitialized();
        }

        protected virtual void OnInitialized() { }
        internal virtual void OnBeforeDungeonGenerate(RoundManager _) { }


    }
}
