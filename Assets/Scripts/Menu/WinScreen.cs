using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    
    public static GameObject shopScreen; // always the same shop, if different shops are implemented remove static
    public AudioClip buttonSound;
    public static bool gamePaused = false;

    public void Start()
    {
        // Trash Lösung, bad performance nach besserer Lösung suchen ... lädt alle Objekte in der Szene und sucht nach Shop Canvas, es kann nicht GameObject.Find() verwendet werden, da das Objekt zu Beginn der Szene inaktiv ist
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject temp in allObjects)
        {
            if(temp.name.Equals("Shop Canvas"))
            {
                shopScreen = temp;
            }
        }

        
    }
    public void OnQuitButtonClick()
    {

        SoundManager.instance.PlaySingle(buttonSound);
        SceneManager.LoadScene("MainMenu");
        if (PlayerController.GetLife() > 0  || Player2Controller.GetLife() > 0) // Player still alive but quits the game, so score should be safed to scoreboard!
        {
            HighscoreTable.AddHighscoreEntry(EnemyHitDetection.scoreInt, PlayerController.playerName);
        }

        Player2Controller.ResetPlayerUpgrades();
        PlayerController.ResetPlayerUpgrades();
        EnemyHitDetection.scoreInt = 0;
        EnemyHitDetection.moneyInt = 0;
        Shop.moneyAmount = 0;
        Shop.priceMultishot = 1;
        Shop.priceHeal = 5;
        Shop.priceSingleshot = 10;
        Shop.priceFireRateIncrease = 10;
        Shop.priceSpeedIncrease = 5;
        LevelManager.level = -1;
        
        if(LevelManager.multiplayer)
        {
            LevelManager.multiplayer = false;
            Destroy(GameObject.Find("Player2(Clone)"));
        }


    }

    public void OnNextLevelButtonClick() // can not be static because unity buttons can not invoke static methods!
    {
        LevelManager.LoadNextLevel();
        SoundManager.instance.PlaySingle(buttonSound);
    }

    public void OnRetryButtonClick()
    {
        LevelManager.level = 0;
        LevelManager.LoadSpecificLevel(0);
        EnemyHitDetection.scoreInt = 0;
        PlayerController.ResetPlayerUpgrades(); // Player has lost and looses all his upgrades and his life gets set back to the original amount
        Player2Controller.ResetPlayerUpgrades();
        SoundManager.instance.PlaySingle(buttonSound);
        Shop.moneyAmount = 0;
        Shop.priceMultishot = 1;
        Shop.priceHeal = 5;
        Shop.priceSingleshot = 10;
        Shop.priceFireRateIncrease = 10;
        Shop.priceSpeedIncrease = 5;
        if (LevelManager.multiplayer)
        {
            Destroy(GameObject.Find("Player2(Clone)"));
        }

    }

    public void OnShopButtonClick()
    {
        shopScreen.SetActive(true);
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySingle(buttonSound);
        SoundManager.instance.shopMusicSource.Play();
    }

    public void OnContinueButtonClick()
    {
        gamePaused = false; // activate Update Methods in other scripts
        gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    public void OnRestartPlusClick()
    {
        LevelManager.level = 0;
        LevelManager.LoadSpecificLevel(0);
        SoundManager.instance.PlaySingle(buttonSound);
        EnemyHitDetection.difficulty++;
    }
}
