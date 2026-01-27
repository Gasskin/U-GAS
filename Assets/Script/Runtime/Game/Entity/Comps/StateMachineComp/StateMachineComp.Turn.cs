using UnityEngine;

namespace Script.Runtime.Game
{
    public partial class StateMachineComp
    {
        public class StateMachineTurn
        {
            public bool IsFacingRight { get; private set; } = true;
    
            private StateMachineComp _stateMachine;
    
            public StateMachineTurn(StateMachineComp machine)
            {
                _stateMachine = machine;
            }
    
            public void CheckAndTurn()
            {
                var moveDirection = _stateMachine.Context.MoveDir;
                if ((moveDirection.x < 0 && IsFacingRight) || (moveDirection.x > 0 && !IsFacingRight))
                {
                    Turn();
                }   
            }
    
            private void Turn()
            {
                IsFacingRight = !IsFacingRight;
		
                Vector3 scale = _stateMachine.Settings.Sprite.transform.localScale;
                scale.x = IsFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
		
                _stateMachine.Settings.Sprite.transform.localScale = scale;
            }
        }
    
        public StateMachineTurn Turn;
    }
}