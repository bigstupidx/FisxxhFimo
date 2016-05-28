using UnityEngine;
using System.Collections;

public class BoatManager : MonoSingleton<BoatManager>
{
    public GameObject SwingSpot;
    public GameObject hookObj;
    public int SwingAngle;
    public bool IsSwing;
    public float SwingDir;

    public Hook myHook;

    public Transform[] _spots;
    public int newSpot;
    public int CurrentSpot;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Swing();
	}

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


    //hook fish
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
                    //ApplyIndexSpot(newSpot, isOnline);
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
                    //ApplyIndexSpot(newSpot, isOnline);
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
}
