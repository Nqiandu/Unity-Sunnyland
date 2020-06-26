using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC2 : MonoBehaviour
{
    public Transform player;


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, 0, -10f);
    }
}
