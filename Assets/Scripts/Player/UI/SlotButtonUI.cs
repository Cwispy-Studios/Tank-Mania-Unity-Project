using System;

using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;

namespace CwispyStudios.TankMania.Player
{
  public class SlotButtonUI : MonoBehaviour, IPointerClickHandler
  {
    [SerializeField] private TMP_Text slotName;
    [SerializeField] private TMP_Text turretName;

    private TurretSlot slot;

    public event Action<TurretSlot> OnClick;

    public void SetContent( TurretSlot turretSlot, Turret turretInSlot )
    {
      slot = turretSlot;

      slotName.text = slot.name;
      turretName.text = turretInSlot != null ? turretInSlot.name : "Unoccupied";
    }

    public void OnPointerClick( PointerEventData eventData )
    {
      OnClick?.Invoke(slot);
    }
  }
}
