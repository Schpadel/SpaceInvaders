using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnClick : MonoBehaviour
{
    public AudioClip buttonSound;
    public void ExitGame()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        Application.Quit();
    }
}
