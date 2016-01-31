using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {
	public enum SfxClip
	{
		Wrong,
		Success,
		Countdown
	};

	public AudioClip[] allSFX;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void PlaySfxAtLocation(Vector3 source, SFXManager.SfxClip clipNum)
	{
		GameObject temp = new GameObject();
		temp.transform.position = source;
		temp.AddComponent<AudioSource>();
		temp.GetComponent<AudioSource>().PlayOneShot(allSFX[(int)clipNum]);
		StartCoroutine (WaitForSfx (temp));

		//		AudioClip.allSFX[(int)clipNum]
	}
	private IEnumerator WaitForSfx(GameObject temp)
	{
		while(temp.GetComponent<AudioSource>().isPlaying)
		{
			yield return null;
		}
		GameObject.DestroyImmediate (temp);
	}
}
