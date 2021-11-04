using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania.Player
{
  public class CameraController : MonoBehaviour
  {
    [Header("Base Tracking Values")]
    [SerializeField] private float baseHeight = 1.7f;
    [SerializeField] private float baseDistance = 3.5f;

    [Header("Sensitivity")]
    [SerializeField, Range(0.001f, 0.05f)] private float horizontalSensitivity = 0.01f;
    [SerializeField, Range(0.01f, 0.5f)] private float verticalSensitivity = 0.1f;

    [Header("Camera Targeting")]
    [SerializeField, Range(50f, 1000f)] private float rayDistance = 200f;

    [Header("Layer mask")]
    // Layer mask to ignore when finding object the crosshair is looking at
    [SerializeField] private LayerMask lookingAtIgnoreLayerMask;

    [Header("Variables")]
    [SerializeField] private FloatVariable targetHorizontalRotation;
    [SerializeField] private FloatVariable targetVerticalRotation;

    // Target turret that camera is controlling
    private TurretHub trackedTarget;
    private Camera playerCamera;

    private TurretRotationLimits rotationLimits;
    private Vector3 centerScreenPoint;

    private void Awake()
    {
      playerCamera = Camera.main;
      centerScreenPoint = new Vector3(playerCamera.pixelWidth / 2f, playerCamera.pixelHeight / 2f, 0f);
    }

    private void LateUpdate()
    {
      VerticalRotation();
      HorizontalRotation();
    }

    private void ProcessHorizontalInput( float horizontalInput )
    {
      targetHorizontalRotation.Value += horizontalInput * horizontalSensitivity;

      if (rotationLimits.HasYLimits)
      {
        bool rotationWithinLimits = targetHorizontalRotation.Value >= rotationLimits.MinYRot && targetHorizontalRotation.Value <= rotationLimits.MaxYRot;

        if (!rotationWithinLimits)
          targetHorizontalRotation.Value = Mathf.Clamp(targetHorizontalRotation.Value, rotationLimits.MinYRot, rotationLimits.MaxYRot);
      }
    }

    private void ProcessVerticalInput( float verticalInput )
    {
      targetVerticalRotation.Value += -verticalInput * verticalSensitivity;

      if (rotationLimits.HasXLimits)
      {
        bool rotationWithinLimits = targetVerticalRotation.Value >= rotationLimits.MinXRot && targetVerticalRotation.Value <= rotationLimits.MaxXRot;

        if (!rotationWithinLimits)
          targetVerticalRotation.Value = Mathf.Clamp(targetVerticalRotation.Value, rotationLimits.MinXRot, rotationLimits.MaxXRot);
      }
    }

    private void VerticalRotation()
    {
      float deltaRotation = targetVerticalRotation.Value - transform.rotation.eulerAngles.x;

      transform.RotateAround(trackedTarget.transform.position, transform.right, deltaRotation);
    }

    private void HorizontalRotation()
    {
      float deltaRotation = targetHorizontalRotation.Value - transform.rotation.eulerAngles.y;

      transform.RotateAround(trackedTarget.transform.position, Vector3.up, deltaRotation);
    }

    private void InitialiseCameraForNewTrackingTarget()
    {
      Vector3 targetPositionFromTrackedTarget = new Vector3(0f, baseHeight, -baseDistance);
      Quaternion targetRotation = Quaternion.Euler(0f, targetHorizontalRotation.Value, 0f);
      transform.position = trackedTarget.transform.position + targetRotation * targetPositionFromTrackedTarget;

      Vector3 targetRotationFromTrackedTarget = new Vector3(0f, 0f, 0f);
      transform.rotation = targetRotation * Quaternion.Euler(targetRotationFromTrackedTarget);

      targetVerticalRotation.Value = 0f;
    }

    public void SetTrackingTarget( TurretHub target )
    {
      trackedTarget = target;
      targetHorizontalRotation.Value = target.transform.rotation.eulerAngles.y;
      rotationLimits = target.TurretRotationLimits;

      InitialiseCameraForNewTrackingTarget();
    }

    public Vector3 GetCrosshairPosition()
    {
      Ray ray = playerCamera.ScreenPointToRay(centerScreenPoint);

      if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, ~lookingAtIgnoreLayerMask, QueryTriggerInteraction.Ignore))
        return hit.point;
      else
        return ray.origin + ray.direction * rayDistance;
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnAim( InputValue value )
    {
      Vector2 aimInput = value.Get<Vector2>();

      ProcessHorizontalInput(aimInput.x);
      ProcessVerticalInput(aimInput.y);
    }
  }
}
