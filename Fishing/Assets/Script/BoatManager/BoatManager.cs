using UnityEngine;
using System.Collections;

public class BoatManager : Photon.MonoBehaviour
{
    public GameObject SwingSpot;
    public GameObject hookObj;
    public int SwingAngle;

    [HideInInspector]
    public bool IsSwing;

    //[HideInInspector]
    public float SwingDir;

    [HideInInspector]
    public bool isMine;

    [HideInInspector]
    public Transform[] _spots;

    [HideInInspector]
    public int CurrentSpot;

    [HideInInspector]
    public int newSpot;
    public Hook myHook;

    [HideInInspector]
    public bool isOnline;

    [HideInInspector]
    public int Score;

    [HideInInspector]
    public int FishGoal;

    protected string boat_name;
    protected string character_name;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Boat offfline");
        PreStart();
    }

    public void PreStart()
    {
        LoadBoatNPlayer();
        IsSwing = true;
        _spots = GameManager.Instance.Spots;
        CurrentSpot = -1;
        newSpot = -1;
        Score = 0;
        FishGoal = 0;
        this.ApplyRandomeSpot();
        StartGame();
    }


    public virtual void StartGame()
    {
        isMine = true;
        isOnline = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (!isMine)
        {
            return;
        }
        Swing();

    }

    public void CatchFish(GameObject Fish)
    {
        Debug.Log("My hook in Boat Manager");
        myHook.CatchFish(Fish);
    }

    public void CatchTrash(GameObject Trash)
    {
        Debug.Log("My hook in Boat Manager");
        myHook.CatchTrash(Trash);
    }

    // Swing the worm hook.
    public void Swing()
    {
        if (IsSwing)
        {
            SwingSpot.transform.rotation = Quaternion.Euler(0, 0,
                SwingSpot.transform.rotation.eulerAngles.z + SwingDir);
            if ((int)SwingSpot.transform.rotation.eulerAngles.z == SwingAngle)
            {
                SwingDir = -1; //Change direct
            }

            if ((int)SwingSpot.transform.rotation.eulerAngles.z == 360 - SwingAngle)
            {
                SwingDir = 1;
            }
        }
    }
    [ContextMenu("move")]
    public virtual void Hook() //stop swing, hook move forward.
    {

        //Transform hook = SwingSpot.transform.GetChild(0);
        if (myHook.IsBack)
        {
            return;
        }
        myHook.IsMove = true;
        hookObj.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInChildren<Animator>().SetBool("IsCatch", true);
        IsSwing = false;
        Audio.Instance.Hook();
    }

    [ContextMenu("test")]
    public void Test()
    {
        Transform go = SwingSpot.transform.GetChild(0);
        Debug.Log("Angle: " + Vector3.Angle(go.position, Vector3.right));
    }

    public void ApplyRandomeSpot()
    {
        int index = GameManager.Instance.GetAvailableSpot();
        Debug.Log("Apply Random  = " + index);
        CurrentSpot = -1;
        //Debug.Log("Apply Randome with index = " + index + "-- current spot = " + CurrentSpot);
        ApplyIndexSpot(index, false);
    }


    public void ApplyIndexSpot(int index, bool hasChange)
    {
        Debug.Log("Current index = " + CurrentSpot + " - new index = " + index);
        GameManager.Instance.SetSpotByIndex(index, CurrentSpot, transform, hasChange);
        transform.localPosition = Vector3.zero;
        CurrentSpot = index;
        newSpot = index;
        if (isMine)
        {
            SetIndexPlayerOnline();
        }
    }

    public virtual void SetIndexPlayerOnline()
    {

    }



    [ContextMenu("move right")]
    public virtual void MoveRight()
    {
        if (IsSwing)
        {
            newSpot++;
            if (newSpot < _spots.Length)
            {
                Debug.Log("Current: " + CurrentSpot);
                if (GameManager.Instance.CheckSpotAvailable(newSpot))
                {
                    //Offline nen khong can
                    ApplyIndexSpot(newSpot, isOnline);
                }
                else
                {
                    MoveRight();
                }
            }
            else
            {
                newSpot = -1;
                MoveRight();
            }
        }
    }

    public virtual void MoveLeft()
    {
        if (IsSwing)
        {
            newSpot--;
            if (newSpot >= 0)
            {
                if (GameManager.Instance.CheckSpotAvailable(newSpot))
                {

                    //Offline nen ko can
                    ApplyIndexSpot(newSpot, isOnline);
                }
                else
                {
                    MoveLeft();
                }
            }
            else
            {
                newSpot = _spots.Length;
                MoveLeft();
            }
        }
    }


    public virtual void LoadBoatNPlayer()
    {
        Debug.Log("Load Boat Offline");
        LoadDataOrSyncBoat();
        //SetBoatNPlayer();
    }

    public virtual void LoadDataOrSyncBoat()
    {
        //this.boat_name = PlayerPrefs.GetString(RoomProperties.PLAYER_BOAT, "Default");
        //this.character_name = PlayerPrefs.GetString(RoomProperties.PLAYER_NAME_CHARACTER, "Cyan");
    }

}
