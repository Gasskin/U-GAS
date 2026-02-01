using Animancer;

namespace Script.Runtime.Game.Entity
{
    public partial class StateMachineComp
    {
        public const string StateName_Idle = "idle";
        public const string StateName_Fall = "fall";
        public const string StateName_Jump = "jump";
        public const string StateName_MultiJump = "multi_jump";
        public const string StateName_Run = "run";
        public const string StateName_Dash = "dash";


        // public Animator Animator { get; private set; }
        public AnimancerComponent Animancer { get; private set; }

        public void PlayAnima(string clipName)
        {
            var clip = Settings.GetClip(clipName);
            if (clip != null)
            {
                Animancer.Stop();
                Animancer.Play(clip);
            }
        }
    }
}