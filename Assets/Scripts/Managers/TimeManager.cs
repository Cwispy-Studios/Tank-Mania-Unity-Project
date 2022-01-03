using UnityEngine;

namespace CwispyStudios.TankMania
{
  [CreateAssetMenu(menuName = "Managers/Time Manager")]
  public class TimeManager : ScriptableObject
  {
    public PlayerInputManager PlayerInput;

    private bool isPaused = false;
    private float timeScale;

    private void OnEnable()
    {
      isPaused = false;
    }

    public void PauseGame()
    {
      if (isPaused) return;

      timeScale = Time.timeScale;
      Time.timeScale = 0f;

      isPaused = true;

      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.Confined;

      PlayerInput.DisableInput();
    }

    public void ResumeGame()
    {
      Time.timeScale = timeScale;

      isPaused = false;

      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;

      PlayerInput.EnableInput();
    }
  }
}
