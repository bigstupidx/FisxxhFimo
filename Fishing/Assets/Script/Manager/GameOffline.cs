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
        GameObject fish = GameManager.Instance.CreateFishByNameOffline(fishInfo.IdFish);

        fish.transform.SetParent(this.fishContainer);
        Move2 move2 = fish.GetComponent<Move2>();
        move2.FishInf = fishInfo;
        move2.start_game = true;
        fishsDic.Add(fishInfo.Id, fish);
    }

    public override void CreateBoatPlayer()
    {
        if (GameManager.Instance.prefabBoat != null)
        {
            GameObject boat = Instantiate(GameManager.Instance.prefabBoat, Vector3.zero, Quaternion.identity) as GameObject;
            boat.GetComponent<BoatManager>().ApplyRandomeSpot();
            SwipeDetector.Instance.boat = boat.GetComponent<BoatManager>();
        }
        //GameObject boat = GameManagerNew.Instance.CreateBoatPlayerOffline();
        
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

    public override void EndGame()
    {

    }

}
