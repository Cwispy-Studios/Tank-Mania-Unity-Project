using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Upgrades
{
  public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
  {
    private Image imageComponent;
    private Upgrade upgradeComponent;

    public Action<Upgrade> OnHoverEvent;
    public Action OnNotHoverEvent;
    public Action<Upgrade> OnSelectEvent;

    private void Awake()
    {
      imageComponent = GetComponent<Image>();
    }

    public void OnPointerEnter( PointerEventData eventData )
    {
      OnHoverEvent?.Invoke(upgradeComponent);
    }

    public void OnPointerExit( PointerEventData eventData )
    {
      OnNotHoverEvent?.Invoke();
    }
    public void OnPointerClick( PointerEventData eventData )
    {
      OnSelectEvent?.Invoke(upgradeComponent);
    }

    public void SetUpgradeOfButton( Upgrade upgrade )
    {
      upgradeComponent = upgrade;

      imageComponent.sprite = upgrade.UpgradeImage;
    }
  }
}
