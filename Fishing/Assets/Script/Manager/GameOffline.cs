using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOffline : GameMode
{

    public override void UpdateGame(float timeUpdate)
    {
        base.UpdateGame(timeUpdate);

        this.CheckFish();
    }

    public override void CreateFish(FishInfo fishInfo)
    {
        //base.CreateFish(fishInfo);
        GameObject fish = GameManager.Instance.CreateFishByNameOffline(fishInfo.NameFish);

        fish.transform.SetParent(this.fishContainer);
        Move2 move2 = fish.GetComponent<Move2>();
        move2.FishInf = fishInfo;
        move2.start_game = true;
        fishsDic.Add(fishInfo.Id, fish);
    }

    public override void CreateBoatPlayer()
    {
        //GameObject boat = GameManagerNew.Instance.CreateBoatPlayerOffline();
        GameObject boat = Instantiate(this.prefabBoat, Vector3.zero, Quaternion.identity) as GameObject;
        //boat.GetComponent<BoatManager>().ApplyRandomeSpot();
        SwipeDetector.Instance.boat = boat.GetComponent<BoatManager>();
        //base.CreateBoatPlayer();
    }

    public override void RemoveFish(FishInfo fishInfo, GameObject fishObj = null)
    {
        //base.RemoveFish(fishInfo);
        if (fishObj != null)
        {
            Destroy(fishObj);
        }

        fishsDic.Remove(fishInfo.Id);
    }

    //public override void EndGame()
    //{
    //    if (GameManager.Instance.Score >= levelInfo.GoldGoal &&
    //        GameManager.Instance.FishGoal >= levelInfo.NumberFishGoal)
    //    {
    //        PlayerPrefs.SetString("Background", (levelInfo.Lv + 1).ToString());
    //        if (levelInfo.Lv == PlayerPrefs.GetInt("UnlockedLevel"))
    //        {
    //            PlayerPrefs.SetInt("UnlockedLevel", levelInfo.Lv + 1);
    //        }
    //        PlayerPrefs.SetInt("PlayLevel", levelInfo.Lv + 1);
    //    }
    //    else
    //    {
    //        // PlayerPrefs.SetString("Background", (levelInfo.Lv).ToString());
    //    }
    //    //Debug.Log(PlayerPrefs.GetInt("Playlevel"));
    //    // PlayerPrefs.SetString("Background", (PlayerPrefs.GetInt("Playlevel") / 5).ToString());
    //    PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + GameManager.Instance.Score);
    //    foreach (KeyValuePair<int, GameObject> entry in this.fishsDic)
    //    {
    //        Destroy(entry.Value);
    //    }
    //}

}
