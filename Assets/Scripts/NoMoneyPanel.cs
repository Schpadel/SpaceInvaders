using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoneyPanel : MonoBehaviour
{
    // Start is called before the first frame update

    
    public void OnOkayButtonClick()
    {
        Shop.noMoney.SetActive(false);
    }
}
