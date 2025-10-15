using System;
using U_GAS.Editor;
using UnityEditor;
using UnityEngine;

namespace U_GAS
{
    public class LinearMagnitudeEditorTool
    {
        private const string _GEN_PATH = "GameEffect/Editor/Asset/Magnitude";

        private static string GenPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/NewLinearMagnitude.asset";
        
        [MenuItem("U-GAS/Game Effect/Magnitude/New LinearMagnitude")]
        public static void Create()
        {
            var so = ScriptableObject.CreateInstance<LinearMagnitudeUAssetProvider>();
            AssetDatabase.CreateAsset(so, GenPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = so;
        }
    }
    
    public class LinearMagnitudeUAssetProvider : ModifierMagnitudeCalculationUAssetProvider, IUAssetProvider
    {
        public float k;
        public float b;

        public IUAsset GetUAsset()
        {
            var data = new LinearMagnitudeCalculation();
            data.k = k;
            data.b = b;
            return data;
        }
    }
}