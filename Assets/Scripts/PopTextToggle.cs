// Author: Fatima Nadeem

using UnityEngine;
using UnityEngine.UI;

public class PopTextToggle : MonoBehaviour
/*
    This class helps pop text follow the player bubble and appear/disappear.
*/
{
    public Canvas gameCanvas;

    float scaleFactor;
    RectTransform rect;
    Text text;

    void Start()
    {
        scaleFactor = gameCanvas.scaleFactor;
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        Disappear();
    }

    public void Disappear()
    {
        text.enabled = false;
    }

    public void FollowAndAppear(Vector3 playerPos)
    {
        Vector2 myPos = Camera.main.WorldToScreenPoint(playerPos);
        myPos /= scaleFactor;
        rect.anchoredPosition = myPos;
        text.enabled = true;
    }
}
