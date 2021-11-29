using UnityEngine;

namespace CwispyStudios.TankMania
{
  public class HideableWindow : MonoBehaviour
  {
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private bool pauseOnEnable = true;

    public void ShowWindow()
    {
      if (pauseOnEnable) timeManager.PauseGame();
      gameObject.SetActive(true);
    }

    public void HideWindow()
    {
      if (pauseOnEnable) timeManager.ResumeGame();
      gameObject.SetActive(false);
    }
  }
}
