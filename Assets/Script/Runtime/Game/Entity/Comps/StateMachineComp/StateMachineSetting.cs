using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Runtime.Game.Entity
{
    public class StateMachineSetting : MonoBehaviour
    {
        [Title("动画组")]
        public List<AnimationClip> Clips = new();

        private Dictionary<string, AnimationClip> _clipDict = new();

        [Title("组件索引")]
        public SpriteRenderer Sprite;

        public Rigidbody2D Rg;

        [Title("跑步")]
        public float RunSpeed = 8f;
        public float RunAcceleration = 100f;
        public float RunDeceleration = 80f;

        public AnimationClip GetClip(string clipName)
        {
            if (_clipDict.TryGetValue(clipName, out AnimationClip clip))
            {
                return clip;
            }
            for (int i = 0; i < Clips.Count; i++)
            {
                if (Clips[i].name == clipName)
                {
                    _clipDict.Add(clipName, Clips[i]);
                    return Clips[i];
                }
            }
            return null;
        }
    }
}