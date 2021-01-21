using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateShopUI : MonoBehaviour
{

    public Text money;
    public Text life;
    public Text lifePlayer2;
    public Text lifeCost;
    public Text multishotCost;
    public Text FireRateIncreaseCost;
    public Text SpeedIncreaseCost;


    // Update is called once per frame
    void Update()
    {
        money.text = "Money: " + EnemyHitDetection.moneyInt.ToString()+ " coins";
        life.text = "Lifes: " + PlayerController.GetLife();
        lifeCost.text = "Lifes Cost: " + Shop.priceHeal;
        multishotCost.text = "Multishot Cost: " + Shop.priceMultishot;
        FireRateIncreaseCost.text = "Fire Rate + Cost: " + Shop.priceFireRateIncrease;
        SpeedIncreaseCost.text = "Movementspeed + Cost: " + Shop.priceSpeedIncrease;

        if(LevelManager.multiplayer)
        {
            lifePlayer2.gameObject.SetActive(true);
            lifePlayer2.text = "Player2 Lifes: " + Player2Controller.GetLife();
        }

    }
}
