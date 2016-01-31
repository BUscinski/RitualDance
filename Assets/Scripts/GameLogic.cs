using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public enum Difficulty
	{
		Stay,
		FlipY,
		FlipX,
		FlipXandY
	};
	private const int MAIN_TIMER = 5;
	private int gamePoints = 0;
	private int numCompleted = 0;
	private int currentTimer;
	private SFXManager sfxManager;
	public Texture2D[] startButtons;
	private int startButtonIndex;
	public ShapeMatch shapeMatch;

	public Color32[] colorPose;
	public Texture2D []textures;
	private int selectedTexture;
	private bool hovering;

	public Difficulty currentDifficulty;

	public GUIStyle guiStyle_timer;
	public GUIStyle guiStyle_score;

	public ColorDepthImage colorDepthImg;

	// Use this for initialization
	void Start () {
		selectedTexture = Random.Range (0, textures.Length);
		selectedTexture = 0;
		sfxManager = GameObject.Find ("SFX Manager").GetComponent<SFXManager>();
	}

	public void SetShape() {
		if(selectedTexture < textures.Length && selectedTexture < colorPose.Length) {
			print (colorPose.Length);
			shapeMatch.SetTexture (textures[selectedTexture]);
			shapeMatch.setColorPose(colorPose[selectedTexture]);
		} 
		else {
			print ("add textures and colors");
		}
	}
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space))
		{
			shapeMatch.StartGame ();
			SetShape();
			StartTimerUpdate();
		}

		if(Input.GetKeyDown (KeyCode.L))
		{
			ChooseInputOrientation();
		}
		if(currentTimer > MAIN_TIMER - 4) {
			guiStyle_timer.normal.textColor = Color.red;
			float duration = 0.7f;
			float phi = Time.time/duration * 2 * Mathf.PI;
			float amplitude = (float)Mathf.Cos(phi)*4 + 30;
			guiStyle_timer.fontSize = (int)amplitude;
		}
	}

	void OnGUI() {
		GUI.Label( new Rect(Screen.width/2,15,100,20), currentTimer + " : " + MAIN_TIMER, guiStyle_timer);
		GUI.Label( new Rect(20,30,100,20), "Score = " + gamePoints, guiStyle_score);
		GUI.Label ( new Rect(20,55,100,20), "Number Shapes Attempted = " + numCompleted, guiStyle_score);
	}
	public void StartTimerUpdate()
	{
		StartCoroutine(TimerUpdate());
	}
	public IEnumerator TimerUpdate() {
		yield return new WaitForSeconds(1);
		currentTimer ++;

		if (currentTimer > MAIN_TIMER) {
			// setting selected shape to match
			int newSelectedTexture = (selectedTexture + 1)%textures.Length;
			selectedTexture = newSelectedTexture;
			SetShape();
			currentTimer = 0;
			guiStyle_timer.normal.textColor = new Color32(6,152,255,255);


			print ("change texture");
			if(shapeMatch.currentlyPassing)
			{
				// increase points and take screenshot:
				Texture2D picture = colorDepthImg.SaveCopyImg();
				gamePoints++;
				sfxManager.PlaySfxAtLocation(Vector3.zero, SFXManager.SfxClip.Success);
			}
			else
			{
				sfxManager.PlaySfxAtLocation(Vector3.zero, SFXManager.SfxClip.Wrong);
			}
			numCompleted++;
			switch(gamePoints)
			{
				case 3:
					currentDifficulty = Difficulty.FlipX;
					break;
				case 5:
					currentDifficulty = Difficulty.FlipY;
					break;
				case 7:
					currentDifficulty = Difficulty.FlipXandY;
					break;
			}

			ChooseInputOrientation ();
			yield return new WaitForSeconds(2.5f);
		}
		if(currentTimer > MAIN_TIMER -5)
		{
			sfxManager.PlaySfxAtLocation (Vector3.zero, SFXManager.SfxClip.Countdown);
		}
		yield return StartCoroutine (TimerUpdate ());
	}


	private void ChooseInputOrientation()
	{
		int randRotationNumber = 0;
		switch(currentDifficulty)
		{
		case Difficulty.Stay:
			randRotationNumber = 0;
			break;
		case Difficulty.FlipX:
			randRotationNumber = Random.Range (0, 2);
			break;
		case Difficulty.FlipY:
			randRotationNumber = Random.Range (0, 3);
			break;
		case Difficulty.FlipXandY:
			randRotationNumber = Random.Range (0, 4);
			break;
		}
		shapeMatch.FlipInput(randRotationNumber);
	}
}
