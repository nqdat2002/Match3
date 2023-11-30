using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropdown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool eng = true;

    public void ChangeLanguage(int index)
    {
        if (index == 1 && eng)
        {
            eng = false;
            PlayerPrefs.SetInt("lang", 0);
        }
        else
        {
            eng = true;
            PlayerPrefs.SetInt("lang", 1);
        }
    }
}
