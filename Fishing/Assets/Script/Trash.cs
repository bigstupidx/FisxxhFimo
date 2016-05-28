using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Trash : MonoBehaviour
{
    public float time;
    public GameObject smoke;
    public CircleCollider2D box;

    // Use this for initialization
    void Start()
    {
        time = 4;
    }

    // Update is called once per frame
    void Update()
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
            box.radius = 2.5f;
            HOTween.To(smoke.transform, 0.4f, new TweenParms()
                .Prop("localScale", new Vector3(3, 1, 1.5f))
                .Ease(EaseType.EaseOutCubic)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                }));
        }
    }

    public void ScaleCollider()
    {
        float t = 0;
        box.radius = Mathf.Lerp(0, 2.5f, t);
        t += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.LogError(col.tag);
        if (col.tag == "Fish")
        {
            // TODO: set isFreeze = true;
        }
    }

}
