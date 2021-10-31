using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class TurretsSlotsHandler : MonoBehaviour
  {
    // Slots where turrets can be placed on the tank
    [SerializeField] private TurretSlot[] turretSlots;
    public IEnumerable<TurretSlot> ListOfTurretSlots => turretSlots;

    // Reference to all available turrets that can be obtained in the game
    [SerializeField] private TurretContainer availableTurrets;

    // Unlocked turrets and which slots they are assigned to
    private Dictionary<Turret, TurretSlot> unlockedTurretsToSlots = new Dictionary<Turret, TurretSlot>();

    public IEnumerable<Turret> UnlockedTurrets => unlockedTurretsToSlots.Keys;

    public void UnlockRandomTurret()
    {
      var unlockedTurrets = unlockedTurretsToSlots.Keys;

      if (availableTurrets.ListOfTurrets.Count > unlockedTurrets.Count)
      {
        List<Turret> unlockableTurrets = new List<Turret>(availableTurrets.ListOfTurrets);

        // XOR the two lists to get turrets that have not been unlocked yet
        foreach (Turret unlockedTurret in unlockedTurrets)
          if (unlockableTurrets.Contains(unlockedTurret)) unlockableTurrets.Remove(unlockedTurret);

        int index;

        if (unlockableTurrets.Count == 1) index = 0;
        else index = Random.Range(0, unlockableTurrets.Count);

        // Create an instance of the turret
        Turret turretInstance = Instantiate(unlockableTurrets[index], transform);
        turretInstance.name = unlockableTurrets[index].name;
        turretInstance.gameObject.SetActive(false);

        unlockedTurretsToSlots.Add(turretInstance, null);
      }
    }

    public void AssignTurretToSlot( Turret turret, TurretSlot slot )
    {
      // Disable the turret if it has been previously assigned
      if (IsTurretAssigned(turret))
      {
        
      }

      unlockedTurretsToSlots[turret] = slot;

      turret.gameObject.SetActive(true);
      turret.transform.parent = slot.transform.parent;
      turret.transform.position = slot.transform.position;
      turret.transform.rotation = slot.transform.rotation;
      turret.SetTurretRotationLimits(slot.RotationLimits);
    }

    //private void 

    public bool IsTurretAssigned( Turret turret )
    {
      return unlockedTurretsToSlots[turret] != null;
    }

    public TurretSlot GetSlotOfTurret( Turret turret )
    {
      return unlockedTurretsToSlots[turret];
    }

    public Turret GetTurretOfSlot( TurretSlot slot )
    {
      foreach (Turret turret in unlockedTurretsToSlots.Keys)
      {
        if (unlockedTurretsToSlots[turret] == slot) return turret;
      }

      return null;
    }
  }
}
