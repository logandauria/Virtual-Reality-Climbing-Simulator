﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{
    private CharacterController character;
    public static XRController climbingHand;
    private ContinuousMovement continuousMovement;

    // Start is called before the first frame update
    void Start()
    {
        // initialize attributes
        character = GetComponent<CharacterController>();
        continuousMovement = GetComponent<ContinuousMovement>()
;    }

    // Update is called once per frame
    void Update()
    {
        if (climbingHand)
        {
            continuousMovement.enabled = false;
            Climb();
        } else
        {
            continuousMovement.enabled = true;
        }
    }

    //Climbing Computations
    void Climb()
    {
        // compute the movement using velocity and hand movement vectors
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
        character.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }
}