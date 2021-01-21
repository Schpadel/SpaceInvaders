using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextDecay : MonoBehaviour
{

    public float DestroyTime = 3.0f;
    public Vector3 Offset = new Vector3(0, 1, 0);


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);
        transform.localPosition += Offset; 
    }

    public void DestroyOldText()
    {
        Destroy(gameObject);  // gameObject Achtung klein geschrieben gibt Zugriff auf das GameObject an das das Script angehängt ist, kommt von der Vererbung von MonoBehaviour
    }

}
