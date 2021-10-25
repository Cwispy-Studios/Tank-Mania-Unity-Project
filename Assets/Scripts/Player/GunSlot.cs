using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class GunSlot : MonoBehaviour
  {
    [SerializeField] private GunRotationLimits gunRotationLimits;

    private bool isUnlocked = false;
    public bool IsUnlocked => isUnlocked;
  }
}
