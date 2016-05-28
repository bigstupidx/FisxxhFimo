using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour
{
    public BoatManager boatMan;

    public bool IsMove;
    public bool IsBack;
    public float Speed;
    public GameObject Fish;

    public Transform BoatPos;
    public Transform Root;
    public Transform End;
    public Animator PlayerAnimator;
    public float Dis;

    public LineRenderer _line;

    //Power up
    public string PowerUps;
    public float PowerUpTime;
    public float SpeedBonus = 0;

    public Sprite Sprite;

    public bool catchFish;
    // Use this for initialization
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>().sprite;
        IsMove = false;
        IsBack = false;
        //_line = GetComponent<LineRenderer>();
        _line.sortingLayerName = "Foreground";
        _line.SetColors(Color.white, Color.white);

        //if (!boatMan.isMine)
        //if (!boatMan.isMine)
        //{
        //    GetComponent<BoxCollider2D>().enabled = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //DrawLine();
        BackToBoat();
        //if (!boatMan.isMine)
        //{
        //    return;
        //}

        Move();
    }

    public void Move()
    {
        if (IsMove)
        {
            float speed = Speed * Time.deltaTime;
            transform.position -= transform.up * speed;
        }
    }

    // Return hook to boat
    public void BackToBoat()
    {
        if (IsBack)
        {
            float speed = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, BoatPos.position, speed);
            if (Fish != null)
            {
                if (Fish.tag == "Fish")
                {
                    CalculateFishPos();// TODO: uncomment
                    Fish.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (Fish.tag == "Trash")
                {
                    CalculateFishPos();// TODO: uncomment
                    Fish.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {

                    Debug.Log("BackToBoat: Can't not calculate Fish pos. Tag = " + Fish.tag);
                }

            }
            else
            {
                Debug.Log("BackToBoat: Fish null");
            }
            float distance = Vector3.Magnitude(BoatPos.position - transform.position);
            if (distance <= 0.15f) // When hook returned to boat
            {

                IsBack = false;
                GetComponentInParent<BoatManager>().IsSwing = true;
                PlayerAnimator.SetBool("IsUp", false);
                if (Fish != null)
                {
                    if (Fish.tag == "PowerUp")
                    {
                        if (Fish.name == "Faster")
                        {
                            SpeedBonus = 2;
                        }
                    }
                    else
                    {
                        //boatMan.IncreaseScore(Fish); TODO: uncomment
                        //if (Fish.GetComponent<Move2>())
                        //{
                        //    Fish.GetComponent<Move2>().enabled = true;
                        //}
                    }
                    //Fish.GetComponent<Move2>().FishInf.Status = 2;
                    Fish.SetActive(false);
                    //GameManagerNew.Instance.RemoveFishFromList(Fish.GetComponent<Move2>().FishInf, Fish); TODO: uncomment
                    //PoolObject.Instance.DespawnFish(Fish);
                }
                Fish = null;
                //GetComponent<BoxCollider2D>().enabled = true;
                Speed = 4 + SpeedBonus;
                Audio.Instance.StopEffect();
            }
        }
    }

    public void RotateFish()
    {
        var rot = Root.transform.rotation;
        Fish.transform.localScale = new Vector3(Mathf.Abs(Fish.transform.localScale.x), Fish.transform.localScale.y);
        Fish.transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, 360 - rot.eulerAngles.z);
    }

    // Check collision with fish.
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Wall" && !IsBack)
        {
            IsMove = false;
            IsBack = true;
            PlayerAnimator.SetBool("IsUp", true);
            PlayerAnimator.SetBool("IsCatch", false);
            return;
        }

        if (other.tag == "Mouth")
        {
            if (GameManager.Instance.gameMode.catchFish)
            {
                GameObject fishObj = other.transform.parent.gameObject;
                if (!fishObj.GetComponent<Move2>().isCatch)
                {
                    IsMove = false;
                    IsBack = true;
                    GetComponent<BoxCollider2D>().enabled = false;
                    PlayerAnimator.SetBool("IsUp", true);
                    PlayerAnimator.SetBool("IsCatch", false);


                    if (GameManager.Instance.gameModeConfig == GameModeConfig.GAME_OFFLINE)
                    {
                        fishObj.GetComponent<Move2>().isCatch = true;
                        boatMan.CatchFish(fishObj);
                    }
                    else
                    {
                        //fishObj.GetComponent<Move2>().CatchByBoat(boatMan.photonView.viewID);
                    }
                }
            }
            else
            {
                //Destroy Fish
            }
        }

        if (other.tag == "Trash")
        {
            CatchTrash(other.gameObject);
        }

        //play sound

        Audio.Instance.StopHookSound();

        Audio.Instance.PlayEffect(SoundType.SWIRLING, true, 0);
    }

    public void CatchFish(GameObject fishObj)
    {
        IsMove = false;
        IsBack = true;
        PlayerAnimator.SetBool("IsUp", true);
        PlayerAnimator.SetBool("IsCatch", false);

        Fish = fishObj;
        if (Fish.GetComponent<Move2>())
        {
            Fish.GetComponent<Move2>().enabled = false;
        }
        Dis = (GetComponent<Renderer>().bounds.extents.y + Fish.GetComponent<Renderer>().bounds.extents.x) - 0.05f;
        //Speed -= Fish.GetComponent<FishProperties>().Weight;
        Fish.transform.localScale = new Vector3(Mathf.Abs(Fish.transform.localScale.x), Fish.transform.localScale.y);
        Fish.transform.right = Root.transform.up;
    }

    public void CatchTrash(GameObject trashObj)
    {
        IsMove = false;
        IsBack = true;
        PlayerAnimator.SetBool("IsUp", true);
        PlayerAnimator.SetBool("IsCatch", false);
        Fish = trashObj;
        Fish.transform.localScale = new Vector3(Mathf.Abs(Fish.transform.localScale.x), Fish.transform.localScale.y);
        Fish.transform.right = Root.transform.up;
    }

    void DrawLine()
    {
        _line.SetPosition(0, new Vector3(Root.position.x, Root.transform.position.y, -5));
        _line.SetPosition(1, new Vector3(End.position.x, End.position.y, -5));
        float Distance = Vector3.Distance(Root.position, End.position);
        _line.material.mainTextureScale = new Vector2(Distance * 2, 1f);
        //Debug.DrawLine(Root.transform.position, transform.position);
    }

    void CalculateFishPos()
    {
        Vector3 right = new Vector3(Root.position.x, Root.position.y) - new Vector3(Root.position.x - 1, Root.position.y);
        Vector3 down = new Vector3(Root.position.x, Root.position.y) - new Vector3(transform.position.x, transform.position.y);
        float angle = Vector3.Angle(down, right);
        float x = transform.position.x + (Dis * Mathf.Cos(135 + angle * Mathf.Deg2Rad));
        float y = transform.position.y + (Dis * Mathf.Sin(135 + angle * Mathf.Deg2Rad));
        Fish.transform.position = new Vector3(x, y);
    }
}
