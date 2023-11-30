using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject levelMenu;
    public GameObject settings;
    public GameObject menu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Play()
    {
        levelMenu.SetActive(true);
        menu.SetActive(false);
        settings.SetActive(false);
    }
    public void Infinite()
    {
        SceneManager.LoadScene("MainEvent");
    }
    public void Settings()
    {
        settings.SetActive(true);
        menu.SetActive(false);
        levelMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
