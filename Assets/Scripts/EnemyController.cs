using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private Transform enemy;
    public GameObject[] waypoints;
    public float enemySpeed;
    int currentWaypoint = 0;
    public float nearIt;
    public GameObject winScreen;
    public GameObject gameWinScreen;

    public AudioClip bossSound;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        SoundManager.instance.musicSource.Stop();
        if (LevelManager.level == 2)
        {

            SoundManager.instance.musicSource.PlayDelayed(2.56f);
            SoundManager.instance.PlaySingle(bossSound);
        }
        else
        {
            SoundManager.instance.musicSource.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (WinScreen.gamePaused)  // if game paused skip update method 
        {
            return;
        }

        if (Vector2.Distance(waypoints[currentWaypoint].transform.position,transform.position) < nearIt)
        {
            
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;  // start from the first Waypoint again !!
            }

        }   // else move towards Waypoint
        enemy.transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, Time.deltaTime * enemySpeed * EnemyHitDetection.difficulty);

           
       


        if (enemy.childCount == 0)
        {
            if (LevelManager.level == 2 && gameWinScreen != null)
            {
                gameWinScreen.SetActive(true);
            }
            else
            {
                Debug.Log(LevelManager.level);
                winScreen.SetActive(true);
            }
        }


    }

}


