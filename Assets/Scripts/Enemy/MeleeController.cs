using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using Combat;

  public class MeleeController : MonoBehaviour
  {
    // Hit boxes should be added as children of this game object
    [Header("Melee attack information")] [SerializeField]
    private MeleeAttackType[] attackTypes;

    private Collider[] overlapBoxBuffer = new Collider[10];
    private LayerMask playerLayerMask;

    private float attackCountdown = 0f;

    private void Awake()
    {
      playerLayerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
      if (attackCountdown > 0f) attackCountdown -= Time.deltaTime;
    }

    public void TryAttack()
    {
      if (attackCountdown <= 0f) MeleeAttack(0);
    }

    public void MeleeAttack(int attackIndex)
    {
      if (attackIndex > attackTypes.Length)
        throw new NullReferenceException($"No hitbox with index with {attackIndex} on this melee controller");

      MeleeAttackType chosenAttackType = attackTypes[attackIndex];

      attackCountdown = chosenAttackType.AttackAttributes.AttackRate.Value;

      int size = Physics.OverlapBoxNonAlloc(chosenAttackType.HitBox.position,
        chosenAttackType.HitBox.lossyScale / 2f,
        overlapBoxBuffer, chosenAttackType.HitBox.rotation, playerLayerMask);

      if (size == 0) return;

      for (int i = 0; i < size; i++)
      {
        Damageable otherDamageable = overlapBoxBuffer[i].GetComponentInParent<Damageable>();

        // Seems like this check is unnecessary since we already filter the enemies when using the player's layer mask @Cwispy
        if (otherDamageable.CanTakeDamageFromTeam(Team.Enemy))
          otherDamageable.TakeDamage(chosenAttackType.AttackAttributes.Damage.DirectDamage.Value);
      }
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      foreach (var info in attackTypes)
      {
        if (info.HitBox == null) continue;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(info.HitBox.position,
          info.HitBox.rotation, info.HitBox.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
      }
    }
  }
}