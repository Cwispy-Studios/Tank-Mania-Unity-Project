using System.Collections;
using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
  using Combat;
  public class CameraShaker : MonoBehaviour
  {
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeIntensity;

    private float halfIntensity;

    private void Awake()
    {
      BulletEvents.OnBulletFired += StartShake;
      BulletEvents.OnBulletHit += StartExtremeShake;

      halfIntensity = shakeIntensity / 2;
    }

    private void StartShake(Projectile projectile)
    {
      StartCoroutine(Shake());
    }

    private void StartExtremeShake(Projectile projectile)
    {
      StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
      float endTime = Time.time + shakeDuration;

      float x = 0;
      float y = 0;

      while (Time.time < endTime)
      {
        transform.localPosition = new Vector3((Mathf.PerlinNoise(x, 0) * shakeIntensity) - halfIntensity,
          Mathf.PerlinNoise(y, 0) * shakeIntensity - halfIntensity, 0);
        x += .1f;
        y += .2f;
        yield return null;
      }

      transform.localPosition = Vector3.zero;
    }
  }
}