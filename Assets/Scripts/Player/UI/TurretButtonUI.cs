using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.TankMania.Player
{
  public class TurretButtonUI : MonoBehaviour, IPointerClickHandler
  {
    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text slotStatusName;
    [SerializeField] private Button unassignButton;

    private Button button;
    private TurretController turret;

    public event Action<TurretController> OnClickEvent;
    public event Action<TurretController> OnUnassignEvent;

    private void Awake()
    {
      button = GetComponent<Button>();
    }

    public void SetContent( TurretController t, TurretSlot assignedSlot )
    {
      turret = t;

      turretName.text = turret.name;
      slotStatusName.text = assignedSlot == null ? "Unassigned" : assignedSlot.name;

      unassignButton.gameObject.SetActive(assignedSlot != null);
    }

    public void OnPointerClick( PointerEventData eventData )
    {
      if (button.interactable) OnClickEvent?.Invoke(turret);
    }

    public void UnassignTurret()
    {
      OnUnassignEvent?.Invoke(turret);
    }
  }
}
