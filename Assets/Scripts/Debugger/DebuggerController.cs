using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania.Debugger
{
  using Upgrades;

  public class DebuggerController : MonoBehaviour
  {
    [SerializeField] private UpgradePanel upgradePanel;
    private void Update()
    {
      if (Keyboard.current.uKey.wasPressedThisFrame) upgradePanel.ShowUpgradesPanel();
    }
  }
}
