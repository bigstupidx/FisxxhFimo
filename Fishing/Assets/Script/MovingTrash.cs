using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MovingTrash : MonoBehaviour
{
    public float radius;

    public Vector3 pos1;
    public Vector3 pos2;

	// Use this for initialization
	void Start ()
    { 
        //Shake();
	    
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shake()
    {
        pos1 = new Vector3(-radius + transform.position.x, transform.position.y);
        pos2 = new Vector3(radius + transform.position.x, transform.position.y);
        HOTween.To(transform, 1.5f, new TweenParms()
            .Prop("position", pos1)
            .OnComplete(() =>
            {
                HOTween.To(transform, 2f, new TweenParms()
                    .Prop("position", pos2)
                    .Loops(-1, LoopType.YoyoInverse));
            }));
    }

    void OnDisable()
    {
        //Debug.LogError("False");
        HOTween.Pause(gameObject);
    }
}
