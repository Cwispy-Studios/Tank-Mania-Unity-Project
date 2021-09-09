using System;
using CwispyStudios.TankMania.Combat;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  public class MeleeEnemy : Enemy
  {
    // Hit boxes should be added as children of this game object
    [SerializeField] private Transform[] hitBoxes;

    private Collider[] overlapBoxBuffer = new Collider[4];
    private LayerMask playerLayerMask;

    private void Awake()
    {
      playerLayerMask = LayerMask.GetMask("Player");
    }

    public void MeleeAttack(int hitBoxIndex, int damageAmount)
    {
      int size = Physics.OverlapBoxNonAlloc(hitBoxes[hitBoxIndex].position, hitBoxes[hitBoxIndex].lossyScale,
        overlapBoxBuffer, hitBoxes[hitBoxIndex].rotation, playerLayerMask);

      if (size == 0) return;

      for (int i = 0; i < size; i++)
      {
        Damageable otherDamageable = overlapBoxBuffer[i].GetComponentInParent<Damageable>();

        // Seems like this check is unnecessary since we already filter the enemies when using the player's layer mask @Cwispy
        if (otherDamageable.CanTakeDamageFromTeam(Team.Enemy))
          otherDamageable.TakeDamage(damageAmount);
      }
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      foreach (var box in hitBoxes)
      {
        if (box == null) continue;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(box.position,
          box.rotation, box.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
      }
    }
  }
}