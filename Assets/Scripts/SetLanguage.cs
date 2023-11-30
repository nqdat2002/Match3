using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLanguage : MonoBehaviour
{
    //public Text replay;
    //public Text done;
    //public Text lose;
    //public Text resume;
    //public Text replay2;
    //public Text settings;
    //public Text menu;
    //public Text music;
    //public Text sound;


    //string[] Turkish = { "tekrar", "ileri", "tekrar dene", "DEVAM", "TEKRAR", "AYARLAR", "MENU", "Müzik", "Ses" };
    //string[] English = { "replay", "next", "try again", "RESUME", "REPLAY", "SETTINGS", "MENU", "Music", "Sound" };

    //// Start is called before the first frame update

    //void Start()
    //{

    //}
    //// Update is called once per fram

    //void Update()
    //{
    //    if (Dropdown.eng)
    //    {
    //        replay.text = English[0];
    //        done.text = English[1];
    //        lose.text = English[2];
    //        resume.text = English[3];
    //        replay2.text = English[4];
    //        settings.text = English[5];
    //        menu.text = English[6];
    //        music.text = English[7];
    //        sound.text = English[8];
    //    }
    //    else
    //    {
    //        replay.text = Turkish[0];
    //        done.text = Turkish[1];
    //        lose.text = Turkish[2];
    //        resume.text = Turkish[3];
    //        replay2.text = Turkish[4];
    //        settings.text = Turkish[5];
    //        menu.text = Turkish[6];
    //        music.text = Turkish[7];
    //        sound.text = Turkish[8];
    //    }
    //}


    public Text settingsMusic;
    public Text settingsSound;
    public Text settingsReset;

    public Text menuPlay;
    public Text menuInfinity;
    public Text menuSettings;
    public Text menuExit;

    public Text levelText1;
    public Text levelText2;


    // SetLanguage 2


    string[] Turkish = { "Müzik", "Ses", "Sıfırla", "OYNA", "SONSUZ MOD",
        "AYARLAR", "ÇIKIŞ" , "DAHA FAZLA LEVEL GÖRMEK İÇİN OYNAMAYA DEVAM ET"};
    string[] English = { "Music", "Sound", "Reset", "PLAY", "INFINITY MODE",
        "SETTINGS", "EXIT", "TO SEE MORE LEVELS KEEP PLAYING"};



    private void Update()
    {
        if (!Dropdown.eng)
        {
            settingsMusic.text = Turkish[0];
            settingsSound.text = Turkish[1];
            settingsReset.text = Turkish[2];
            menuPlay.text = Turkish[3];
            menuInfinity.text = Turkish[4];
            menuSettings.text = Turkish[5];
            menuExit.text = Turkish[6];
            levelText1.text = Turkish[7];
            levelText2.text = Turkish[7];
        }
        else
        {
            settingsMusic.text = English[0];
            settingsSound.text = English[1];
            settingsReset.text = English[2];
            menuPlay.text = English[3];
            menuInfinity.text = English[4];
            menuSettings.text = English[5];
            menuExit.text = English[6];
            levelText1.text = English[7];
            levelText2.text = English[7];
        }
    }
}
