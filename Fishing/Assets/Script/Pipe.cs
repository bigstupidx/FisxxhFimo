using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Pipe : Photon.MonoBehaviour
{
    public GameObject trash;
    public Transform trashPoint;
    public float delay;
    public Animator anim;
	// Use this for initialization
	void Start ()
	{
	    delay = 25;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (GameManager.Instance.gameMode.timeGame <= 15)
	    {
            return;
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
            delay = 8;
            anim.SetTrigger("startPipe");
        }
    }

    public void Spawn()
    {
        Vector3 newPos = new Vector3(Random.Range(-6, 6), Random.Range(-1.5f, -3));
       // GameObject go = Instantiate(trash, trashPoint.position, Quaternion.identity) as GameObject;

        GameObject go;

        if (GameManager.Instance.gameModeConfig == GameModeConfig.GAME_OFFLINE)
        {
            go = Instantiate(trash, trashPoint.position, Quaternion.identity) as GameObject;
            HOTween.To(go.transform, 2f, "position", newPos);
        }
        else
        {
            go = PhotonNetwork.Instantiate(trash.name, trashPoint.position, Quaternion.identity, 0) as GameObject;
            OnMove(go, newPos);
            //photonView.RPC("OnMove", PhotonTargets.AllBufferedViaServer, new object[] { go, newPos });
        }
    }
    public void OnMove(GameObject go, Vector3 newPos)
    {
        if(go.GetComponent<Trash>())
        {
            go.GetComponent<Trash>().MoveTrash(newPos);
        }
        
    }
}
