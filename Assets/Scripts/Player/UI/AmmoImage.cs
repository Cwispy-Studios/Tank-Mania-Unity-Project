using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  public class AmmoImage : MonoBehaviour
  {
    [SerializeField] private Image ammoImage;

    public void SetFill( float fill )
    {
      ammoImage.fillAmount = fill;
    }
  }
}
