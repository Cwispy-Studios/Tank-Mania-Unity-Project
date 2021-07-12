﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania.Camera
{
  using Tank;
  using Utils;

  public class CameraController : MonoBehaviour
  {
    [Header("Base Tracking Values")]
    [SerializeField] private float baseHeight = 1.7f;
    [SerializeField] private float baseDistance = 3.5f;
    [SerializeField] private float baseRotation = 8f;

    [Header("Vertical Rotation Limits")]
    [SerializeField] private float verticalRotationLimitFromBaseValue = 25f;

    [Header("Sensitivity")]
    [SerializeField, Range(0.001f, 0.05f)] private float horizontalSensitivity = 0.01f;
    [SerializeField, Range(0.01f, 0.5f)] private float verticalSensitivity = 0.1f;

    private TankTurretController trackedTarget;

    private float targetHorizontalRotationValue = 0f;
    private float targetVerticalRotationValue = 0f;

    private float minVerticalRotationLimit;
    private float maxVerticalRotationLimit;

    public float BaseRotation { get { return baseRotation; } }

    private void Awake()
    {
      minVerticalRotationLimit = MathHelper.ConvertToSignedAngle(baseRotation + verticalRotationLimitFromBaseValue);
      maxVerticalRotationLimit = MathHelper.ConvertToSignedAngle(baseRotation - verticalRotationLimitFromBaseValue);
    }

    private void LateUpdate()
    {
      VerticalRotation();
      HorizontalRotation();
    }

    private void ProcessHorizontalInput( float horizontalInput )
    {
      targetHorizontalRotationValue += horizontalInput * horizontalSensitivity;
      targetHorizontalRotationValue = MathHelper.ConvertToSignedAngle(targetHorizontalRotationValue);
    }

    private void ProcessVerticalInput( float verticalInput )
    {
      targetVerticalRotationValue += -verticalInput * verticalSensitivity;
      targetVerticalRotationValue = MathHelper.ConvertToSignedAngle(targetVerticalRotationValue);
      targetVerticalRotationValue = Mathf.Clamp(targetVerticalRotationValue, maxVerticalRotationLimit, minVerticalRotationLimit);
    }

    private void VerticalRotation()
    {
      float signedEulerAngle = MathHelper.ConvertToSignedAngle(transform.rotation.eulerAngles.x);
      float deltaRotation = targetVerticalRotationValue - signedEulerAngle;

      transform.RotateAround(trackedTarget.transform.position, transform.right, deltaRotation);
    }

    private void HorizontalRotation()
    {
      float signedEulerAngle = MathHelper.ConvertToSignedAngle(transform.rotation.eulerAngles.y);
      float deltaRotation = targetHorizontalRotationValue - signedEulerAngle;

      transform.RotateAround(trackedTarget.transform.position, Vector3.up, deltaRotation);
    }

    private void InitialiseCameraForNewTrackingTarget()
    {
      Vector3 targetPositionFromTrackedTarget = new Vector3(0f, baseHeight, -baseDistance);
      Quaternion targetRotation = Quaternion.Euler(0f, targetHorizontalRotationValue, 0f);
      transform.position = trackedTarget.transform.position + targetRotation * targetPositionFromTrackedTarget;

      Vector3 targetRotationFromTrackedTarget = new Vector3(baseRotation, 0f, 0f);
      transform.rotation = targetRotation * Quaternion.Euler(targetRotationFromTrackedTarget);

      targetVerticalRotationValue = baseRotation;
    }

    public void SetTrackingTarget( TankTurretController target )
    {
      trackedTarget = target;
      targetHorizontalRotationValue = MathHelper.ConvertToSignedAngle(target.transform.rotation.eulerAngles.y);

      InitialiseCameraForNewTrackingTarget();
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnAim( InputValue value )
    {
      Vector2 aimInput = value.Get<Vector2>();

      ProcessHorizontalInput(aimInput.x);
      ProcessVerticalInput(aimInput.y);

      trackedTarget.ReceiveSignedRotation(targetHorizontalRotationValue, targetVerticalRotationValue);
    }
  }
}