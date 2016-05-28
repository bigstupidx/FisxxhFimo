using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMulti : GameMode {

    public GameObject prefabBoatMulti;

    #region Method override from GameMode
    public override void StartGame()
    {
        this.prefabBoatMulti = GameManager.Instance.prefabBoatMulti;
        base.StartGame();

        GameManager.Instance.trashSpawn.isSpawn = false;
        //Handle Multi
        if (PhotonNetwork.isMasterClient)
        {
            gameState = GameState.GAME_WAITING;
            this.timeGame = GameServer.Time_Wating;
            SyncGameStateForServer();

            ExitGames.Client.Photon.Hashtable roomProp = new ExitGames.Client.Photon.Hashtable();// { { "spots", statusSpots } };
            roomProp.Add(RoomProperties.Spots, GameManager.Instance.listSpots);
            roomProp.Add(RoomProperties.GameStage, gameState);
            roomProp.Add(RoomProperties.MaxFish, this.maxFishCount);

            PhotonNetwork.room.SetCustomProperties(roomProp);

            GameManager.Instance.isUpdateData = true;
        }
        else
        {
            GameManager.Instance.isUpdateData = true;
            GameManager.Instance.listSpots = (int[])PhotonNetwork.room.customProperties[RoomProperties.Spots];
            gameState = (GameState)PhotonNetwork.room.customProperties[RoomProperties.GameStage];
            int level = (int)PhotonNetwork.room.customProperties[RoomProperties.Room_Level];
            Debug.Log("Level = " + level);
            //GameManager.Instance.LoadUIAndBackground(level);
        }

    }

    public override void UpdateGame(float timeUpdate)
    {
        //base.UpdateGame (timeUpdate);
        if (PhotonNetwork.isMasterClient)
        {
            CheckFish();
        }
        switch (gameState)
        {
            case GameState.GAME_READY:
                ReadyGame();
                break;
            case GameState.GAME_WAITING:
                timeGame -= timeUpdate;
                if (timeGame <= 0)
                {
                    ChangeStateStartGame();
                }
                //Create Fish
                WaitingGame(timeUpdate);
                break;
            case GameState.GAME_START:
                //Start Game
                //StartGame();
                PrepareRunGame();
                break;
            case GameState.GAME_RUN:
                timeGame -= timeUpdate;
                if (timeGame <= 0)
                {
                    EndGame();
                    //ChangeStateStartGame();
                }
                break;
            case GameState.GAME_FINISH:
                break;
        }
    }

    [ContextMenu("Test Start")]
    public override void TestToWaiting()
    {
        this.gameState = GameState.GAME_WAITING;
    }

    #region Fish
    public override void RemoveFish(FishInfo fishInfo, GameObject fishObj = null)
    {
        //base.RemoveFish(fishInfo, fishObj);
        if (fishObj != null && PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(fishObj);
        }

        fishsDic.Remove(fishInfo.Id);
    }

    public override void CreateBoatPlayer()
    {
        //base.CreateBoatPlayer();
        GameObject gameObj = PhotonNetwork.Instantiate(this.prefabBoatMulti.name, Vector3.zero, Quaternion.identity, 0) as GameObject;

        SwipeDetector.Instance.boat = gameObj.GetComponent<BoatMulti>();
    }

    public override void CreateFish(FishInfo fishInfo)
    {
        //base.CreateFish(fishInfo);
        if (!this.fishsDic.ContainsKey(fishInfo.Id))
        {
            GameObject fish = PhotonNetwork.Instantiate(fishInfo.NameFish, Vector3.zero, Quaternion.identity, 0) as GameObject;

            Move2 move2 = fish.GetComponent<Move2>();

            move2.FishInf = fishInfo;
            //move2.photonView.viewID = fishInfo.Id;
            move2.StartGame(gameState == GameState.GAME_RUN);

            this.fishsDic.Add(fishInfo.Id, fish);
        }
    }
    #endregion

    public override void EndGame()
    {
        base.EndGame();
        if (GameManager.Instance.gameModeConfig == GameModeConfig.GAME_MULTI_VS)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("End");
            }
        }
        //mmmm
        if (this.gameState == GameState.GAME_RUN && PhotonNetwork.isMasterClient)
        {
            //SceneControl.Instance.LoadScene(SceneControl.SCENE_END_MULTI);
            foreach (KeyValuePair<int, GameObject> entry in this.fishsDic)
            {
                PhotonNetwork.Destroy(entry.Value);
            }
        }

        PlayerPrefs.SetInt("fish", GameManager.Instance.fishCountCollect);
        PlayerPrefs.SetInt("trash", GameManager.Instance.trashCountCollect);
        PlayerPrefs.SetInt("Scene", (int)GameManager.Instance.gameModeConfig);

        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("End");

    }
    #endregion

    #region Method_IS_MINE
    void SyncGameStateForServer()
    {
        ExitGames.Client.Photon.Hashtable roomProp = new ExitGames.Client.Photon.Hashtable();// { { "spots", statusSpots } };
        roomProp.Add(RoomProperties.GameStage, gameState);
        PhotonNetwork.room.SetCustomProperties(roomProp);
    }


    void ReadyGame()
    {
    }

    public override void ResetGameWaiting()
    {
        timeGame = GameServer.Time_Wating;
        //base.ResetGameWaiting();
    }

    void WaitingGame(float timeUpdate)
    {

        if (PhotonNetwork.isMasterClient)
        {
            timeGame -= timeUpdate;
            if (timeGame < 0)
            {
                ChangeStateStartGame();
            }
            //if (PhotonNetwork.playerList.Length == GameServer.MaxPlayer)//
            //{
            //    ChangeStateStartGame();
            //    //Invoke("ChangeStateStartGame", 1.5f);
            //}
        }
    }

    void PrepareRunGame()
    {

        GameManager.Instance.trashSpawn.isSpawn = true;

        //this.StartGame();
        this.timeGame = 60;
        foreach (var entry in this.fishsDic)
        {
            GameObject fishObj = entry.Value;
            fishObj.GetComponent<Move2>().StartGame(true);//start_game = true;
        }
        PhotonNetwork.room.open = false;
        GameManager.Instance.DisableUIMulti();
        ChangeStateRunGame();
        //Invoke("ChangeStateRunGame", 2.5f);
    }

    void ChangeStateStartGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            gameState = GameState.GAME_START;
            SyncGameStateForServer();
        }
    }

    void ChangeStateRunGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            gameState = GameState.GAME_RUN;
            SyncGameStateForServer();
        }

    }
    #endregion
}
