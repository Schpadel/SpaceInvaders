using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Material m1;
    Vector2 offset;

    public float xVelocity, yVelocity;
    private void Awake()
    {
        m1 = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(xVelocity, yVelocity);

    }

    // Update is called once per frame
    void Update()
    {
        if (WinScreen.gamePaused)
        {
            return;
        }
        m1.mainTextureOffset += offset * Time.deltaTime;
    }
}
