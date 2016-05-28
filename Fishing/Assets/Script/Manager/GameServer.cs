using UnityEngine;
using System.Collections;

public class RoomProperties
{
    public const string Spots = "spots";
    public const string Fish = "Fish";
    public const string GameStage = "GameStage";
    public const string MaxFish = "MaxF";
    public const string Player_Ready = "PlayerReady";
    public const string Player_Index = "Index";
    public const string Room_Level = "Level";
}

public class GameServer : Photon.MonoBehaviour
{

    public static int CountPlayerReady;
    public const int MaxPlayer = 4;
    public const int Time_Wating = 10;
    public const int Time_Game = 60;
    // Use this for initializatio
    void Start()
    {

    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach (DictionaryEntry entry in propertiesThatChanged)
        {
            Debug.Log("Prop change key = " + entry.Key + "--- Value = " + entry.Value);
            Debug.Log("Type of = " + entry.Key.GetType());

            if (entry.Key.GetType() != typeof(System.String))
            {
                return;
            }
            string key = (string)entry.Key;

            //entry.Key
            switch (key)
            {
                case RoomProperties.GameStage:
                    GameState gameState = (GameState)entry.Value;
                    GameManager.Instance.SetGameState(gameState);
                    Debug.Log("Game State = " + gameState);
                    break;
                case RoomProperties.Spots:
                    int[] spots = (int[])entry.Value;
                    GameManager.Instance.listSpots = spots;
                    break;
                case RoomProperties.MaxFish:
                    int maxFish = (int)entry.Value;
                    break;
                default:
                    //Fish
                    if (key.Contains(RoomProperties.Fish))
                    {
                        FishInfo fishInfo = FishInfo.ToFishInfo((string)entry.Value);
                        //GameManager.Instance.AddFishInfo(fishInfo);
                    }
                    break;
            }
        }
    }

    void OnPhotonCustomPlayerPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesChanged)
    {

    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Has player connect = " + newPlayer.name);
        if (PhotonNetwork.isMasterClient)
        {
            //GameManager.Instance.ResetGameWaiting();
        }

    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Has player disconnect = " + otherPlayer.name);
        if (otherPlayer.customProperties.ContainsKey(RoomProperties.Player_Index))
        {

            int indexSpot = (int)otherPlayer.customProperties[RoomProperties.Player_Index];
            Debug.Log("Reset index " + indexSpot);
            GameManager.Instance.ResetSpot(indexSpot);
        }
    }
}

