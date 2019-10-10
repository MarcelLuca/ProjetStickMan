﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 30.0f;

    private void Awake()
    {
        GetComponent<UnityEngine.Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}
