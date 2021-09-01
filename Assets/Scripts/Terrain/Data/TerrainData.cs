using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CwispyStudios.TankMania.Terrain
{
    [CreateAssetMenu()]
    public class TerrainData : UpdatableData
    {
        public float uniformScale = 2f; //Change this to increase world scale / chunk size
        
        public bool useFlatShading;
        public bool useFalloff;

        public float meshHeightMultiplier;
        public AnimationCurve meshHeightCurve;
    }
}
