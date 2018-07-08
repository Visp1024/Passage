using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 

public class StartGame : MonoBehaviour {

    public GameObject first_time;

    private void Start()
    {
        if (PlayerPrefs.GetInt("FT") != 1) //читает с рееcтра
        {
            PlayerPrefs.SetInt("FT", 1);
            first_time.SetActive(true);
        }
        //Screen.SetResolution(2560, 1080, true);
    }

    public void HotseatStart4()
    {
        GameLogic.kolvoPlayers = 4;
        bl_SceneLoaderUtils.GetLoader.LoadLevel("2");
        // SceneManager.LoadScene(2);
        Destroy(GameObject.Find("MusicMenu"));
    }

    public void HotseatStart2()
    {
        GameLogic.kolvoPlayers = 2;
        //SceneManager.LoadScene(2);
        bl_SceneLoaderUtils.GetLoader.LoadLevel("2");
        Destroy(GameObject.Find("MusicMenu"));
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene(2);       
    }

    public void LoadMainmenu()
    {
        SceneManager.LoadScene(0);
        Destroy(GameObject.Find("LobbyManager"));
    }

    public void ReloadScen()
    {
        SceneManager.LoadScene(1);
    }

}
