using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Runtime.Game.System
{
    public class HurtBodyMono : MonoBehaviour
    {
        public List<Collider2D> HurtBodies = new();

        private void Awake()
        {
            foreach (var b in HurtBodies)
            {
                b.gameObject.layer = LayerMask.NameToLayer(ColliderSystem.LayerHurtBody);
            }
        }
    }
}