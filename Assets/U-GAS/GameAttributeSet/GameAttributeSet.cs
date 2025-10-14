using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace U_GAS
{
    [Serializable]
    public class GameAttributeSet
    {
        protected GameAttributeSetUAsset uAsset;

        public Dictionary<EGameAttribute, GameAttribute> Attributes;

        protected void OnInit()
        {
            Attributes = new();
            foreach (var attr in uAsset.attributes)
            {
                Attributes.Add(attr, GameAttributeRegister.New(attr));
            }
        }
    }
}