using UnityEngine;
using System.Collections;

public class BoatMulti : BoatManager {

    void Start()
    {
        Debug.Log("Boat multi");
        //PreStart();
        //photonView.owner.
        
        IsSwing = true;
        _spots = GameManager.Instance.Spots;
        CurrentSpot = -1;
        newSpot = -1;
        Score = 0;
        FishGoal = 0;

        

        StartGame();
        timeDelayMoveCur = 0;

        LoadBoatNPlayer();
    }
    float timeDelayMove = 0.4f;
    float timeDelayMoveCur = 0;
    void Update()
    {
        if (!isMine)
        {
            return;
        }

        Swing();

        timeDelayMoveCur += Time.deltaTime;
    }

    public override void MoveRight()
    {
        if (timeDelayMoveCur >= timeDelayMove)
        {
            base.MoveRight();
            timeDelayMoveCur = 0;
        }
    }

    public override void MoveLeft()
    {
        if (timeDelayMoveCur >= timeDelayMove)
        {
            base.MoveLeft();
            timeDelayMoveCur = 0;
        }
    }

    public override void StartGame()
    {
        Debug.Log("Start Boat Multi");
        //base.StartGame();
        isOnline = true;
        CurrentSpot = -1;

        isMine = photonView.isMine;
    }

    IEnumerator CheckUpdateData()
    {
        Debug.Log("Check data");
        while (!GameManager.Instance.isUpdateData)
        {
            //Debug.Log("Wait for update boat");
            yield return null;
            //Debug.Log("Updating boat");
        }
        //yield return new WaitForSeconds(0.5f);
        int index_Spot = GameManager.Instance.GetAvailableSpot();
        Debug.Log("Check Update data - with index = " + index_Spot);
        ApplyIndexSpot(index_Spot, true);//also Appl index to server 

        StopCoroutine("CheckUpdateData");
    }

    public override void SetIndexPlayerOnline()
    {
        //base.SetIndexPlayerOnline();
        ExitGames.Client.Photon.Hashtable playerProp = new ExitGames.Client.Photon.Hashtable();
        playerProp.Add(RoomProperties.Player_Index, this.CurrentSpot);
        PhotonNetwork.player.SetCustomProperties(playerProp);
    }

    [ContextMenu("move")]
    public override void Hook() //stop swing, hook move forward.
    {
        if (myHook.IsBack)
        {
            return;
        }
        photonView.RPC("OnHook", PhotonTargets.AllBufferedViaServer, new object[] { });
        Audio.Instance.Hook();
    }

    [PunRPC]
    void OnHook()
    {
        if (photonView.isMine)
        {

            myHook.IsMove = true;
            //Transform hook = SwingSpot.transform.GetChild(0);
            //hook.gameObject.GetComponent<Hook>().IsMove = true;
            hookObj.GetComponent<BoxCollider2D>().enabled = true;
            GetComponentInChildren<Animator>().SetBool("IsCatch", true);
            IsSwing = false;
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("IsCatch", true);
            IsSwing = false;
        }

    }

    [PunRPC]
    void OnApplySpot(int index, bool hasChange)
    {
        if (index != 0)
        {
            GameManager.Instance.SetSpotByIndex(index, CurrentSpot, transform, hasChange);
            transform.localPosition = Vector3.zero;
            CurrentSpot = index;
            //newSpot = index;
        }
    }

    public override void LoadBoatNPlayer()
    {
        Debug.Log("Load Boat Multi");
        Debug.Log("Photon ID = " + PhotonNetwork.player.ID + "  --- View ID = " + photonView.viewID);
        //base.LoadBoatNPlayer();
        if (photonView.isMine)
        {
            Debug.Log("Load boat with Mine");
            //this.LoadDataOrSyncBoat();
            //this.SetBoatNPlayer();
            StartCoroutine("CheckUpdateData");

        }
        else
        {
            Debug.Log("Load boat with not Mine");
            ExitGames.Client.Photon.Hashtable playerDataRev = photonView.owner.customProperties;//PhotonNetwork.player.customProperties;

            //PhotonNetwork.
            if (playerDataRev != null && playerDataRev.ContainsKey(RoomProperties.Player_Index) && playerDataRev.ContainsKey(RoomProperties.Player_Index))
            {
                int index = (int)playerDataRev[RoomProperties.Player_Index];
                ApplyIndexSpot(index, false);
                //Debug.Log("Load boat not mine: Boat name =  " + this.boat_name);
                //this.SetBoatNPlayer();
            }
            else
            {
                Debug.Log("Load boat not mine: Reload");
                Invoke("LoadBoatNPlayer", 0.5f);
            }
        }
    }
}
