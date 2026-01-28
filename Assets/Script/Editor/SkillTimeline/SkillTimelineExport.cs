using System.Collections.Generic;
using System.IO;
using cfg.Gas;
using MemoryPack;
using Script.Runtime.Framework.System;
using Script.Runtime.Game;
using Script.Runtime.Game.Entity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Script.Editor
{
    public class SkillTimelineExport
    {
        [MenuItem("Assets/Battle/Export Skill Timeline")]
        public static void Export()
        {
            if (Selection.activeObject is null)
            {
                return;
            }
            var folder = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!AssetDatabase.IsValidFolder(folder))
            {
                return;
            }
            var tbSkillTimeline = new List<SkillTimeline>();
            var guids = AssetDatabase.FindAssets("t:TimelineAsset", new string[] { folder });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<TimelineAsset>(path);
                if (asset == null)
                {
                    continue;
                }
                if (!int.TryParse(asset.name, out int id))
                {
                    Debug.LogError($"{asset.name} 不是合法命名", asset);
                    continue;
                }
                tbSkillTimeline.Add(ToSkillTimeline(id, asset));
            }
            var bin = MemoryPackSerializer.Serialize(tbSkillTimeline);
            var export = "Assets/Config/CustomData/gas_tbskilltimeline.bytes";
            if (File.Exists(export))
            {
                File.Delete(export);
            }
            File.WriteAllBytes(export, bin);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (Application.isPlaying)
            {
                SystemDriver.ConfigSystem.HotReload();
            }
        }

        private static SkillTimeline ToSkillTimeline(int id, TimelineAsset asset)
        {
            var result = new SkillTimeline();
            result.Id = id;
            var tracks = asset.GetOutputTracks();
            foreach (var track in tracks)
            {
                var timelineClips = track.GetClips();
                foreach (var timelineClip in timelineClips)
                {
                    if (timelineClip.asset is SkillTimelineClipAsset skillClipAsset)
                    {
                        if (skillClipAsset.SkillClip == null)
                        {
                            Debug.LogError($"timeline: {id}, has null skill timeline clip", asset);
                            continue;
                        }
                        timelineClip.displayName = skillClipAsset.SkillClip.GetType().Name.Replace("Clip","");
                        skillClipAsset.SkillClip.StartFrame =
                            Mathf.RoundToInt((float)(timelineClip.start * SkillTimelineDriver.TimelineFrame));
                        skillClipAsset.SkillClip.EndFrame =
                            Mathf.RoundToInt((float)(timelineClip.end * SkillTimelineDriver.TimelineFrame));
                        result.AddClip(skillClipAsset.SkillClip);
                    }
                    else if (timelineClip.asset is AnimationPlayableAsset animaClipAsset)
                    {
                        if (animaClipAsset.clip == null)
                        {
                            Debug.LogError($"timeline: {id}, has null animation clip", asset);
                            continue;
                        }
                        var anima = new SkillTimelineAnimaClip()
                        {
                            AnimationName = animaClipAsset.clip.name
                        };
                        anima.StartFrame =
                            Mathf.RoundToInt((float)(timelineClip.start * SkillTimelineDriver.TimelineFrame));
                        anima.EndFrame =
                            Mathf.RoundToInt((float)(timelineClip.end * SkillTimelineDriver.TimelineFrame));
                        result.AddClip(anima);
                    }
                }
            }

            return result;
        }
    }
}