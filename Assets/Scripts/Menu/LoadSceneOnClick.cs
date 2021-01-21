using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnClick : MonoBehaviour
{
    public AudioClip buttonSound;
    public GameObject playerName;


    public void LoadSceneNormalDifficulty()
    {
        SceneManager.LoadScene("SpaceInvaders0");
        PlayerController.playerName = playerName.GetComponent<Text>().text.ToString();
        SoundManager.instance.PlaySingle(buttonSound);
        EnemyHitDetection.difficulty = 1;
        
    }

    public void LoadSceneHardDifficulty()
    {
        SceneManager.LoadScene("SpaceInvaders0");
        PlayerController.playerName = playerName.GetComponent<Text>().text.ToString();
        SoundManager.instance.PlaySingle(buttonSound);
        EnemyHitDetection.difficulty = 2;
        
    }

    public void LoadMultiplayer()
    {
        LevelManager.multiplayer = true;
        SceneManager.LoadScene("SpaceInvaders0");
        PlayerController.playerName = playerName.GetComponent<Text>().text.ToString();
        SoundManager.instance.PlaySingle(buttonSound);
        EnemyHitDetection.difficulty = 1;


    }
}
