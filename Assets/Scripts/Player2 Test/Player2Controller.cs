using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player2Controller : MonoBehaviour
{
    public GameObject player;


    public static String playerName;
    public static float speed = 5f;
    public UnityEngine.Object bullet;
    public UnityEngine.Object multishot;
    private GameObject bulletHit;
    public static float fireRate = 0.5f;
    private float nextFire;
    private static int life = 5;
    public static int maxlife = 10;
    public static int lifeAtStart = 5;
    public static Text lifeTextPlayer2;
    public float dashDistance;
    private float nextDash;
    public static float dashRate = 5.0f;
    public GameObject gameOverScreen;
    public static FiringMode PlayerFiringMode = new FiringMode("SINGLESHOT"); // Firing mode is being set by the shop if item is bought
    public GameObject floatingTextPrefab;
    public GameObject pauseScreen;
    // private bool rotatedForBossLevel = false;

    public AudioClip shootSound;
    public AudioClip playerDieSound;
    public AudioClip multiShootSound;
    private float input;
    private float input2;
    // Start is called before the first frame update
    void Start()
    {

       // PlayerFiringMode.SetMultishot();//FOR TESTING REASONS
        player = this.gameObject;
        lifeTextPlayer2 = GameObject.Find("LifeTextPlayer2").GetComponent<Text>();

        UpdateHealthText();
        
        
        
    }


    // Update is called once per frame
    void Update()
    {
        if (LevelManager.multiplayer)
        {
            this.gameObject.SetActive(true);
        }
        // nicht mehr notwendig, da Player2 bereits gedreht in Szene 
        //if(LevelManager.level == 2 && !rotatedForBossLevel)
        //{
        //    transform.Rotate(0, 0, -90);
        //    bullet = Resources.Load("Prefabs/BulletHorizontal");
        //    multishot = Resources.Load("Prefabs/MultishotHorizontal");
        //    rotatedForBossLevel = true;
        //}

        //if  (LevelManager.level != 2 && rotatedForBossLevel)
        //{
        //    transform.Rotate(0, 0, 90);
        //    bullet = Resources.Load("Prefabs/Bullet");
        //    multishot = Resources.Load("Prefabs/Multishot");
        //    rotatedForBossLevel = false;
        //}


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            WinScreen.gamePaused = true;  // Pause game when escape is pressed
            pauseScreen.SetActive(true);  // show pause menu
        }


        if (WinScreen.gamePaused)  // if game paused skip update method 
        {
            Debug.Log("Paused!");
            return;
        }

        input = Input.GetAxisRaw("Player2Horizontal");
        input2 = Input.GetAxisRaw("Player2Vertical");
        player.transform.position += Vector3.right * input * speed * Time.deltaTime;
        player.transform.position += Vector3.up * input2 * speed * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Keypad0) && Time.time > nextFire)
        {
            Invoke("SpawnBullet", 0);
            nextFire = Time.time + fireRate;
        }

        ExecuteDash();

 

    }

    private void ExecuteDash()
    {

        var dashKey = KeyCode.RightShift; // set Dash Key here 

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.LeftArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.left * dashDistance;
            Debug.Log(player.transform.position);
            nextDash = Time.time + dashRate;
            //  Debug.Log("LShift and A pressed");

        }

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.RightArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.right * dashDistance;
            nextDash = Time.time + dashRate;
        }

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.UpArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.up * dashDistance;
            Debug.Log(player.transform.position);
            nextDash = Time.time + dashRate;

        }

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.DownArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.down * dashDistance;
            Debug.Log(player.transform.position);
            nextDash = Time.time + dashRate;

        }
    }

    public static void SetLife(int newLife)
    {
        life = newLife;
    }

    public static int GetLife()
    {
        return life;
    }

    public static void ResetPlayerUpgrades()
    {
        SetLife(lifeAtStart);
        SetSpeed(5.0f);
        fireRate = 0.5f;
        dashRate = 5;
        PlayerFiringMode.SetSingleshot();
    }



    public static void SetSpeed(float pspeed)
    {
        speed = pspeed;
    }

    public static void IncreaseSpeed()
    {
        speed += 0.5f;
    }

    public static float GetSpeed()
    {
        return speed;
    }

    public void SpawnBullet()
    {
        var rotation = player.transform.rotation;
        rotation *= Quaternion.Euler(0, 0, 90);
        if (PlayerFiringMode.IsSingleshot())
        {
            SoundManager.instance.PlaySingle(shootSound);
            if (LevelManager.level == 2)
            {
                Instantiate(bullet, new Vector2(player.transform.position.x +1.0f, player.transform.position.y), rotation); //fine tuning, start from first level else level manger != 2
            }
            else
            {
                Instantiate(bullet, new Vector2(player.transform.position.x, player.transform.position.y + 1.0f), rotation);
            }

            
        }

        if (PlayerFiringMode.IsMultishot())
        {
            Debug.Log("Multishot!");
            SoundManager.instance.PlaySingle(multiShootSound);
            if (LevelManager.level == 2)
            {
                // SoundManager.PlayShootSound() implement
                Instantiate(multishot, new Vector2(player.transform.position.x +1.0f, player.transform.position.y), rotation);
            }
            else
            {
                // SoundManager.PlayShootSound() implement
                Instantiate(multishot, new Vector2(player.transform.position.x, player.transform.position.y + 1.0f), rotation);
            }
            
        }

    }

    public static void UpdateHealthText()
    {
        try
        {
            lifeTextPlayer2.text = "Player 2 Lifes: " + life.ToString();
        }
        catch (NullReferenceException)
        {
            Debug.Log("LifeText Player 2 not assigned!");
        }

    }


    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {


            bulletHit = collision.gameObject;
            Debug.Log(collision.gameObject);
            Destroy(bulletHit);    // Destroy bullet, which hit the player 

            life--;
            SoundManager.instance.PlaySingle(playerDieSound);
            if (floatingTextPrefab != null)
            {
                ShowFloatingText();
            }
            UpdateHealthText();
            if (life <= 0)
            {
                Destroy(player);

                if(LevelManager.multiplayer)
                {
                    if(PlayerController.GetLife() <= 0)
                    {
                        SoundManager.instance.musicSource.Stop();
                        gameOverScreen.SetActive(true);   // Load GameOver Scene
                        HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, playerName);  // adds Player Score on death with name set in main menu to the highscore board  
                        Debug.Log("Score saved!");
                    }
                }
                

            }

            Debug.Log("PlayerHit by Bullet!");
        }

        if (collision.gameObject.CompareTag("BlobAlien") || collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("GunAlien"))
        {
            if (collision.gameObject.CompareTag("BlobAlien"))
            {
                life -= 3;  // damage from blobAlienChargeAttack
                Destroy(collision.gameObject);
                SoundManager.instance.PlaySingle(playerDieSound);
            }
            else
            {
                life--;
                SoundManager.instance.PlaySingle(playerDieSound);
            }

            if (floatingTextPrefab != null)
            {
                ShowFloatingText();
            }

            UpdateHealthText();
            if (life <= 0)
            {
                
                Debug.Log("Kill Player!");
                Destroy(player);
                if (LevelManager.multiplayer)
                {
                    if (PlayerController.GetLife() <= 0)
                    {
                        SoundManager.instance.musicSource.Stop();
                        gameOverScreen.SetActive(true);   // Load GameOver Scene
                        HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, playerName);  // adds Player Score on death with name set in main menu to the highscore board  
                        Debug.Log("Score saved!");
                    }
                }
            }

            Debug.Log("Explosion Hit");
        }
    }

    void ShowFloatingText()
    {
        var fText = Instantiate(floatingTextPrefab, new Vector3(transform.position.x, transform.position.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y , transform.position.z), Quaternion.identity,transform);  // Display Text Mesh 1 unit above Player Position

        fText.GetComponent<TextMesh>().text = life.ToString();
    }
    


    public class FiringMode
    {
        readonly String[] possibleFiringModes = { "MULTISHOT", "SINGLESHOT" };
        String firingMode;

        public FiringMode(String firingMode)
        {
            this.firingMode = firingMode;
        }
        private void SetFiringMode(String firingMode)
        {
            bool correct = false;
            foreach(String temp in possibleFiringModes)
            {
                if(temp.Equals(firingMode))
                {
                    correct = true;
                }
            }

            if (correct)
            {
                this.firingMode = firingMode;
            }
            else
            {
                Debug.Log("Firing Mode not valid!");
            }
        }

        private String GetFiringMode()
        {
            return this.firingMode;
        }

        public void SetMultishot()
        {
            this.firingMode = "MULTISHOT";
            

        }

        public void SetSingleshot()
        {
            this.firingMode = "SINGLESHOT";
        }

        public bool IsMultishot()
        {
            if(this.firingMode.Equals("MULTISHOT"))
            {
                return true;
            }
            return false;
        }

        public bool IsSingleshot()
        {
            if (this.firingMode.Equals("SINGLESHOT"))
            {
                return true;
            }
            return false;
        }


    }

}

