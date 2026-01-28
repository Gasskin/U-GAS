using Script.Runtime.Game.Entity.BattleInputComp;
using UnityEngine;

namespace Script.Runtime.Game.Entity.StateMachineComp
{
    public class StateMachineContext
    {
        // public Vector2Int MoveDir { get; set; }
        public Vector2Int MoveDir { get; set; }

        public ButtonState Jump { get; set; } = new();
        public ButtonState Dash { get; set; } = new();
    
        // public ButtonState Attack { get; set; } = new();

        public void LateTick(float dt)
        {
            Jump.ResetFrameState();
            Dash.ResetFrameState();
        }
    }

    public partial class StateMachineComp
    {
        public StateMachineContext Context = new();
    }
}