using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update

    public static Text money;
    public static int moneyAmount;
    public static int priceMultishot = 1; // billig zum debuggen 
    public static int priceHeal = 5;
    public static int priceSingleshot = 10;
    public static int priceFireRateIncrease = 10;
    public static int priceSpeedIncrease = 5;
    public static GameObject noMoney;

    public AudioClip buySound;
    public AudioClip buttonSound;




    private void Start()
    {
        money = GameObject.Find("Money").GetComponent<Text>();
        noMoney = GameObject.Find("NoMoneyControl");
    }

    // Update is called once per frame
    void Update()
    {
        moneyAmount = EnemyHitDetection.moneyInt;

    }


    public void OnBuyItem(Item item)
    {
        if (item.CanBuy(moneyAmount))
        {
            Debug.Log("Item bought!");
            EnemyHitDetection.moneyInt = moneyAmount - item.GetPrice();
            item.SetEffect();
            SoundManager.instance.PlaySingle(buySound);
            money.text = "Money: " + EnemyHitDetection.moneyInt.ToString(); // update  money text  after succesfull buy
        }
        else
        {
            noMoney.SetActive(true);
        }
    }

    public void OnContinueClick()
    {
        int temp = PlayerController.GetLife(); // no variable persistence when loading new scene
        LevelManager.LoadNextLevel();
        PlayerController.SetLife(temp);
        GameObject.Find("Shop Canvas").SetActive(false);
        PlayerController.UpdateHealthText();


        SoundManager.instance.shopMusicSource.Stop();
        SoundManager.instance.PlaySingle(buttonSound);
        SoundManager.instance.musicSource.Play();

        // loaded next Level

    }

    public void OnHealClick()
    {
        Item Heal = new Item("HEAL", priceHeal);
        if (PlayerController.GetLife() < PlayerController.maxlife && Heal.CanBuy(moneyAmount))
        {
            OnBuyItem(Heal);
            priceHeal *= 2;
        }

            PlayerController.UpdateHealthText();
    } 

    public void OnMultishotClick()
    {
        Item Multishot = new Item("MULTISHOT", priceMultishot);

        if (Multishot.CanBuy(moneyAmount))
        {
            OnBuyItem(Multishot);
            priceMultishot *= 2;
        }
    }

    public void OnSingleshotClick()
    {
        Item Singleshot = new Item("SINGLESHOT", priceSingleshot);
        OnBuyItem(Singleshot);
    }

    public void OnIncreaseFireRateClick()
    {
        Item IncreaseFireRate = new Item("FIRERATE", priceFireRateIncrease);

        if (IncreaseFireRate.CanBuy(moneyAmount))
        {
            OnBuyItem(IncreaseFireRate);
            priceFireRateIncrease *= 2;
        }

    }

    public void OnIncreaseSpeedClick()
    {
        Item IncreaseSpeed = new Item("SPEED", priceSpeedIncrease);

        if (IncreaseSpeed.CanBuy(moneyAmount))
        {
            OnBuyItem(IncreaseSpeed);
            priceSpeedIncrease *= 2;
        }
        
    }

    public class Item
    {
        private readonly string name;
        private readonly int value;

        public Item(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public int GetPrice()
        {
            return this.value;
        }
        public bool CanBuy(int money)
        {
            if (this.value <= money)
            {
                return true;
            }

            Debug.Log("Not enough Money!");
            return false;
        }

        public void SetEffect()
        {
            if (this.name.Equals("MULTISHOT"))
            {
                if(LevelManager.multiplayer)
                {
                    Player2Controller.PlayerFiringMode.SetMultishot();
                }
                PlayerController.PlayerFiringMode.SetMultishot();
            }

            if (this.name.Equals("SINGLESHOT"))
            {
                if (LevelManager.multiplayer)
                {
                    Player2Controller.PlayerFiringMode.SetSingleshot();
                }
                PlayerController.PlayerFiringMode.SetSingleshot();
            }

            if (this.name.Equals("HEAL"))
            {
                if (LevelManager.multiplayer)
                {
                    if (Player2Controller.GetLife() < Player2Controller.maxlife)
                    {
                        Player2Controller.SetLife(Player2Controller.GetLife() + 1);
                    }
                }
                if (PlayerController.GetLife() < PlayerController.maxlife) 
                { 
                    PlayerController.SetLife(PlayerController.GetLife() + 1);
                }
            }

            if (this.name.Equals("FIRERATE"))
            {
                if(LevelManager.multiplayer)
                {
                    Player2Controller.fireRate -= 0.2f;
                    if (Player2Controller.fireRate < 0) // results in way too strong fire rates, maybe percent upgrades instead of fixed upgrades ? 
                    {
                        Player2Controller.fireRate = 0;
                        Debug.Log("Fire Rate max Upgrade reached!");
                    }
                }
                PlayerController.fireRate -= 0.2f;
                if (PlayerController.fireRate < 0) // results in way too strong fire rates, maybe percent upgrades instead of fixed upgrades ? 
                {
                    PlayerController.fireRate = 0;
                    Debug.Log("Fire Rate max Upgrade reached!");
                }
            }
            if (this.name.Equals("SPEED"))
            {
                if(LevelManager.multiplayer)
                {
                    Player2Controller.IncreaseSpeed();
                    if(Player2Controller.speed >10f)
                    {
                        Player2Controller.SetSpeed(10f);
                    }
                }
                PlayerController.IncreaseSpeed();
                if (PlayerController.speed > 10f)
                {
                    PlayerController.SetSpeed(10f);
                    Debug.Log("Speed max Upgrade reached!");
                }
            }
        }

    }

}
