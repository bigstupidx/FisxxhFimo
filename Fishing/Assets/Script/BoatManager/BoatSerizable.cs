using UnityEngine;
using System.Collections;

public class BoatSerizable : Photon.MonoBehaviour {

    public BoatManager boatManager;
    public Transform hookTrans;
    // Use this for initialization
    void Start()
    {

    }

    private Vector3 correctPosHook;
    private Quaternion correctRotateHook;
    private int currentSpot;
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (boatManager.IsSwing)
        //{
        //    return;
        //}
        if (stream.isWriting)
        {
            stream.SendNext(boatManager.CurrentSpot);
            stream.SendNext(hookTrans.position);
            stream.SendNext(hookTrans.rotation);
        }
        else
        {
            currentSpot = (int)stream.ReceiveNext();
            correctPosHook = (Vector3)stream.ReceiveNext();
            correctRotateHook = (Quaternion)stream.ReceiveNext();
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Serizable with is mine = " + photonView.isMine);
        if (!photonView.isMine) // && !boatManager.IsSwing
        {
            boatManager.ApplyIndexSpot(currentSpot, false);
            hookTrans.position = Vector3.Lerp(hookTrans.position, correctPosHook, Time.deltaTime * 5);
            hookTrans.rotation = Quaternion.Lerp(hookTrans.rotation, correctRotateHook, Time.deltaTime * 5);
        }
    }
}
