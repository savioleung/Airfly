using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float HP = 10;
    private void FixedUpdate()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
