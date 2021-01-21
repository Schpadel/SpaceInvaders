using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Transform bullet;
    public float bulletSpeed = 4;
    public Vector3 direction = new Vector3(0, 1, 0);  // override in inspector or prefab for special attacks / different bullet behavior
    // Start is called before the first frame update
    void Start()
    {


        bullet = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WinScreen.gamePaused)
        {

            return;
        }

        bullet.position += direction * bulletSpeed * Time.deltaTime;

    }

}
