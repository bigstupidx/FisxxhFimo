using UnityEngine;
using System.Collections;

public class BoatMulti : BoatManager {

	// Use this for initialization
	void Start () {
        Debug.Log("Boat offfline");
        PreStart();
	}

    void Update()
    {
        Debug.Log("Update");
        if (!isMine)
        {
            Debug.Log("not is mine");
            return;
        }
        Swing();

    }

    public override void StartGame()
    {
        Debug.Log("Start Boat Multi");
        //base.StartGame();
        isOnline = true;
        CurrentSpot = -1;

        isMine = photonView.isMine;
    }
}
