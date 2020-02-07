using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
	public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
		gameObject.transform.Translate(0, -1 * Time.deltaTime * speed,0);
    }
}
