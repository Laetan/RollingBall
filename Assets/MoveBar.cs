using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveBar : MonoBehaviour
{
    public Transform leftHandle, rightHandle;
    private float xLeft, xRight;
    private Rigidbody2D body;
    private Vector3 normalScale;
    private float normalSize;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        normalScale = gameObject.transform.localScale;
        xLeft = leftHandle.position.x;
        xRight = rightHandle.position.x;
        normalSize = xRight - xLeft;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yLeft = leftHandle.position.y;
        float yRight = rightHandle.position.y;
        double rotation = Math.Atan((yLeft - yRight) / normalSize) * 180 / Math.PI;
        body.MovePosition(new Vector3(xLeft, yLeft, 0));
        body.MoveRotation((float)rotation * -1f);
        //Debug.Log(String.Format("{0} - {1} / {2}", yLeft , yRight,  normalSize));
        double newSize = Math.Sqrt(Math.Pow(normalSize, 2) + Math.Pow(yLeft - yRight, 2));
        //Debug.Log(newSize);
        gameObject.transform.localScale = new Vector3(normalScale.x * (float)(newSize/normalSize), normalScale.y, normalScale.z);
        //Debug.Log(normalScale);
    }
}
