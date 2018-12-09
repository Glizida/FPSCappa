using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{

    public int ammoGlok;
    public int ammoAK47;
    public int ammoM4A1;

    private void Awake()
    {
        ammoM4A1 = Random.Range(30, 60);
        ammoAK47 = Random.Range(30, 50);
        ammoGlok = Random.Range(10, 20);
    }
}
