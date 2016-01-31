using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StartButtonController : MonoBehaviour, IPointerEnterHandler {
	public Texture2D[] startButtons;
	private int startButtonIndex = 0;
	private bool hovering;

	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if(!hovering)
		{
			if(startButtonIndex < startButtons.Length - 1)
			{
				startButtonIndex++;
			}
			else
			{
				startButtonIndex = 1;
			}
		}
		GetComponent<CanvasRenderer>().SetTexture(startButtons[startButtonIndex]);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		startButtonIndex = 0;
		hovering = true;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		startButtonIndex = 1;
		hovering = false;
	}
	public void Disable()
	{
		GameObject.Destroy (gameObject);
	}
}
