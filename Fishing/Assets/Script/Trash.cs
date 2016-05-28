using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Trash : Photon.MonoBehaviour
{
    public float time;
    public GameObject smoke;
    public CircleCollider2D box;

    public float radius;
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

    public void MoveTrash(Vector3 newPos)
    {
        photonView.RPC("OnMove", PhotonTargets.AllBufferedViaServer, new object[] { newPos });
    }

    [PunRPC]
    public void OnMove(Vector3 newPos)
    {
        HOTween.To(transform, 2f, "position", newPos);
    }

    public void Explose()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            box.enabled = true;
            box.radius = radius;
            HOTween.To(smoke.transform, 0.4f, new TweenParms()
                .Prop("localScale", new Vector3(3, 1, 1.5f))
                .Ease(EaseType.EaseOutCubic)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                }));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.LogError(col.tag);
        if (col.tag == "Fish")
        {
            GameManager.Instance.gameMode.AddFishFreezee(col.gameObject);
            // TODO: set isFreeze = true;
            //col.gameObject.GetComponent<Move2>().isFreeze = true;
        }
    }

}
