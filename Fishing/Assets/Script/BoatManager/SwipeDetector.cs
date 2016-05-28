using System.CodeDom;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeDetector : MonoSingleton<SwipeDetector>, IPointerDownHandler, IPointerUpHandler
{
    int currentspot;
    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;

    public Text text;
    public BoatManager boat;

    void Start()
    {
        minSwipeDistX = 50;
        minSwipeDistY = 20;
    }

    public void left()
    {
        Debug.Log("Left");
        //boat.MoveLeft();
    }

    public void right()
    {
        Debug.Log("Right");
    //boat.MoveRight();
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    left();
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    right();
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_EDITOR
        Debug.Log("Down");
#endif
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //#if UNITY_EDITOR
        //        Debug.Log("Up");
        //#endif
        //        if (GameManagerNew.Instance.gameModeConfig != GameModeConfig.GAME_OFFLINE)
        //        {
        //#if UNITY_EDITOR
        //            Debug.Log("Game Multi - Check game Run");
        //#endif
        //            if (!GameManagerNew.Instance.CheckGameRun())
        //            {
        //#if UNITY_EDITOR
        //                Debug.Log("Game Multi - Game not running");
        //#endif
        //                return;
        //            }
        //        }

        float swipeDistHorizontal = (new Vector3(eventData.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
        if (swipeDistHorizontal > minSwipeDistX)
        {
            float swipeValue = Mathf.Sign(eventData.position.x - startPos.x);
            if (swipeValue > 0)
            {
                right();
            }
            else if (swipeValue < 0)
            {
                left();
            }
        }
        else
        {
            Debug.Log("down");
            boat.Hook();
        }
    }
}
