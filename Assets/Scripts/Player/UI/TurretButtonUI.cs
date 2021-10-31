using System;

using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;

namespace CwispyStudios.TankMania.Player
{
  public class TurretButtonUI : MonoBehaviour, IPointerClickHandler
  {
    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text slotStatusName;

    private Turret turret;

    public event Action<Turret> OnClickEvent;

    public void SetContent( Turret t, TurretSlot assignedSlot )
    {
      turret = t;

      turretName.text = turret.name;
      slotStatusName.text = assignedSlot == null ? "Unassigned" : assignedSlot.name;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
      OnClickEvent?.Invoke(turret);
    }
  }
}
