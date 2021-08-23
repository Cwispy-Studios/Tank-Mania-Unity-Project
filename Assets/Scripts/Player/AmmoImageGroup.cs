using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class AmmoImageGroup : MonoBehaviour
  {
    [SerializeField] private AmmoImage ammoImagePrefab;
    [SerializeField, Range(2, 10)] private int ammoImagesPerColumn = 2;
    public int AmmoImagesPerColumn => ammoImagesPerColumn;
    
    private AmmoImage[] ammoImages;

    public void Initialise()
    {
      ammoImages = new AmmoImage[ammoImagesPerColumn];

      for (int i = 0; i < ammoImagesPerColumn; ++i)
      {
        AmmoImage reticle = Instantiate(ammoImagePrefab, transform);

        ammoImages[i] = reticle;
      }
    }

    public void SetAllActive( bool active )
    {
      foreach (AmmoImage ammoImage in ammoImages)
        ammoImage.gameObject.SetActive(active);
    }

    public void SetAmountActive( int amount )
    {
      if (amount <= 0 || amount > ammoImagesPerColumn) 
        Debug.LogError($"Invalid number, {amount}, of reticles to enable!");

      SetAllActive(false);

      for (int i = 0; i < amount; ++i)
        ammoImages[i].gameObject.SetActive(true);
    }

    public AmmoImage this[int i]
    {
      get { return ammoImages[i]; }
    }

    private void OnDestroy()
    {
      foreach (AmmoImage ammoImage in ammoImages) Destroy(ammoImage);
    }
  }
}
