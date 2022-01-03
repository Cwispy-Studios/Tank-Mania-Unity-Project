<<<<<<< HEAD
using System;
=======
>>>>>>> develop
using System.Collections.Generic;
using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
<<<<<<< HEAD
=======
  using Combat;
>>>>>>> develop
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

<<<<<<< HEAD
    private void BulletFiredShockWave(Projectile.Projectile projectile)
=======
    private void BulletFiredShockWave(Projectile projectile)
>>>>>>> develop
    {
      StartShockWave(projectile, shootHeight);
    }

<<<<<<< HEAD
    private void BulletHitShockWave(Projectile.Projectile projectile)
=======
    private void BulletHitShockWave(Projectile projectile)
>>>>>>> develop
    {
      StartShockWave(projectile, hitHeight);
    }

<<<<<<< HEAD
    private void StartShockWave(Projectile.Projectile projectile, float height)
=======
    private void StartShockWave(Projectile projectile, float height)
>>>>>>> develop
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