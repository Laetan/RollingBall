using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBarHandle : MonoBehaviour
{
    private Touch fingerFollowed;
    private Touch noFinger;
    public bool checkMouse = true;
    public float lowerPosition = 0;
    public float upperPosition = 100;

    private bool followMouse = false;

    // Start is called before the first frame update
    void Start()
    {
        noFinger.fingerId = -1;
        fingerFollowed = noFinger;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Bounds handleBounds = ((Renderer)gameObject.GetComponent<Renderer>()).bounds;
        if (fingerFollowed.fingerId == -1)
        {
            // Not following any finger right now, check if one is touching
            int fingerOnScreen = Input.touchCount;
            for(int i = 0; i < fingerOnScreen; i++)
            {
                Touch finger = Input.GetTouch(i);
                if (finger.phase != TouchPhase.Began)
                    continue;
                Vector3 fingerPos = ToWorldCoord(finger.position);
                if (handleBounds.Contains(fingerPos))
                {
                    Debug.Log("Handle touched !");
                    fingerFollowed = finger;
                    break;
                }
            }
        }
        else
        {
            switch (fingerFollowed.phase)
            {
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    FingerReleased();
                    break;
                case TouchPhase.Moved:
                    FingerMoved();
                    break;                    
            }
        }

        //Click input for debugging
        if (checkMouse)
        {
            if(!Input.GetMouseButton(0))
                followMouse = false;
            else
            {
                Vector3 mousePos = ToWorldCoord(Input.mousePosition);

                //Debug.Log("mouse pos : " + mousePos + " -- " + Input.mousePosition);
                

                if (followMouse)
                {
                    MoveTo(mousePos);
                }
                else if(handleBounds.Contains(mousePos))
                {
                    Debug.Log("click !");
                    followMouse = true;
                }
            }
        }
    }

    

    void FingerMoved()
    {
        MoveTo(ToWorldCoord(fingerFollowed.position));
    }

    void MoveTo(Vector3 moveTo)
    {
        Bounds handleBounds = ((Renderer)gameObject.GetComponent<Renderer>()).bounds;
        float delta = moveTo.y - handleBounds.center.y;
        float newY = gameObject.transform.position.y + delta;
        if (newY > upperPosition)
            newY = upperPosition;
        if (newY < lowerPosition)
            newY = lowerPosition;
        Vector3 newPos = gameObject.transform.position;
        newPos.y = newY;
        gameObject.transform.position = newPos;
    }

    void FingerReleased()
    {
        fingerFollowed = noFinger;
        Debug.Log("Handle released");
    }

    Vector3 ToWorldCoord( Vector2 pos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, - Camera.main.transform.position.z));
    }

}
