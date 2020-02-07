using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{
    public float Speed = 1f;
    private Rigidbody2D body;
    public float leftPosition, rightPosition;
    public float zoneSize = 10f;
    public float xOffset;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        leftPosition = body.position.y;
        rightPosition = leftPosition;
        xOffset = body.position.x;
        //Debug.Log((Math.Atan(0) * 180 / Math.PI).ToString("0.000"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float leftMove = Input.GetAxis("BarLeft") * Speed;
        float rightMove = Input.GetAxis("BarRight") * Speed;

		leftPosition = Math.Max(leftPosition + leftMove , -5 );
		rightPosition = Math.Max(rightPosition + rightMove,  -5 );
        double rotation = Math.Atan((leftPosition - rightPosition) / zoneSize) * 180 / Math.PI;
        //Debug.Log(rotation.ToString("0.000") );
        body.MovePosition(new Vector2(xOffset, leftPosition));
        body.MoveRotation((float)rotation * -1f );
    }
}
