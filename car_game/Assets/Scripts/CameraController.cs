using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject player;      
    public Vector3 offset;        

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}