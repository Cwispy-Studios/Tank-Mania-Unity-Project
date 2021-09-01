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

        public float minHeight
        {
            get
            {
                return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
            }
        }
        
        public float maxHeight
        {
            get
            {
                return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
            }
        }
    }
}
