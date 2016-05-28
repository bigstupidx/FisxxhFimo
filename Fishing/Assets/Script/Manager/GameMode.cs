using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameModeConfig
{
    GAME_OFFLINE = 1,
    GAME_MULTI_VS = 2,
    GAME_MULTI_COOP = 3
}

public class GameMode : MonoBehaviour
{
    //public GameState gameState;
    public GameObject prefabBoat;   

    const float TIME_UPDATE_FISH = 0.1f;
    public float timeGame;

    public int maxFishCount;
    public Level levelInfo;

    public Dictionary<int, FishInfo> fishInfosDic;
    public Dictionary<int, GameObject> fishsDic;

    public Transform fishContainer;
    public BoatManager[] listPlayer;

    //public bool isUpdatedData = false;//for multi mode

    public virtual void StartGame()
    {
        maxFishCount = 15;
        //this.levelInfo = levelInfo;
        fishInfosDic = new Dictionary<int, FishInfo>();
        fishsDic = new Dictionary<int, GameObject>();
        timeGame = 60;
        //fishContainer = GameManager.Instance.FishContain;

        //prefabBoat = GameManager.Instance.prefabBoat;
        CreateBoatPlayer();
    }

    public virtual void TestToWaiting()
    {
    }

    public virtual void CreateBoatPlayer()
    {

    }

    public virtual void ResetGameWaiting() { }

    public virtual void UpdateGame(float timeUpdate)
    {
        timeGame -= timeUpdate;
        //Debug.Log("Time Game = " + timeGame);
        if (timeGame <= 0)
        {
            EndGame();
        }
    }

    public virtual void EndGame()
    {
        Debug.Log("End Game");
        //if (this.gameState != GameState.GAME_RUN)
        //{
        //    return;
        //}

    }

    public virtual void AddFishInfo(FishInfo fishInfo)
    {

    }


    public void CheckFish()
    {
        //Debug.Log("Count of fish = " + fishsDic.Count);
        //Fish = GameObject.Find("Fish").GetComponentsInChildren<Transform>();
        if (fishsDic.Count < maxFishCount)
        {
            FishInfo fishInfo = new FishInfo();
            fishInfo.Id = Random.Range(1, 9999);
            fishInfo.IdFish = Random.Range(0, levelInfo.FishList.Count);
            fishInfo.Status = 0;
            fishInfo.NameFish = levelInfo.FishList[fishInfo.IdFish];
            fishInfo.IdPath = RandomePath(fishInfo.NameFish);
            //GameManagerNew.Instance.FishObjs[fishInfo.NameFish]
            CreateFish(fishInfo);
        }
    }

    public virtual void CreateFish(FishInfo fishInfo)
    {

    }

    public virtual void RemoveFish(FishInfo fishInfo, GameObject fishObj = null)
    {

    }

    int RandomePath(string name)//GameObject fish
    {
        //return Random.Range(700, 899);
        if (name == "Crab" || name == "Hydro")
        {
            return Random.Range(500, 699);
        }
        else if (name == "ReaperFish" || name == "Piranha" || name == "GreatWhiteShark" || name == "HammerHeadShark")
        {
            return Random.Range(700, 899);
        }
        else
        {
            return Random.Range(0, 499);
        }
    }

    public virtual void CheckGoal()
    {

    }
}
