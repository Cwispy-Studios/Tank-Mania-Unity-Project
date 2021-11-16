using UnityEngine;

namespace Enemy
{
  public class BirdAttractor : MonoBehaviour
  {
    [SerializeField] private float strokeTime;
    [SerializeField] private float strokeLength;

    private float lastStrokeStart = 0;

    // Update is called once per frame
    void Update()
    {
      transform.localPosition = (Vector3.left *
                                 (Mathf.Sin(((Time.time - lastStrokeStart) / strokeTime) * Mathf.PI) * strokeLength));
    }
  }
}