using UnityEditor;
using UnityEngine;

namespace U_GAS
{
    public class TestMagnitudeUAssetEditorTool
    {
        private const string _GEN_PATH = "GameEffect/Editor/Asset/Magnitude";

        private static string GenPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/TestMagnitudeUAsset.asset";
        
        [MenuItem("U-GAS/Game Effect/Magnitude/New TestMagnitudeUAsset")]
        public static void Create()
        {
            var so = ScriptableObject.CreateInstance<TestMagnitudeUAsset>();
            AssetDatabase.CreateAsset(so, GenPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = so;
        }
    }
    
    public class TestMagnitudeUAsset : ModifierMagnitudeCalculationUAsset, IUAsset
    {
        public float k;
        
        public IUData GetUData()
        {
            var data = new TestMagnitudeUData();
            data.k = k;
            return data;
        }
    }
}