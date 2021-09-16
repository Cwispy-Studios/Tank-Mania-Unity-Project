using UnityEngine;

namespace CwispyStudios.TankMania.Terrain
{
    public class HideOnPlay : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}

