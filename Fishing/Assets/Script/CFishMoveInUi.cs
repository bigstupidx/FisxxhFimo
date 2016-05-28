using UnityEngine;
using System.Collections;

public class CFishMoveInUi : MonoBehaviour {
    private float vx = 1;
    public float speed = 5.0f;

	// Use this for initialization
	void Start () {
        vx = transform.localScale.x;
	}

    private float timeDelay = 0.0f;
	// Update is called once per frame
	void Update () {
        if((timeDelay += Time.deltaTime) >=(Random.Range(12,15)))
        {
            vx *= -1;
            timeDelay = 0.0f;
        }
        
        transform.localPosition += new Vector3(vx, 0, 0) * Time.deltaTime * speed;
        transform.localScale = new Vector3(vx,1,1);
	}
}
