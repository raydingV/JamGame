using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private float Speed = 1f;

    [SerializeField] bool Latency = true;

    private void Start()
    {
        gameObject.transform.position = playerTransform();
    }

    private void FixedUpdate()
    {
        if(player != null)
        {
            if(Latency == true)
            {
                FollowPlayerLate();
            }
            else
            {
                FollowPlayer();
            }
        }
    }
    
    Vector3 playerTransform()
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    void FollowPlayerLate()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position ,playerTransform(), Speed * Time.deltaTime);
    }

    void FollowPlayer()
    {
        gameObject.transform.position = playerTransform();
    }
}
