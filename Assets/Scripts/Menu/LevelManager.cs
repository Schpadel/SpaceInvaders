using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static bool multiplayer = false; 
    public static int level = 0; // first level = 0 second level = 1 third level(boss) = 2


    public static void LoadNextLevel()
    {
        if (level < 2)  // adjust to nummber of created levels
        {
 
            level++;
            SceneManager.LoadScene("SpaceInvaders" + level.ToString());  // SpaceInvaders + int to load next level
            

        }
        else
        {
            SceneManager.LoadScene("SpaceInvaders");
            level = -1; // beginn from the beginning
        }
    }

    public static void LoadSpecificLevel(int levelNummber)
    {
        SceneManager.LoadScene("SpaceInvaders" + levelNummber.ToString());
    }

}
