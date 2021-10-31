using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  public class TurretsMenuUI : MonoBehaviour
  {
    [SerializeField] private TurretsSlotsHandler turretsSlotsHandler;
    [SerializeField] private Transform content;
    [SerializeField] private TurretButtonUI turretButtonPrefab;
    [SerializeField] private GunSlotsMenuUI gunSlotsMenu;

    private TurretSlot selectedSlot = null;

    private void OnEnable()
    {
      CreateUI();
    }

    private void OnDisable()
    {
      DestroyUI();

      selectedSlot = null;
    }

    private void CreateUI()
    {
      foreach (Turret turret in turretsSlotsHandler.UnlockedTurrets)
      {
        bool turretIsAssigned = turretsSlotsHandler.IsTurretAssigned(turret);

        TurretButtonUI button = Instantiate(turretButtonPrefab, content);
        button.SetContent(turret, turretsSlotsHandler.GetSlotOfTurret(turret));
        button.GetComponent<Button>().interactable = !turretIsAssigned;

        button.OnClickEvent += AssignTurretToSlot;
      }
    }

    private void DestroyUI()
    {
      foreach (Transform t in content)
      {
        Destroy(t.gameObject);
      }
    }

    private void RefreshUI()
    {
      DestroyUI();
      CreateUI();
    }

    private void AssignTurretToSlot( Turret turret )
    {
      turretsSlotsHandler.AssignTurretToSlot(turret, selectedSlot);

      RefreshUI();
      gunSlotsMenu.RefreshUI();
    }

    public void SetSelectedSlot( TurretSlot slot )
    {
      selectedSlot = slot;
    }
  }
}
