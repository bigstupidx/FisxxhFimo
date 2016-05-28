using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HandleMulti : MonoBehaviour {

    public Transform playerPrefab;
    public Transform rootBoat;
    // Use this for initialization
    void Awake()
    {
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Main");
            return;
        }

        GameObject boat = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity, 0) as GameObject;
        boat.transform.parent = rootBoat;
        boat.transform.localPosition = Vector3.zero;
        SwipeDetector.Instance.boat = boat.GetComponent<BoatManager>();
    }
}
