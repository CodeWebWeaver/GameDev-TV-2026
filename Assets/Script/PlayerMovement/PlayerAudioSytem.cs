using FMODUnity;
using KinematicCharacterController;
using System;
using UnityEngine;
using Zenject;

public class PlayerAudioSytem : MonoBehaviour {
    [Inject] FmodAudioService audioService;
    [Inject] PlayerInputService input;
    [SerializeField] KinematicPlayerMovement controller;
    [SerializeField] KinematicCharacterMotor motor;

    [SerializeField] EventReference walkEvent;
    [SerializeField] EventReference onJump;
    

    private bool _walking;

    private void OnEnable() {
        controller.OnJumped += HandleJump;
    }

    private void HandleJump() {
        RuntimeManager.PlayOneShot(onJump, transform.position);
    }

    private void Update() {
        bool shouldWalk =
            motor.GroundingStatus.IsStableOnGround &&
            input.MoveInput.sqrMagnitude > 0.01f;

        if (shouldWalk == _walking)
            return;

        _walking = shouldWalk;

        if (_walking)
            audioService.PlayLooped("walking", walkEvent);
        else
            audioService.StopLooped("walking");
    }
}