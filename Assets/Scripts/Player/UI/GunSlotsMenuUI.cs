using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  public class GunSlotsMenuUI : MonoBehaviour
  {
    [SerializeField] private TurretsSlotsHandler turretsSlots;
    [SerializeField] private Transform content;
    [SerializeField] private SlotButtonUI slotButtonPrefab;
    [SerializeField] private TurretsMenuUI turretsMenu;

    private void OnEnable()
    {
      CreateUI();
    }

    private void OnDisable()
    {
      DestroyUI();

      turretsMenu.gameObject.SetActive(false);
    }

    private void CreateUI()
    {
      foreach (TurretSlot turretSlot in turretsSlots.ListOfTurretSlots)
      {
        SlotButtonUI button = Instantiate(slotButtonPrefab, content);
        button.SetContent(turretSlot, turretsSlots.GetTurretOfSlot(turretSlot));
        button.OnClick += SetSelectedSlot;
      }
    }

    private void DestroyUI()
    {
      foreach (Transform t in content)
      {
        Destroy(t.gameObject);
      }
    }

    public void RefreshUI()
    {
      DestroyUI();
      CreateUI();
    }

    private void SetSelectedSlot( TurretSlot slot )
    {
      turretsMenu.gameObject.SetActive(true);
      turretsMenu.SetSelectedSlot(slot);
    }
  }
}
