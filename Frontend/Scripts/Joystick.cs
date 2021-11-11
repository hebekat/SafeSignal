using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Joystick : MonoBehaviour
{
    public GameObject pointer;
    public Animator[] anims;
    public Image[] popups;
    public SpriteRenderer[] indicators;
    public int direction;

    SpriteRenderer sr;
    Vector2 origin;
    Color currentColor;
    Color newCol;
    bool run;
    int res;
	bool popon;
    

    IEnumerator SetOpacity(SpriteRenderer sr)
    {
        //Fades in touch indicator
        while (true)
        {
            if (sr.color.a >= 1)
                yield break;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + 0.01f);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Start()
    {
		popon = false;
        sr = pointer.GetComponent<SpriteRenderer>();
        run = false;
    }
    void Update()
    {
        //Slides in the indicators and starts tracking finger movement
        if (Input.GetMouseButtonDown(0))
        {
            foreach (Animator a in anims)
                a.SetBool("slideIn", true);
            origin = (Vector2)Input.mousePosition;
            run = true;
            pointer.transform.position = Camera.main.ScreenToWorldPoint(origin);
            pointer.transform.position = new Vector3(pointer.transform.position.x, pointer.transform.position.y, 0);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            sr.enabled = true;
            StartCoroutine(SetOpacity(sr));
        }

        //Highlights the indicator user is currently pointing at
        if (run && Vector2.Distance(origin, (Vector2)Input.mousePosition) > 30)
        {
            
            res = CheckVector(origin - (Vector2)Input.mousePosition);
            switch (res)
            {
               
                case 1:
                    resetIndicatorColors();
                    ColorUtility.TryParseHtmlString("#1379EC", out newCol);
                    indicators[0].color = newCol;
                    break;
                case 2:
                    resetIndicatorColors();
                    ColorUtility.TryParseHtmlString("#3BE843", out newCol);
                    indicators[1].color = newCol;
                    break;
                case 3:
                    resetIndicatorColors();
                    ColorUtility.TryParseHtmlString("#E83F50", out newCol);
                    indicators[2].color = newCol;                 
                    break;
                    
                case 4:
                    resetIndicatorColors();
                    ColorUtility.TryParseHtmlString("#E5D160", out newCol);
                    indicators[3].color = newCol;
                    break;
            }
        }

        //Triggers the behaviour the user selected 
        if (Input.GetMouseButtonUp(0))
        {
            
            foreach (Animator a in anims)
                a.SetBool("slideIn", false);
            run = false;
            sr.enabled = false;
            pointer.transform.position = new Vector2(5, 0);
            direction = 0;
            switch (res)
            {
                case 3:
                    StartCoroutine(GetRequest("http://kldafgn.altervista.org/next/request.php?p=danger"));
					StartCoroutine(PostDelete());
                    break;
                case 1:
                    StartCoroutine(GetRequest("http://kldafgn.altervista.org/next/request.php?p=fuel"));
					StartCoroutine(PostDelete());

                    break;
                case 4:
					popon = !popon;
					if (popon)
					{
						foreach (Image srs in popups)
							srs.enabled = true;
					}
					else
					{
						foreach (Image srs in popups)
							srs.enabled = false;
					}
                    break;
                case 2:
                    StartCoroutine(GetRequest("http://kldafgn.altervista.org/next/request.php?p=slow"));
					StartCoroutine(PostDelete());
                    break;
            }
            res = 0;
        }
    }

    //up = 1, right = 2, down = 3, left = 4;

    int CheckVector(Vector2 vec)
    {
        float angle = Vector2.SignedAngle(Vector2.down, vec);
        if (angle < 45 && angle >= -45)
            return (1);
        else if (angle < 135 && angle >= 45)
            return (4);
        else if (angle < -135 || angle >= 135)
            return (3);
        else if (angle < -45 && angle >= -135)
            return (2);
        return (0);
    }

    void resetIndicatorColors()
    {
        ColorUtility.TryParseHtmlString("#6B9DD5CC", out newCol);
        indicators[0].color = newCol;
        ColorUtility.TryParseHtmlString("#59B15DCC", out newCol);
        indicators[1].color = newCol;
        ColorUtility.TryParseHtmlString("#B15861CC", out newCol);
        indicators[2].color = newCol;
        ColorUtility.TryParseHtmlString("#B2A559CC", out newCol);
        indicators[3].color = newCol;
    }

	IEnumerator PostDelete()
	{
		yield return new WaitForSeconds(2);
		StartCoroutine(GetRequest("http://kldafgn.altervista.org/next/request.php?d"));
	}

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
        }
    }
}
