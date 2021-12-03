using System.Collections.Generic;
using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
  using Combat;
  public class ShockWaveHandler : MonoBehaviour
  {
    [SerializeField] private Shader shockWaveShader;
    [SerializeField] private float shootHeight;
    [SerializeField] private float hitHeight;

    private List<Material> shockWaveMaterials = new List<Material>();

    private int positionId;
    private int startId;
    private int timeId;
    private int heightId;

    private void Awake()
    {
      foreach (var renderer in FindObjectsOfType<Renderer>())
      {
        if (renderer.material.shader == shockWaveShader)
          shockWaveMaterials.Add(renderer.material);
      }

      BulletEvents.OnBulletFired += BulletFiredShockWave;
      BulletEvents.OnBulletHit += BulletHitShockWave;

      positionId = Shader.PropertyToID("_StartPos");
      startId = Shader.PropertyToID("_StartShockWave");
      timeId = Shader.PropertyToID("_CurrTime");
      heightId = Shader.PropertyToID("_Height");
    }

    private void BulletFiredShockWave(Projectile projectile)
    {
      StartShockWave(projectile, shootHeight);
    }

    private void BulletHitShockWave(Projectile projectile)
    {
      StartShockWave(projectile, hitHeight);
    }

    private void StartShockWave(Projectile projectile, float height)
    {
      Vector3 position = projectile.transform.position;

      foreach (var material in shockWaveMaterials)
      {
        material.SetVector(positionId, position);
        material.SetFloat(startId, 1);
        material.SetFloat(timeId, Time.time);
        material.SetFloat(heightId, height);
      }
    }
  }
}