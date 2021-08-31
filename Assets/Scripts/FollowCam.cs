using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform followPlayer;

    // offset from the player's position
    private Vector3 followOffset;

    void Start()
    {
        followOffset = transform.position - followPlayer.position;
    }

    void LateUpdate()
    {
        transform.position = followPlayer.position + followOffset;
    }
}