using UnityEngine;
using System.Collections;

public class CButtonSinglePlay : CBaseButtonClick {

    public override void OnClicked()
    {
        CScreenManager.Instance.ShowScreenByType(eScreenType.GAME_PLAY);
    }
}
