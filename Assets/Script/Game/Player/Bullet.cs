using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage=10;
    private void FixedUpdate()
    {

        transform.Rotate(20,20,0);
    }

}
