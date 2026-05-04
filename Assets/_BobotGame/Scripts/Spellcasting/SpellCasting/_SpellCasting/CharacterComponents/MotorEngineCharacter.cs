using System;
using UnityEngine;

namespace SpellCasting
{
    public class MotorEngineCharacter : MotorEngine
    {
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private float gravity = 9.8f;

        private float _ySpeed;

        //jam todo smoooth movement. stop wasting time on shit god damn
        DateTime latestMoveTime;
        DateTime previousMoveTime;

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            //jam wait shouldn't this be in motordriver
                //nah
            if (characterController.isGrounded)
            {
                _ySpeed = 0;
            } else
            {
                _ySpeed -= Time.fixedDeltaTime * gravity;
            }
        }

        public override void FixedMove(Vector3 movement)
        {
            characterController.Move(movement + Vector3.up * _ySpeed);
        }

        public override void Teleport(Vector3 destination)
        {
            _ySpeed = 0;
            characterController.enabled = false;
            characterController.transform.position = destination;
            characterController.enabled = true;
        }

    }
}
