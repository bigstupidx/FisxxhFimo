using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetUIEnd : MonoBehaviour
{
    public Text fish;
    public Text trash;
    public Text score;
	// Use this for initialization
	void Start ()
    {
	    SetUI();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetUI()
    {
        fish.text = PlayerPrefs.GetInt("fish").ToString() + " x 100";
        trash.text = PlayerPrefs.GetInt("trash").ToString() + " x 150";
        score.text = PlayerPrefs.GetInt("fish")*100 + PlayerPrefs.GetInt("trash")*100 + "$";
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
