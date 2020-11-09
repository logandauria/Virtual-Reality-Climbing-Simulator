using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float gravity = -9.81f;
    public float speed = 1;
    public float additionalHeight = 0.2f;
    public XRNode inputSource;
    public LayerMask groundLayer;
    private float fallingSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<XRRig>();
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        // calculates movement direction given controller input
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * speed);

        // gravity calculations to determine whether or not to fall
        if (CheckIfGrounded())
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    // connects the VR rig to the capsule hitbox
    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    // checks to see if VR rig is grounded or not
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
