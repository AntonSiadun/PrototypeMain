using System;
using UnityEngine;

namespace PlayerElements
{
    public class PlayerMoving
    {
        private readonly Joystick _joystick;
        private  CharacterController _characterController;
        private const float Speed = 2f;

        public PlayerMoving(Joystick joystick, CharacterController characterController)
        {
            _joystick = joystick;
            _characterController = characterController;
        }

        public Vector3 GetInputVector()
        {
            var horizontal = -_joystick.Horizontal;
            var vertical = -_joystick.Vertical;
            return new Vector3(horizontal,0f,vertical);
        }
    
        public void MoveCharacterIfCanMove(bool canMove)
        {
            Vector3 movement = GetInputVector();
            if (movement.magnitude > 0)
            {
                movement.Normalize();
                movement *= Speed * Time.deltaTime;
                _characterController.Move(movement);
            }
        }

        public void TurnOffCharacterController()
        {
            _characterController.enabled = false;
        }

        public void TurnOnCharacterController()
        {
            _characterController.enabled = true;
        }
    }
}