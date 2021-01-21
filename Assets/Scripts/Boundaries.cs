using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public Camera MainCamera;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight; 
    // Start is called before the first frame update
    void Start()
    {
        screenBounds = CheckIfInCamera.GetCameraBounds(); // get the Camera Boundaries
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;  // Get Width of the object which should stay inside the boundaries
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;  // Get Heigth of the object which should stay inside the boundaries
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition;
        newPosition = transform.position;

        newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x *-1 + objectWidth, screenBounds.x - objectWidth);    // Assigns a new X-Value to newPosition which is in between the X-ScreenBounds
        newPosition.y = Mathf.Clamp(newPosition.y, screenBounds.y *-1 + objectHeight, screenBounds.y - objectHeight);  // Assigns a new Y-Value to newPosition which is in between the Y-ScreenBounds 

        transform.position = newPosition;
    }
}
