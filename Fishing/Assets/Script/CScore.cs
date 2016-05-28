using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CScore : MonoBehaviour {
    [SerializeField]
    private float countFish = 0.0f; // so luong ca

    public float CountFish
    {
        get { return countFish; }
        set { countFish = value; }
    }
    [SerializeField]
    private float countToxic = 0.0f; //so luong chat doc

    public float CountToxic
    {
        get { return countToxic; }
        set { countToxic = value; }
    }
    [SerializeField]
    private float countTrash = 0.0f;

    public float CountTrash
    {
        get { return countTrash; }
        set { countTrash = value; }
    }

    public Text txtCountFish;
    public Text txtCountToxic;
    public Text txtCountTrash;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(txtCountFish)
        {
            txtCountFish.text = countFish.ToString();
        }
        if(txtCountToxic)
        {
            txtCountToxic.text = CountToxic.ToString();
        }
        if(txtCountTrash)
        {
            txtCountTrash.text = CountTrash.ToString();
        }
	}
}
