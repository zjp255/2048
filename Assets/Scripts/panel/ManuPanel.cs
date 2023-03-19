using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuPanel : MonoBehaviour
{

    private void Awake()
    {
        GameObject[] obj = new GameObject[2];
        obj = GameObject.FindGameObjectsWithTag("Audio");
        if (obj.Length >= 2)
        {
            GameObject.Destroy(obj[1]);
        }
    }
    //}
    public void ExitGame()
    {
        Application.Quit();
    }

}
