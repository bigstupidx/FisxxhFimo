using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CButtonSinglePlay : CBaseButtonClick
{

    public override void OnClicked()
    {
        //CScreenManager.Instance.ShowScreenByType(eScreenType.GAME_PLAY);
        SceneManager.LoadScene("Main");
    }
}
