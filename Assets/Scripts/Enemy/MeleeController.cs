using CwispyStudios.TankMania.Combat;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  public class MeleeController : MonoBehaviour
  {
    // Hit boxes should be added as children of this game object
    [Header("Melee attack information")] 
    [SerializeField] 
    private AttackInformation[] attackInformations;

    private Collider[] overlapBoxBuffer = new Collider[4];
    private LayerMask playerLayerMask;

    private void Awake()
    {
      playerLayerMask = LayerMask.GetMask("Player");
    }

    public void MeleeAttack(int attackIndex)
    {
      int size = Physics.OverlapBoxNonAlloc(attackInformations[attackIndex].hitBox.position,
        attackInformations[attackIndex].hitBox.lossyScale/2,
        overlapBoxBuffer, attackInformations[attackIndex].hitBox.rotation, playerLayerMask);

      if (size == 0) return;

      for (int i = 0; i < size; i++)
      {
        Damageable otherDamageable = overlapBoxBuffer[i].GetComponentInParent<Damageable>();

        // Seems like this check is unnecessary since we already filter the enemies when using the player's layer mask @Cwispy
        if (otherDamageable.CanTakeDamageFromTeam(Team.Enemy))
          otherDamageable.TakeDamage(attackInformations[attackIndex].damageStats.DirectDamage.Value);
      }
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      foreach (var info in attackInformations)
      {
        if (info.hitBox == null) continue;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(info.hitBox.position,
          info.hitBox.rotation, info.hitBox.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
      }
    }
  }
}