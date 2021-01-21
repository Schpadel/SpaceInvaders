using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class LevelGenerator : MonoBehaviour
{
    static int enemyAmount;
    const int minX = -13;
    const int maxX = 13;
    const int minY = -5;
    const int maxY = 3;
    static GameObject enemyPrefab;

    public void Start()
    {
        if(SceneManager.GetActiveScene().name.Equals("SpaceInvadersEndless"))
        {
            LevelGenerator.GenerateNewLevel(35);
        }
    }

    public static void GenerateNewLevel(int maxEnemy)
    {
        enemyAmount = Random.Range(1, maxEnemy);
        for (int i = 0; i < enemyAmount; i++)
        {
            GenerateEnemyPosition();
        }

    }

    private static void GenerateEnemyPosition()
    {
        int random = Random.Range(0, 2);

        Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));

        if (random == 0)
        {
            Instantiate(Resources.Load("Prefabs/GunAlien", typeof(GameObject)), position, Quaternion.identity, GameObject.Find("Enemies").transform);
        }

        if(random == 1)
        {
            Instantiate(Resources.Load("Prefabs/BlobAlien", typeof(GameObject)), position, Quaternion.identity, GameObject.Find("Enemies").transform);
        }
        

        
    }

}
