using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoSingleton<GameManager>
{
    public int[] listSpots;
    public Transform[] Spots;

    public List<GameObject> Fish;
    public Dictionary<string, GameObject> FishObjs;

    GameMode gameMode;

    // Use this for initialization
    void Start ()
    {
        FishObjs = Fish.ToDictionary(x => x.name, x => x);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void StartGame()
    {
        listSpots = new int[Spots.Length];

        gameMode = new GameOffline();
    }


    public int GetAvailableSpot()
    {
        for (int i = 0; i < listSpots.Length; i++)
        {
            if (listSpots[i] == 0)
            {
                return i;
            }
        }
        return 0;
    }

    public bool CheckSpotAvailable(int index)
    {
        if (listSpots[index] == 0)
        {
            return true;
        }
        return false;
    }

    public GameObject CreateFishByNameOffline(string fishName)
    {
        //Debug.Log("Create Fish name = " + fishName);
        if (this.FishObjs == null)
        {
            Debug.Log("Fuck");
            return null;
        }
        if (this.FishObjs.ContainsKey(fishName))
        {
            //Debug.Log("Contained key");
            return Instantiate(this.FishObjs[fishName], Vector3.zero, Quaternion.identity) as GameObject;
        }
        else {
            Debug.Log("FishObj not contain fish = " + fishName);
            return null;
        }

    }

}
