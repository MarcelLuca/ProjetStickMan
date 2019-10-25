﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaisseControler : MonoBehaviour
{

    public int hitPoint; //The ammount of hp la caisse doit avoir
    public bool isDestroyed;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPoint <= 0)
        {
            isDestroyed = true;
        }

        if(isDestroyed)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int ammount)
    {
        this.hitPoint -= ammount;
        GetComponent<AudioSource>().Play();
        
    }
}
