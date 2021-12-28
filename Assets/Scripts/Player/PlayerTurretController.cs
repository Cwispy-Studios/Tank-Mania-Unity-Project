using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class PlayerTurretController : MonoBehaviour
  {
    [Header("Camera Variables")]
    [SerializeField] private FloatVariable cameraHorizontalRotation;
    [SerializeField] private FloatVariable cameraVerticalRotation;

    [Header("Starting Turret")]
    [SerializeField] private TurretController startingTurret;

    private CameraController playerCamera;

    private TurretController turretHub;
    private GunController gun;

    private void Awake()
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      playerCamera = Camera.main.GetComponent<CameraController>();

      if (startingTurret != null) SetControlledTurret(startingTurret);
    }

    private void Update()
    {
      turretHub.RotateMountToValue(cameraHorizontalRotation.Value);
      turretHub.RotateGunToValue(cameraVerticalRotation.Value);
    }

    public void SetControlledTurret( TurretController controlledTurret )
    {
      // Enable AI turret controller of previous turret

      turretHub = controlledTurret;
      gun = turretHub.GetComponentInChildren<GunController>();

      playerCamera.SetTrackingTarget(turretHub);

      // Disable AI turret controller of current turret
    }

    /// <summary>
    /// Input Action callback
    /// </summary>
    private void OnMainFire()
    {
      gun.QueueFiring();
    }
  }
}
