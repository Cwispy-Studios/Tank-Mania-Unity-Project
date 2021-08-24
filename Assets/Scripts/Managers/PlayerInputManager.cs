using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania
{
  [CreateAssetMenu(menuName = "Managers/Player Input Manager")]
  public class PlayerInputManager : ScriptableObject
  {
    public InputActionAsset playerInput;

    public void DisableInput()
    {
      playerInput.Disable();
    }

    public void EnableInput()
    {
      playerInput.Enable();
    }
  }
}
