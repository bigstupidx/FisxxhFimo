using UnityEngine;
using System.Collections;

public class Login : MonoSingleton<Login> {
    public GameObject splashImage;
    void Awake()
    {
        if (PhotonNetwork.connected)
        {
            //splashImage.SetActive(false);
            //JoinRandome();
            Invoke("JoinRandome", 1.0f);
        }
        else {
            Connect();
        }
        
    }

    public void Connect()
    {
        Debug.Log("Connect");
        //This tells Photon to make sure all players - which are in the same room - always load the same scene
        PhotonNetwork.automaticallySyncScene = true;

        //Don't join the default lobby on start, we do this ourselves in OnConnectedToMaster()
        PhotonNetwork.autoJoinLobby = true;

        if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
        {
            // Connect to the photon master-server.
            // We use the setting saved in PhotonServerSettings
            // (a .asset in this project
            PhotonNetwork.ConnectUsingSettings("Fish");
        }
        PhotonNetwork.playerName = SystemInfo.deviceUniqueIdentifier + "*" + SystemInfo.deviceUniqueIdentifier;//"Hoang";
    }

    public void JoinRandome()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.playerName + UnityEngine.Random.Range(1, 9999), new RoomOptions() { maxPlayers = GameServer.MaxPlayer }, null);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #region Event

    public void OnConnectedToPhoton()
    {
        Debug.Log("Connected");
        //JoinRandome();
        Invoke("JoinRandome", 1.5f);
        //splashImage.SetActive(false);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel("MultiPlay");
        
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    void OnLeftRoom()
    {
        Debug.Log("On Left Room");
        //SceneControl.Instance.LoadScene(SceneControl.SCENE_START);
    }

    public void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
    }

    public void OnPhotonJoinRoomFailed(object[] cause)
    {
        Debug.Log("OnPhotonJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
    }

    public void OnPhotonRandomJoinFailed()
    {
        CreateRoom();
        Debug.Log("OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms.");
    }

    #endregion
}
