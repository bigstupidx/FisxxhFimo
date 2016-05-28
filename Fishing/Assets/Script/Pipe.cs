using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Pipe : MonoBehaviour
{
    public GameObject trash;
    public Transform trashPoint;
    public float delay;
    public Animator anim;
	// Use this for initialization
	void Start ()
	{
	    delay = 20;
	}
	
	// Update is called once per frame
	void Update ()
    {
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
            delay = 5;
            anim.SetTrigger("startPipe");
        }
    }

    public void Spawn()
    {
        Vector3 newPos = new Vector3(Random.Range(-6, 6), Random.Range(-1.5f, -3));
        GameObject go = Instantiate(trash, trashPoint.position, Quaternion.identity) as GameObject;
        HOTween.To(go.transform, 2f, "position", newPos);
    }
}
