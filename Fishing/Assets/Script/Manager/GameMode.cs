using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    GAME_READY = 0,
    GAME_WAITING = 1,
    GAME_START = 2,
    GAME_RUN = 3,
    GAME_FINISH = 4
}

public enum GameModeConfig
{
    GAME_OFFLINE = 1,
    GAME_MULTI_VS = 2
}

public class GameMode : MonoBehaviour
{
    public GameState gameState;
    public GameObject prefabBoat;   

    const float TIME_UPDATE_FISH = 0.1f;
    public float timeGame;

    public int maxFishCount;

    public Dictionary<int, FishInfo> fishInfosDic;
    public Dictionary<int, GameObject> fishsDic;
    public Dictionary<int, GameObject> fishDies;

    public Dictionary<int, GameObject> trashsDic;
    public List<GameObject> trashs;

    public Transform fishContainer;
    public BoatManager[] listPlayer;

    public bool catchFish;

    //public bool isUpdatedData = false;//for multi mode

    public virtual void StartGame()
    {
        Debug.Log("Start game here aaa");
        maxFishCount = 20;
        //this.levelInfo = levelInfo;
        fishInfosDic = new Dictionary<int, FishInfo>();
        fishsDic = new Dictionary<int, GameObject>();

        trashs = new List<GameObject>();
        fishDies = new Dictionary<int, GameObject>();
        timeGame = 60;
        catchFish = false;

        GameManager.Instance.trashSpawn.isSpawn = true;
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
        if (timeGame <= 15)
        {
            catchFish = true;
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
        if (maxFishCount > 0)//fishsDic.Count < maxFishCount
        {
            maxFishCount--;

            FishInfo fishInfo = new FishInfo();
            fishInfo.Id = Random.Range(1, 9999);
            fishInfo.IdFish = Random.Range(0, GameManager.Instance.fishPrefabs.Count);
            fishInfo.Status = 0;
            fishInfo.NameFish = GameManager.Instance.fishPrefabs[fishInfo.IdFish].name;//levelInfo.FishList[fishInfo.IdFish];
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
        ////return Random.Range(700, 899);
        ////if (name == "Crab" || name == "Hydro")
        ////{
        ////    return Random.Range(500, 699);
        ////}
        ////else if (name == "ReaperFish" || name == "Piranha" || name == "GreatWhiteShark" || name == "HammerHeadShark")
        //{
        //    return Random.Range(700, 899);
        //}
        //else
        {
            return Random.Range(0, 499);
        }
    }

    public virtual void CheckGoal()
    {

    }

    public void AddFishFreezee(GameObject fish)
    {
        if (fishsDic.ContainsValue(fish))
        {
            //fish.GetComponent<Move2>().isFreeze = true;
            fish.GetComponent<Move2>().SetIsFreeze(true);
            fishDies.Add(fish.GetComponent<Move2>().FishInf.Id, fish);
            fishsDic.Remove(fish.GetComponent<Move2>().FishInf.Id);
        }
        //fishsDic.Add fish.GetComponent<Move2>().FishInf.Id
    }

    public void RemoveTrash(GameObject trash)
    {
        trashs.Remove(trash);

        int randFishHoiSinh = Random.Range(0, fishDies.Count);

        int i = 0;
        foreach (KeyValuePair<int, GameObject> pair in fishDies)
        {
            if (i == randFishHoiSinh)
            {
                Debug.Log("Hoi sinh fish");
                //fish Die
                GameObject fish = pair.Value;
                fish.GetComponent<Move2>().SetIsFreeze(false);//isFreeze = false;
                

                fishDies.Remove(pair.Key);

                fishsDic.Add(pair.Key, pair.Value);
                return;
            }
            else
            {
                i++;
            }
        }
    }

    public void AddTrash(GameObject trash)
    {
        trashs.Add(trash);

        //Remove 1 fish.
        int randFishDie = Random.Range(0, fishsDic.Count);

        int i = 0;
        foreach (KeyValuePair<int, GameObject> pair in fishsDic)
        {
            if (i == randFishDie)
            {
                //fish Die
                GameObject fish = pair.Value;

                fish.GetComponent<Move2>().SetIsFreeze(true);
                //fish.GetComponent<Move2>().isFreeze = true;
                fishsDic.Remove(pair.Key);

                fishDies.Add(pair.Key, pair.Value);
                return;
            }
            else {
                i++; 
            }
        }
    }

}
