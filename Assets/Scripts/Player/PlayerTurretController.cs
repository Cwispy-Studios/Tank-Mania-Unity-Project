using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class PlayerTurretController : MonoBehaviour
  {
    [Header("Camera Variables")]
    [SerializeField] private FloatVariable cameraHorizontalRotation;
    [SerializeField] private FloatVariable cameraVerticalRotation;

    [Header("Starting Turret")]
    [SerializeField] private Turret startingTurret;

    private CameraController playerCamera;

    private Turret turret;
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
      turret.RotateTurretToValue(cameraHorizontalRotation.Value);
      turret.RotateGunToValue(cameraVerticalRotation.Value);
    }

    public void SetControlledTurret( Turret controlledTurret )
    {
      // Enable AI turret controller of previous turret

      turret = controlledTurret;
      gun = turret.GetComponentInChildren<GunController>();

      playerCamera.SetTrackingTarget(turret);

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
