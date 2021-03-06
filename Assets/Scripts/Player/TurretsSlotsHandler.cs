using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class TurretsSlotsHandler : MonoBehaviour
  {
    [Header("Components")]
    [SerializeField] private PlayerTurretController playerTurretController;

    [Header("References")]
    // Slots where turrets can be placed on the tank
    [SerializeField] private TurretSlot[] turretSlots;
    public IEnumerable<TurretSlot> ListOfTurretSlots => turretSlots;

    // Reference to all available turrets that can be obtained in the game
    [SerializeField] private TurretContainer availableTurrets;

    // Unlocked turrets and which slots they are assigned to
    private Dictionary<TurretHub, TurretSlot> unlockedTurretsToSlots = new Dictionary<TurretHub, TurretSlot>();
    // List of all unlocked turrets
    public IEnumerable<TurretHub> UnlockedTurrets => unlockedTurretsToSlots.Keys;

    public void UnlockRandomTurret()
    {
      var unlockedTurrets = unlockedTurretsToSlots.Keys;

      if (availableTurrets.ListOfTurrets.Count > unlockedTurrets.Count)
      {
        List<TurretHub> unlockableTurrets = new List<TurretHub>(availableTurrets.ListOfTurrets);

        // XOR the two lists to get turrets that have not been unlocked yet
        foreach (TurretHub unlockedTurret in unlockedTurrets)
          if (unlockableTurrets.Contains(unlockedTurret)) unlockableTurrets.Remove(unlockedTurret);

        int index;

        if (unlockableTurrets.Count == 1) index = 0;
        else index = Random.Range(0, unlockableTurrets.Count);

        // Create an instance of the turret
        TurretHub turretInstance = Instantiate(unlockableTurrets[index], transform);
        turretInstance.name = unlockableTurrets[index].name;
        turretInstance.gameObject.SetActive(false);

        unlockedTurretsToSlots.Add(turretInstance, null);
      }
    }

    public void AssignTurretToSlot( TurretHub turret, TurretSlot slot )
    {
      if (IsTurretAssigned(turret))
      {
        
      }

      unlockedTurretsToSlots[turret] = slot;

      turret.AssignToSlot(slot);
    }

    public void UnassignTurret( TurretHub turret )
    {
      unlockedTurretsToSlots[turret] = null;

      turret.gameObject.SetActive(false);
    }

    public bool IsTurretAssigned( TurretHub turret )
    {
      return unlockedTurretsToSlots[turret] != null;
    }

    public TurretSlot GetSlotOfTurret( TurretHub turret )
    {
      return unlockedTurretsToSlots[turret];
    }

    public TurretHub GetTurretOfSlot( TurretSlot slot )
    {
      foreach (TurretHub turret in unlockedTurretsToSlots.Keys)
      {
        if (unlockedTurretsToSlots[turret] == slot) return turret;
      }

      return null;
    }
  }
}
