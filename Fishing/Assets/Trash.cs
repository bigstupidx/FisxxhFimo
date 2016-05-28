using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Trash : MonoBehaviour
{
    public float time;
    public GameObject smoke;

	// Use this for initialization
	void Start ()
	{
	    time = 4;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    Explose();
	}

    public void Explose()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            HOTween.To(smoke.transform, 0.4f, new TweenParms()
                .Prop("localScale", new Vector3(3, 1, 1.5f))
                .Ease(EaseType.EaseOutCubic)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                }));
        }
    }
}
