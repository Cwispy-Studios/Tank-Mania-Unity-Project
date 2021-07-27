using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
  public class VfxParentDisabler : MonoBehaviour
  {
    private int numberOfVfx = 0;
    private int numberOfVfxDisabled = 0;

    private CFX_AutoDestructShuriken[] autoDestructVfx;

    private void Awake()
    {
      // Look for CVX_AutoDestructShuriken child objects
      autoDestructVfx = GetComponentsInChildren<CFX_AutoDestructShuriken>(true);
      foreach (CFX_AutoDestructShuriken vfx in autoDestructVfx)
      {
        ++numberOfVfx;
        vfx.OnDeactivate += VfxDisabled;
      }

      // Look for CFX_AutoStopLoopedEffect child objects
      foreach (CFX_AutoStopLoopedEffect vfx in GetComponentsInChildren<CFX_AutoStopLoopedEffect>())
      {
        ++numberOfVfx;
        vfx.OnStop += VfxDisabled;
      }
    }

    private void OnEnable()
    {
      numberOfVfxDisabled = 0;

      foreach (CFX_AutoDestructShuriken vfx in autoDestructVfx) vfx.gameObject.SetActive(true);
    }

    private void VfxDisabled()
    {
      ++numberOfVfxDisabled;

      if (numberOfVfxDisabled >= numberOfVfx)
      {
        gameObject.SetActive(false);
      }
    }
  }
}
