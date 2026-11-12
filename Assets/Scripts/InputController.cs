using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

        public Vector2 Move;
        public bool IsJumping;
        public Trigger IsAttacking;
        public Trigger IsLightAttacking;
        public bool IsBlocking; 
        public bool JumpPressed { get; private set; }

        public void OnMove(InputValue value) {
                Move = value.Get<Vector2>();
        }

        public void OnJump(InputValue value) {
                IsJumping = value.isPressed;

                if (value.isPressed)
                {
                        JumpPressed = true;
                }
        }

        public void ConsumeJumpPressed() {
                JumpPressed = false;
        }
        public void OnAttack(InputValue value) {
                if (value.isPressed) IsAttacking.Launch();
        }
        
        public void OnLightAttack(InputValue value) {
                if (value.isPressed) IsLightAttacking.Launch();
        }

        public void OnBlock(InputValue value) {
                 IsBlocking = value.isPressed;
        }
}