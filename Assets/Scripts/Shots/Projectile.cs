﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : Shot {

    public float MaxDuration;
    // Use this for initialization
    void Awake()
    {
        CurrentWaypoints = new List<Vector2>();
    }

    void Start () {
        Destroy(gameObject, MaxDuration);
        StartCoroutine("Move");
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        Ship ship = collider.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            if (this.tag == "FriendlyShot" && ship.tag == "Hostile" ||
                this.tag == "HostileShot" && ship.tag == "Friendly")
            {
                ship.TakeDamage(Damage);
                Instantiate(HitAnimation, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        /*
        ShipCore shipCore = collider.gameObject.GetComponent<ShipCore>();
        if (shipCore != null)
        {
            if (this.tag == "FriendlyShot" && shipCore.tag == "Hostile" ||
                this.tag == "HostileShot" && shipCore.tag == "Friendly")
            {
                shipCore.TakeDamage(Damage);
                Instantiate(HitAnimation, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        */
    }
}