using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania.Debugger
{
  using Player;

  public class DebuggerController : MonoBehaviour
  {
    [SerializeField] private HideableWindow upgradePanel;
    [SerializeField] private HideableWindow gunTurretsMenu;
    [SerializeField] private TurretsSlotsHandler turretHandler;

    private void Update()
    {
      if (Keyboard.current.uKey.wasPressedThisFrame)
        upgradePanel.ShowWindow();

      if (Keyboard.current.tKey.wasPressedThisFrame)
        gunTurretsMenu.ShowWindow();

      if (Keyboard.current.aKey.wasPressedThisFrame)
        turretHandler.UnlockRandomTurret();
    }
  }
}
