using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;

public class TrashSpawn : MonoBehaviour
{
    public List<GameObject> trashPoint;
    public Transform trashContainer; 

    public List<GameObject> trash;
    public Dictionary<string, GameObject> trashList;
    public float delay;
    public int startTrash;
	// Use this for initialization
	void Start ()
	{
	    delay = 0.3f;
	    trashList = new Dictionary<string, GameObject>();
	    trashList = trash.ToDictionary(x => x.name, x => x);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (startTrash > 0)
	    {
            InstantTrash();
	        startTrash--;
	    }
	    else
	    {
	        if (startTrash == 0)
	        {
	            startTrash--;
	            delay = 3;
	        }
	    }
	    SpawnTrash();
	}

    public void SpawnTrash()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            delay = 3f;
            if (trashContainer.childCount < 20)
            {
                InstantTrash();
                //kill fish
            }
        }
    }

    public void InstantTrash()
    {
        int trashID = Random.Range(0, trash.Count);
        int trashPointID = Random.Range(0, this.trashPoint.Count);
        GameObject go = Instantiate(trash[trashID], trashPoint[trashPointID].transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(trashContainer);
        Vector3 pos = new Vector3(Random.Range(-8f, 8f), Random.Range(0f, -5f));
        TrashMove(go, pos);
    }

    public void TrashMove(GameObject trash, Vector3 pos)
    {
        MovingTrash movingTrash = trash.GetComponent<MovingTrash>();
        HOTween.To(trash.transform, 3f, new TweenParms()
            .Prop("position", pos)
            /*.OnComplete(movingTrash.Shake)*/);
    }
}
