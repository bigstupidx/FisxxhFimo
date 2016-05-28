using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CBaseButtonClick : MonoBehaviour {
    virtual public void OnClicked() { }
    void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(OnClicked);
    }
}
