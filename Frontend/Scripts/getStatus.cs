using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class getStatus : MonoBehaviour
{
	public Animator[] anims, banners;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(check());
    }

    IEnumerator check()
    {
        while (true) {
            StartCoroutine(GetRequest("kldafgn.altervista.org/next/request.php"));
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
        }
    }

	//Sends signals to the backend
	void elaborate(string res)
	{
		switch (res)
		{
			case "danger":
				foreach (Animator a in anims)
                {
					a.gameObject.GetComponent<SpriteRenderer>().color = new Color(233, 0, 0, .25f);
					a.SetBool("scale", true);
				}
				StartCoroutine(EndTimer());
				break;
			case "slow":
				foreach (Animator a in anims)
                {
					a.gameObject.GetComponent<SpriteRenderer>().color = new Color(29, 233, 0, .25f);
					a.SetBool("scale", true);
				}
				StartCoroutine(EndTimer());
				break;
			case "e1":
				banners[0].SetBool("run", true);
				break;
		}
	}

	public void AnswerBanner(string ans)
	{
		StartCoroutine(GetRequest("kldafgn.altervista.org/next/request.php?p=" + ans));
		foreach (Animator a in banners)
		{
			a.SetBool("run", false);
		}
	}

	IEnumerator EndTimer()
	{
		yield return new WaitForSeconds(5);
			foreach (Animator a in anims)
            	a.SetBool("scale", false);
		yield break;
	}
}
