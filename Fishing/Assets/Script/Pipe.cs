using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Pipe : MonoBehaviour
{
    public GameObject trash;
    public Transform trashPoint;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SpawnTrash()
    {
        Vector3 newPos = new Vector3(Random.Range(-6,6), Random.Range(-1.5f, -3));
        GameObject go = Instantiate(trash, trashPoint.position, Quaternion.identity) as GameObject;
        HOTween.To(go.transform, 2f, "position", newPos);
    }
}
