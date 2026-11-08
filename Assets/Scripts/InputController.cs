using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

        public Vector2 Move;
        public bool IsJumping;
        public Trigger IsAttacking;
        public Trigger IsHeavyAttacking;

        public void OnMove(InputValue value) {
                Move = value.Get<Vector2>();
                
        }

        public void OnJump(InputValue value) {
                IsJumping = value.isPressed;
        }

        public void OnAttack(InputValue value) {
                if (value.isPressed) IsAttacking.Launch();
        }
        
        public void OnHeavyAttack(InputValue value) {
                if (value.isPressed) IsHeavyAttacking.Launch();
        }
}