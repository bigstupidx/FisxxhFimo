using UnityEngine;
using System.Collections;

public class GameManager : MonoSingleton<GameManager>
{
    public int[] listSpots;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

}
