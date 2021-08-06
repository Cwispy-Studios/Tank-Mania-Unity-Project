using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [CreateAssetMenu(menuName = "Damage/Damage")]
  public class Damage : ScriptableObject
  {
    [HideInInspector]
    public Team DamageFrom;

    public float DirectDamage;

    [HideInInspector]
    public SplashDamage SplashDamage;
  }
}
