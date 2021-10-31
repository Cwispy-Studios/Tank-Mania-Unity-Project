using UnityEngine;

namespace CwispyStudios.TankMania
{
  [CreateAssetMenu(menuName = "Managers/Time Manager")]
  public class TimeManager : ScriptableObject
  {
    public PlayerInputManager PlayerInput;

    private float timeScale;

    public void PauseGame()
    {
      timeScale = Time.timeScale;
      Time.timeScale = 0f;

      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.Confined;

      PlayerInput.DisableInput();
    }

    public void ResumeGame()
    {
      Time.timeScale = timeScale;

      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;

      PlayerInput.EnableInput();
    }
  }
}
