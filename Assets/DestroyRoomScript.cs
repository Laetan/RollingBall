using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRoomScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject.transform.parent.gameObject);
    }
}
