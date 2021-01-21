using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitDetection : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemy;
    public GameObject bullet = null;
    public GameObject enemyBullet;
    public static int difficulty = 1; // is being set by LoadSceneOnClick.cs file depending on the selected difficulty!
    public int life = 10;
    public Text score;
    public Text money;
    public static int scoreInt = 0;   // static because same score for all instances of enemies
    public static int moneyInt = 0;
    public float fireRate = 1;
    public float nextFire = 0;
    private int temp = 0;
    private float objectHeight;
    private float objectWidth;
    public Animator anim;
    bool attackBlob;
    bool dead = false;
    private float blobAttackStartPositionY;
    public GameObject floatingTextPrefab;
    private FloatingTextDecay oldText;




    void Start()
    {
        anim = GetComponent<Animator>();
        life *= difficulty;
        score = GameObject.Find("Score").GetComponent<Text>();
        Invoke("Shoot", 1); // recursion
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;





    }

    // Update is called once per frame
    void Update()
    {
        if (WinScreen.gamePaused && anim != null && (enemy.name.Contains("GunAlien") || enemy.name.Contains("Boss")))
        {
           // anim.SetTrigger("paused"); animation trigger not instant, so change to anim.speed more usefull
            anim.speed = 0;
        }
        else if (!WinScreen.gamePaused && anim != null && (enemy.name.Contains("GunAlien") || enemy.name.Contains("Boss")))
        {
            // anim.SetTrigger("unpaused"); animation trigger not instant, so change to anim.speed more usefull
            anim.speed = 1;
        }
        score.text = "Score: " + scoreInt.ToString();

        if (attackBlob)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, GameObject.Find("Player").GetComponent<Transform>().position.y), 20 * Time.deltaTime);
        }



        // If enemy leaves screen on the left side port back to the right side
        if (transform.position.x < CheckIfInCamera.GetCameraBounds().x * (-1))
        {
            transform.position = new Vector2(CheckIfInCamera.GetCameraBounds().x, transform.position.y);
        }

        // If enemy leaves screen on the right side port back to the left side
        if (transform.position.x > CheckIfInCamera.GetCameraBounds().x)
        {
            transform.position = new Vector2(CheckIfInCamera.GetCameraBounds().x * (-1), transform.position.y);
        }

        //*********** Fixed Shoot Interval ***********************************************
        /*  if (Time.time > nextFire)
         {
             Invoke("Shoot", 4);
             nextFire = Time.time + fireRate;
             Debug.Log("Enemy Shoot!");
         }*/
        //*********************************************************************************
    }
    public void Shoot()
    {

        float randomTime = Random.Range(5.0f, 10.0f);


        if (anim != null && !WinScreen.gamePaused) // only play animations and shoot if game is not paused !
        {
            anim.SetTrigger("attack");
        }

        if (anim == null)
        {

            if (temp > 0)  // don't shoot at first call of shoot method to make sure not every enemy shoots at the same time --> no random nummber,yet!
            {
                Instantiate(enemyBullet, new Vector2(enemy.transform.position.x, enemy.transform.position.y - objectHeight), enemyBullet.transform.rotation);
            }

        }

        Invoke("Shoot", randomTime);
        temp++;

    }


    public void GunAlienShoot()
    {
        Instantiate(enemyBullet, new Vector2(enemy.transform.position.x - objectWidth, enemy.transform.position.y - objectHeight), enemyBullet.transform.rotation);
        Instantiate(enemyBullet, new Vector2(enemy.transform.position.x + (objectWidth / 2), enemy.transform.position.y - objectHeight), enemyBullet.transform.rotation);
    }

    public void BlobAlienAttack()
    {
        blobAttackStartPositionY = transform.position.y;
        attackBlob = true;  // attack is being executed in the update method, so the charge does not happen instantly     
    }

    public void BossShoot()
    {
        
        Instantiate(enemyBullet, new Vector2(enemy.transform.position.x - 2.0f, enemy.transform.position.y), enemyBullet.transform.rotation);
        StartCoroutine("Barrage");
    }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
    private IEnumerator Barrage()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
    {
        for (int i = 0; i <= 6; i++)
        {


            Instantiate(enemyBullet, new Vector2(enemy.transform.position.x - 2.0f, enemy.transform.position.y), enemyBullet.transform.rotation);

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {

        if (Attacked(collision))
        {
            bullet = collision.gameObject;
            Destroy(bullet);    // Destroy bullet, which hit the enemy 

            life--;

            if (floatingTextPrefab != null)
            {
                ShowFloatingText();
            }

            //if (anim != null)   placeholder for when a good attacked animation is found 
            //{
            //    anim.SetTrigger("attacked");
            //}

            if (life <= 0)
            {
                if (enemy.name.Contains("GunAlien") || enemy.name.Equals("Boss"))
                {
                    Debug.Log("Death Trigger SET");
                    anim.SetTrigger("death");
                }
                else
                {
                    Destroy(enemy);
                }
                if (!dead)
                {
                    dead = true;
                    scoreInt += 25 * difficulty;
                    if (difficulty == 0.5)
                    {
                        moneyInt += 25;
                    }
                    else if (difficulty == 1)
                    {
                        moneyInt += 20;
                    }
                    else
                    {
                        moneyInt += 15;
                    }
                }
                score.text = "Score: " + scoreInt.ToString();

            }

            Debug.Log("Enemy was hit by Player!");
        }



    }

    public void BlobAttackEnd() // aufrufen am ende der Attack animation
    {
        attackBlob = false;
        transform.position = new Vector3(transform.position.x, blobAttackStartPositionY, transform.position.z);  // port back to starting position, maybe later change to slowly move back ?
    }



    void ShowFloatingText()
    {
        if (life >= 0)
        {
            Debug.Log("Spawn Text");
            var fText = Instantiate(floatingTextPrefab, new Vector3(transform.position.x, transform.position.y + objectHeight, transform.position.z), Quaternion.identity, transform);  // Display Text Mesh 1 unit above Player Position
            fText.GetComponent<TextMesh>().text = life.ToString();
            if (oldText != null)
            {
                oldText.DestroyOldText();
            }
            oldText = fText.GetComponent<FloatingTextDecay>();
        }
        else
        {
            Debug.Log("Life <= 0, not showing floating Text");
        }

    }

    public void OnGunAlienDeath() // used in GunAlien Death animation, called by last frame of the death animation
    {
        Destroy(enemy);
    }

    public void OnBossDeath() // used in Boss Death animation, called by last frame of the death animation 
    {
        Destroy(enemy);
    }
    public static bool Attacked(Collider2D collision)
    {

        // ToDo: Rework using Tag System, change Prefab Tag of Player Bullets to PlayerProjectile and check this instead of the name!
        if (collision.gameObject.name.Equals("Bullet(Clone)"))
        {
            return true;
        }

        if (collision.gameObject.name.Equals("Multishot(Clone)") || collision.gameObject.name.Equals("Bullet") || collision.gameObject.name.Equals("Bullet (1)"))
        {
            return true;
        }

        if (collision.gameObject.name.Equals("BulletHorizontal(Clone)"))
        {
            return true;
        }

        if (collision.gameObject.name.Equals("MultishotHorizontal(Clone)"))
        {
            return true;
        }



        return false;
    }
}



