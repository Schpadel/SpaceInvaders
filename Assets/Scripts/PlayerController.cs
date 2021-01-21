using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public GameObject player;


    public static String playerName;
    public static float speed = 5f;
    public GameObject bullet;
    public GameObject multishot;
    private GameObject bulletHit;
    public static float fireRate = 0.5f;
    private float nextFire;
    private static int life = 5;
    public static int maxlife = 10;
    public static int lifeAtStart = 5;
    public static Text lifeText;
    public Text lifeTextPlayer2;
    public float dashDistance;
    private float nextDash;
    public static float dashRate = 5.0f;
    public GameObject gameOverScreen;
    public static FiringMode PlayerFiringMode = new FiringMode("SINGLESHOT"); // Firing mode is being set by the shop if item is bought
    public GameObject floatingTextPrefab;
    public GameObject pauseScreen;
    public GameObject player2;
    public AudioClip shootSound;
    public AudioClip playerDieSound;
    public AudioClip multiShootSound;
    private float input;
    private float input2;
    public KeyCode dashKey = KeyCode.LeftShift;

    // Start is called before the first frame update
    void Start()
    {
        // PlayerFiringMode.SetMultishot();//FOR TESTING REASONS

        player = this.gameObject;
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        UpdateHealthText();

        lifeTextPlayer2 = GameObject.Find("LifeTextPlayer2").GetComponent<Text>();
        if (!LevelManager.multiplayer) // hide live text of second player if game mode is not set to multiplayer
        {
            lifeTextPlayer2.gameObject.SetActive(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.multiplayer && player2.gameObject != null)
        {
            player2.gameObject.SetActive(true);
        }

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


        

        

        input = Input.GetAxisRaw("Horizontal");
        input2 = Input.GetAxisRaw("Vertical");
        player.transform.position += Vector3.right * input * speed * Time.deltaTime;
        player.transform.position += Vector3.up * input2 * speed * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            Invoke("SpawnBullet", 0);
            nextFire = Time.time + fireRate;
        }

        ExecuteDash(dashKey);


    }

    private void ExecuteDash(KeyCode dashKey)
    {

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.left * dashDistance;
            Debug.Log(player.transform.position);
            nextDash = Time.time + dashRate;
            //  Debug.Log("LShift and A pressed");

        }

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.right * dashDistance;
            nextDash = Time.time + dashRate;
        }

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && Time.time > nextDash)
        {
            player.transform.position += Vector3.up * dashDistance;
            Debug.Log(player.transform.position);
            nextDash = Time.time + dashRate;

        }

        if (Input.GetKeyDown(dashKey) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Time.time > nextDash)
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
                Instantiate(bullet, new Vector2(player.transform.position.x + 1.0f, player.transform.position.y), rotation); //fine tuning, start from first level else level manger != 2
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
                Instantiate(multishot, new Vector2(player.transform.position.x + 1.0f, player.transform.position.y), rotation);
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
            lifeText.text = "Lifes: " + life.ToString();
        }
        catch(NullReferenceException)
        {
            Debug.Log("Player Life Text not assigned!");
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
                // Multiplayer Death Handling
                if (LevelManager.multiplayer)
                {
                    if (Player2Controller.GetLife() <= 0)
                    {
                        SoundManager.instance.musicSource.Stop();
                        gameOverScreen.SetActive(true);   // Load GameOver Scene
                        HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, playerName);  // adds Player Score on death with name set in main menu to the highscore board 
                        Debug.Log("Score saved!");
                    }
                    Destroy(player);

                }
                else
                {
                    //Singleplayer Death Handling
                    SoundManager.instance.musicSource.Stop();
                    gameOverScreen.SetActive(true);   // Load GameOver Scene
                    HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, playerName);  // adds Player Score on death with name set in main menu to the highscore board 
                    Debug.Log("Score saved!");
                    Destroy(player);
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

                if (LevelManager.multiplayer)
                {
                    //Multiplayer Death Handling
                    if (Player2Controller.GetLife() <= 0)
                    {

                        SoundManager.instance.musicSource.Stop();
                        gameOverScreen.SetActive(true);   // Load GameOver Scene
                        HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, playerName);  // adds Player Score on death with name set in main menu to the highscore board 
                        Debug.Log("Score saved!");
                    }
                    Destroy(player);
                }
                else
                {
                    //Single Player Death Handling
                    SoundManager.instance.musicSource.Stop();
                    gameOverScreen.SetActive(true);   // Load GameOver Scene
                    HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, playerName);  // adds Player Score on death with name set in main menu to the highscore board 
                    Debug.Log("Score saved!");
                    Destroy(player);
                }


            }

            Debug.Log("Explosion Hit");
        }
    }

    void ShowFloatingText()
    {
        var fText = Instantiate(floatingTextPrefab, new Vector3(transform.position.x, transform.position.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y, transform.position.z), Quaternion.identity, transform);  // Display Text Mesh 1 unit above Player Position

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
            foreach (String temp in possibleFiringModes)
            {
                if (temp.Equals(firingMode))
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
            if (this.firingMode.Equals("MULTISHOT"))
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

