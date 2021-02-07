using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPar : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "building")
        {
            other.GetComponent<Building>().HP--;
        }
    }
}
