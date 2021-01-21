using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private Animator anim;
    private Transform bullet;
    [SerializeField]
    public float bulletSpeed = 12;
    public Vector3 direction = new Vector3(0, -1, 0); // set in Prefab !!!
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bullet = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WinScreen.gamePaused)  // if game paused skip update method 
        {
            if(anim != null)
            {
                anim.speed = 0;
            }
            return;
        }
        else
        {
            if(anim != null)
            {
                anim.speed = 1;
            }
            
        }
        bullet.position += direction * EnemyHitDetection.difficulty * bulletSpeed * Time.deltaTime;  // faster bullets for higher difficulties
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision) // Destroy enemy bullet when hit with friendly bullet
    {
        Debug.Log("Bullet Trigger");
        if(EnemyHitDetection.Attacked(collision))
        {
            Debug.Log("Bullet Hit and destroyed");
            Destroy(collision.gameObject);  
            Destroy(this.gameObject);
        }
    }
}
