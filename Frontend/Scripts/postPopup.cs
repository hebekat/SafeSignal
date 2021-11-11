using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class postPopup : MonoBehaviour
{
  
	public IEnumerator PostDelete()
	{
		yield return new WaitForSeconds(2);
		StartCoroutine(GetRequest("http://kldafgn.altervista.org/next/request.php?d"));
	}

	public void	PopupPost(string value)
	{
		StartCoroutine(GetRequest("kldafgn.altervista.org/next/request.php?p=" + value));
		StartCoroutine(PostDelete());
	}

	IEnumerator	GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
        }
    }
}
