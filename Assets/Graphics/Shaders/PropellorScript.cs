using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellorScript : MonoBehaviour
{
    [SerializeField] private float rotationIncrement;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, Time.deltaTime * rotationIncrement);
    }
}
