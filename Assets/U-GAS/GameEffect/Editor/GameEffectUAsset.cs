using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace U_GAS.Editor
{
    public class GameEffectUAssetEditorTool
    {
        private const string _GEN_PATH = "GameEffect/Editor/Asset/GameEffect";

        private static string GenPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/NewGameEffect.asset";
        
        [MenuItem("U-GAS/Game Effect/Create New")]
        public static void Create()
        {
            var so = ScriptableObject.CreateInstance<GameEffectUAsset>();
            AssetDatabase.CreateAsset(so, GenPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = so;
        }

        [MenuItem("U-GAS/Game Effect/Gen All")]
        public static void GenAll()
        {
            
        }
    }
    
    public class GameEffectUAsset: ScriptableObject, IUAsset
    {
        [Title("备注")]
        [HideLabel]
        [TextArea]
        public string backUp;

        [Title("类型")]
        [InfoBox(@"1.瞬时：释放GE时立刻生效1次，随后立刻删除，此时持续时间与生效周期没有意义
2.永久：释放GE时立刻生效1次，随后每个生效周期生效1次，生效周期为0时仅生效1次
3.限时：释放GE时立刻生效1次，随后每个生效周期生效1次，到达持续时间后删除")]
        [LabelText("持续类型")]
        [LabelWidth(80)]
        public EDurationPolicy durationPolicy;


        [LabelText("持续时间")]
        [LabelWidth(80)]
        public float duration;


        [LabelText("生效周期")]
        [LabelWidth(80)]
        public float period;


        [Title("标签")]
        [ValueDropdown("@GameTagEditor.GameTagValueDropdown()", IsUniqueList = true)]
        [LabelText("该GE的标签 - 判断移除GE时，会采用这个标签作为依据")]
        [LabelWidth(80)]
        public List<string> assetTags;

        [ValueDropdown("@GameTagEditor.GameTagValueDropdown()", IsUniqueList = true)]
        [LabelText("该GE会附加的标签 - 生效时添加，失效时移除")]
        [LabelWidth(80)]
        public List<string> grantedTags;

        [ValueDropdown("@GameTagEditor.GameTagValueDropdown()", IsUniqueList = true)]
        [LabelText("应用该GE所必须的标签 - 必须拥有全部标签才可以应用该GE")]
        [LabelWidth(80)]
        public List<string> requiredTags;

        [ValueDropdown("@GameTagEditor.GameTagValueDropdown()", IsUniqueList = true)]
        [LabelText("应用该GE所冲突的标签 - 只要存在任一标签则不可应用该GE")]
        [LabelWidth(80)]
        public List<string> conflictTags;

        [ValueDropdown("@GameTagEditor.GameTagValueDropdown()", IsUniqueList = true)]
        [LabelText("GE生效时移除含有以下任一标签的其他GE - 周期性GE每次生效都会尝试删除其他")]
        [LabelWidth(80)]
        public List<string> removeGameEffectsWithTags;
        
        [LabelText("计算器")]
        [LabelWidth(80)]
        public List<ModifierMagnitudeCalculationUAsset> magnitudes;

        [Button]
        public IUData GetUData()
        {
            var data = new GameEffectUData();
            data.durationPolicy = durationPolicy;
            data.duration = duration;
            data.period = period;
            data.assetTags = assetTags;
            data.grantedTags = grantedTags;
            data.requiredTags = requiredTags;
            data.conflictTags = conflictTags;
            data.removeGameEffectsWithTags = removeGameEffectsWithTags;
            data.magnitude = new List<ModifierMagnitudeCalculationUData>();
            foreach (var m in magnitudes)
            {
                if (m is IUAsset asset)
                {
                    data.magnitude.Add((ModifierMagnitudeCalculationUData)asset.GetUData());
                }
            }

            byte[] test = null;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, data);
                test = ms.ToArray();
            }
            
            using (var ms = new MemoryStream(test))
            {
                var de = Serializer.Deserialize<GameEffectUData>(ms);
            }
            
            return data;
        }
    }
}