using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.TankMania.Player
{
  public class SlotButtonUI : MonoBehaviour, IPointerClickHandler
  {
    [SerializeField] private TMP_Text slotName;
    [SerializeField] private TMP_Text turretName;

    private Button button;
    private TurretSlot slot;

    public event Action<TurretSlot> OnClick;

    private void Awake()
    {
      button = GetComponent<Button>();
    }

    public void SetContent( TurretSlot turretSlot, TurretHub turretInSlot )
    {
      slot = turretSlot;

      slotName.text = slot.name;
      turretName.text = turretInSlot != null ? turretInSlot.name : "Unoccupied";
    }

    public void OnPointerClick( PointerEventData eventData )
    {
      if (button.interactable) OnClick?.Invoke(slot);
    }
  }
}
