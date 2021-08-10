using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  public abstract class Upgrade : ScriptableObject
  {
    [Header("Upgrade Descriptors")]
    public string UpgradeName;
    public Sprite UpgradeImage;
    [TextArea(2, 5)] public string UpgradeDescription;
  }
}
