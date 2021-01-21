using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfInCamera : MonoBehaviour
{

    public static Camera MainCamera;
    public Vector2 screenBounds;
    GameObject check;

    // Start is called before the first frame update
    void Start()
    {
        check = this.gameObject;
        screenBounds = GetCameraBounds();  //MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z)); // get the Camera Boundaries
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < screenBounds.y * -1 - 1.0f)  // -1.0f to make sure the object left the camera screen before it gets deleted
        {
            Destroy(check);
        }

        if (this.transform.position.y > screenBounds.y + 2.0f)   // +2.0f to make sure the object left the camera screen before it gets deleted
        {
            Destroy(check);
        }

        if (this.transform.position.x < screenBounds.x * -1 - 1.0f)
        {
            Destroy(check);
        }

        if (this.transform.position.x > screenBounds.x * +2.0f)
        {
            Destroy(check);
        }
    }

    public static Vector3 GetCameraBounds()
    {
        MainCamera = Camera.main;
        return MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z)); // get the Camera Boundaries
    }
}
