using DunGen;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScoopysVarietyMod
{
    public class TileOpeningConnector : MonoBehaviour
    {
        [field: Header("Connection Values")]
        [field: SerializeField] public TileOpeningPreset Preset { get; private set; }
        [field: SerializeField] public bool IsActive { get; private set; } = true;
        [SerializeField] private Vector3 Bounds;
        [SerializeField] private Vector3 Offset;
        [SerializeField] private bool DrawGizmos;

        [field: Header("Debug Values (Runtime Only)"), Space(15)]
        [field: SerializeField] public TileOpeningConnector ConnectedOpening { get; private set; }
        [SerializeField] private List<Collider> Obstructions = new List<Collider>();
        [SerializeField] private Collider[] results;

        private Tile tile;
        internal Tile Tile
        {
            get
            {
                if (tile == null)
                    tile = GetComponentInParent<Tile>();
                return (tile);
            }
        }

        internal string TagName => Preset.Tag.Name;

        private bool IsObstructed => Obstructions.Count > 0;
        public bool IsValidConnector => (IsObstructed == false && IsActive == true);
        public bool IsValidPairing => IsObstructed == false && ConnectedOpening != null && ConnectedOpening.IsObstructed == false;
        private bool hasTried;

        private void Awake()
        {
            results = [];
        }

        internal void TryFindMatch()
        {
            if (IsValidConnector == false) return;
            if (ConnectedOpening != null && ConnectedOpening.IsValidConnector == false)
                ConnectedOpening = null;
            if (ConnectedOpening != null && ConnectedOpening.ConnectedOpening == this) return;
            if (ConnectedOpening == null)
            {
                Obstructions.Clear();
                results = Physics.OverlapBox(transform.position + Offset, Bounds * 0.5f, transform.rotation, Preset.OverlapCastMask);
                foreach (Collider collider in results)
                    if (collider.gameObject != gameObject)
                        if (Vector3.Distance(transform.position, collider.transform.position) < Bounds.z * 2)
                        {
                            if (collider.TryGetComponent(out TileOpeningConnector connector) && connector.IsValidConnector)
                            {
                                if (connector.Preset.Tag == Preset.Tag)
                                    ConnectedOpening = connector;
                            }
                            else
                                Obstructions.Add(collider);
                        }
                hasTried = true;
            }
            else
            {
                if (ConnectedOpening.ConnectedOpening == null && ConnectedOpening.hasTried == true)
                    ConnectedOpening.ConnectedOpening = this;
            }
        }


        private void OnDrawGizmos()
        {
            if (DrawGizmos == false) return;
            Gizmos.matrix = transform.localToWorldMatrix;

            if (ConnectedOpening == null)
            {
                if (hasTried) return;
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(Offset, Bounds);

            }
            else
            {
                if (ConnectedOpening.ConnectedOpening == this)
                    Gizmos.color = Preset.DebugColor;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawCube(Offset, Bounds);
            }
        }
    }
}
