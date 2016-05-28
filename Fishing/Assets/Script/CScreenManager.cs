﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eScreenType
{
    MENU = 1,
    GAME_PLAY =2,
    GAME_FINISH = 3
}

[System.Serializable]
public class ScreenConfig
{
    public GameObject _objScreen;
    public eScreenType _type;
}
public class CScreenManager : MonoSingleton<CScreenManager> {
    public List<ScreenConfig> listScreen = new List<ScreenConfig>();
    private Dictionary<eScreenType, GameObject> dicScreen = new Dictionary<eScreenType, GameObject>();

    public GameObject currScreen;
    void Awake()
    {
        InitDictionary();
    }
	// Use this for initialization
	void Start () {
        ShowScreenByType(eScreenType.MENU);
	}
    public void InitDictionary()
    {
        foreach(ScreenConfig var in listScreen)
        {
            dicScreen.Add(var._type, var._objScreen);
        }
    }
	
    private GameObject GetScreenByType(eScreenType type)
    {
        if(dicScreen.ContainsKey(type))
        {
            return dicScreen[type];
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("ko co loai man hinh nay");
#endif
            return null;
        }
    }

    public void ShowScreenByType(eScreenType type)
    {
        DisableAllScreen();
        currScreen = GetScreenByType(type);
        if(currScreen)
        {
            currScreen.SetActive(true);
        }
    }

    private void DisableAllScreen()
    {
        foreach(ScreenConfig var in listScreen)
        {
            var._objScreen.SetActive(false);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
