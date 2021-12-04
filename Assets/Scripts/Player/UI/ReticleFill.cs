using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class ReticleFill : MonoBehaviour
  {
    [SerializeField] private FloatVariable firingCountdown;
    [SerializeField] private ProjectileAttackAttributes firingInformation;

    private Image reticleImage;

    private void Awake()
    {
      reticleImage = GetComponent<Image>();
    }

    private void Update()
    {
      reticleImage.fillAmount = 1f - Mathf.Clamp01(firingCountdown.Value / firingInformation.AttackRate.Value);
    }
  }
}
