using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonHome()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ButtonReplay()
    {
        GameModeConfig gameModeConfig = (GameModeConfig)PlayerPrefs.GetInt("Scene");
        if (gameModeConfig == GameModeConfig.GAME_MULTI_VS)
        {
            SceneManager.LoadScene("Main");
        }
        else {
            SceneManager.LoadScene("Login");
        }
    }
}
