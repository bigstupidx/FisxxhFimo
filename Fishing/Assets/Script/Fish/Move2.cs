using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class FishInfo
{
    public int Id;
    public int IdFish;
    public int IdPath;
    public string TimeStart;
    public int Status;
    public string NameFish;

    public static FishInfo ToFishInfo(string fish)
    {
        FishInfo temp = new FishInfo();
        string[] path = fish.Split(new string[] { "_" }, StringSplitOptions.None);
        temp.Id = int.Parse(path[0]);
        temp.IdFish = int.Parse(path[1]);
        temp.IdPath = int.Parse(path[2]);
        temp.TimeStart = path[3];
        temp.Status = int.Parse(path[4]);
        temp.NameFish = path[5];
        return temp;
    }

    public string FishInfoToString()
    {
        return Id + "_" + IdFish + "_" + IdPath + "_" + TimeStart + "_" + Status + "_" + NameFish;
    }
}

public class Move2 : Photon.MonoBehaviour
{
    public FishInfo FishInf;
    public Transform[] WayPoint;
    public float Speed = 8;
    public bool IsBtm;
    public bool IsPredator;

    Rigidbody2D _myRigidbody;
    Vector3[] _twoWayPoint;
    float _someScale;
    Vector2 _velocity;
    private int _currentWaypoint = 0;
    bool _loop = false;
    float _distance = 1;

    //[HideInInspector]
    public bool isCatch = false;
    public bool start_game;

    public bool isFreeze;

    public BoxCollider2D myBox;

    void Start()
    {
        myBox = GetComponent<BoxCollider2D>();
        isFreeze = false;
        isCatch = false;
        _twoWayPoint = DataLoader.Instance.ListPath[FishInf.IdPath];
        _myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _someScale = transform.localScale.x;
        transform.position = _twoWayPoint[_twoWayPoint.Length - 1];
    }

    void Update()
    {
        if (!start_game)
        {
            return;
        }
        if (isFreeze)
        {
            _myRigidbody.gravityScale = 1;
            myBox.isTrigger = false;
            return;
        }
        _myRigidbody.gravityScale = 0;
        myBox.isTrigger = true;
        if (_currentWaypoint < _twoWayPoint.Length)
        {
            Vector3 target = _twoWayPoint[_currentWaypoint];
            Vector3 moveDirection = target - transform.position;

            _velocity = _myRigidbody.velocity;
            if (moveDirection.magnitude < _distance)
            {
                _currentWaypoint++;
            }
            else
            {
                _velocity = moveDirection.normalized * Speed;
            }

        }

        else {
            if (_loop)
            {
                _currentWaypoint = 0;
            }
            else
            {
                //Hoang Commnet
                FishInf.Status = 1;

                //GameManagerNew.Instance.RemoveFishFromList(FishInf, this.gameObject); TODO: fix
                //PoolObject.Instance.DespawnFish(this.gameObject);
                _velocity = Vector3.zero;
                _currentWaypoint = 0;
            }
        }
        _myRigidbody.velocity = _velocity;
        CheckFLip();
    }

    public void StartGame(bool startGame)
    {
        this.start_game = startGame;
        photonView.RPC("OnStart", PhotonTargets.AllBufferedViaServer, new object[] { this.start_game, this.FishInf.FishInfoToString() });
    }

    [PunRPC]
    public void OnStart(bool startGame, string fishInfo)
    {
        //Debug.Log("Move2 show info = " + fishInfo + "  -  start game= " + startGame);
        this.FishInf = FishInfo.ToFishInfo(fishInfo);
        this.start_game = startGame;
    }

    public void CatchByBoat(int boatViewID)
    {
        photonView.RPC("OnCatch", PhotonTargets.AllBufferedViaServer, new object[] { boatViewID });
    }

    [PunRPC]
    protected void OnCatch(int viewID)
    {
        //Debug.Log("OnCatch fish id = " + photonView.viewID + " by boat id = " + viewID);
        PhotonView boatView = PhotonView.Find(viewID);
        if (boatView != null)
        {
            isCatch = true;
            boatView.GetComponent<BoatManager>().CatchFish(this.gameObject);
            //Debug.Log("Catch by " + boatView.name);
        }
    }

    void CheckFLip()
    {
        if (_myRigidbody.velocity.x >= 0)
        {
            transform.localScale = new Vector2(_someScale, transform.localScale.y);
        }

        else
        {
            transform.localScale = new Vector2(-_someScale, transform.localScale.y);
        }
    }

    public void RandomPath()
    {
        
    }

    public void SetIsFreeze(bool _isFreezee)
    {
        this.isFreeze = _isFreezee;
        if (isFreeze)
        {
            _myRigidbody.gravityScale = 1;

            GetComponent<Animator>().enabled = false;
            //myBox.isTrigger = true;
        }
        else {
            _myRigidbody.gravityScale = 0;

            GetComponent<Animator>().enabled = true;
        }
    }

    public void Unfreeze()
    {
        _myRigidbody.gravityScale = 0;
        myBox.isTrigger = true;
    }
}
