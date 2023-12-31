using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // Start is called before the first frame update
    private static Audio instance = null;

    public AudioSource blup;
    public static bool IsBlup;

    public static Audio Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
            
    }

    void Update()
    {
        if (IsBlup)
        {
            blup.Play();
            IsBlup = false;
        }
    }
}
