using UnityEngine;

namespace CwispyStudios.TankMania.Terrain
{
    public class HideOnPlay : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}

