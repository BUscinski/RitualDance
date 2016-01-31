using UnityEngine;
using System.Collections;

public class ShapeMatch : MonoBehaviour {

	public GameObject shape;
	public GameObject objectToMatch;
	public Renderer redGreen;
	private const float PERCENTAGE = 0.35f;

	public Texture2D shapeTexture;
	private Texture2D poseTexture;
	private bool startMatch = false;
	private int pixelCount;
	public bool startTest = false;
	public bool currentlyPassing;

	private Color32 poseColor;

	// Use this for initialization
	void Start () {
		currentlyPassing = false;
		SetTexture(shapeTexture);
		poseColor = new Color32(0, 255, 0, 255);
	}
	public void SetTexture(Texture2D tex)
	{
		shapeTexture = tex;
		shape.GetComponent<Renderer>().material.mainTexture = shapeTexture;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.S))
		{
			print (shapeTexture.height * shapeTexture.width);
		}
	}
		       
	public void setColorPose(Color32 color) {
		poseColor = color;
	}
	public Texture2D getShapeDifference(Color32[] depthImg) {
		// set depth img
		if(startTest)
		{
			Texture2D resultTexture = new Texture2D(shapeTexture.width,shapeTexture.height,TextureFormat.ARGB32,false);
			poseTexture.SetPixels32 (depthImg);
			poseTexture.Apply(false);

			int numPixelsDirty = 0;
			Color cFail = new Color(255, 0, 0, 255);
			Color32 cPass = poseColor;//new Color32(0, 255, 0, 255);
			Color cT = new Color(255, 0, 0, 0);

			pixelCount = 0;
			for (int x = 0; x < shapeTexture.width; x++) {
				for(int y = 0;y < shapeTexture.height;y++){
					Color colorShape = shapeTexture.GetPixel(x,y);
					Color colorPose = poseTexture.GetPixel(x,y);
					if(colorPose.a == 1){
						pixelCount ++;
						resultTexture.SetPixel(x,y,cPass);
						if(colorShape.r > 0.09f) {
							resultTexture.SetPixel(x,y,cFail);
							numPixelsDirty++;
						}
					}
					else 
					{
						resultTexture.SetPixel(x,y,cT);
					}
				}
			}
			if(numPixelsDirty > (pixelCount *PERCENTAGE) || pixelCount == 0){
				redGreen.material.color = Color.red;
				currentlyPassing = false;
			}
			else
			{
				redGreen.material.color = Color.green;
				currentlyPassing = true;
			}

			resultTexture.Apply (false);
			return resultTexture;
		}

		return null;
	}
	public void FlipInput(int rotationNumber)
	{
		float smoothness = 5;
		switch(rotationNumber)
		{

			case 0:
				print ("Not Rotating");
				break;
			case 1:
				//shape.transform.localPosition = -shape.transform.localPosition;
				//objectToMatch.transform.Rotate(new Vector3(0, 180, 0));
			StartCoroutine (RotateOverTime (new Vector3(0, 180, 0)));	
			//StartCoroutine(RotateOverTime(new Vector3(0, 180, 0),-1));                                       
				print ("Rotating Y");
				break;
			case 2:
				shape.transform.localPosition = -shape.transform.localPosition;
				//objectToMatch.transform.Rotate(new Vector3(0, 0, 180));
			    StartCoroutine (RotateOverTime (new Vector3(0, 0, 180)));	

			    print ("Rotating Z");
				break;
			case 3:
				shape.transform.localPosition = shape.transform.localPosition;
			    StartCoroutine (RotateOverTime (new Vector3(180, 0, 180)));	

			    //objectToMatch.transform.Rotate(new Vector3(0, 180, 180));
				print ("Rotating YZ");
				break;
		}
	}
	public IEnumerator RotateOverTime(Vector3 rotation)
	{
		GameObject temp = new GameObject();
		temp.transform.rotation = objectToMatch.transform.rotation;
		temp.transform.Rotate (rotation);
		while((objectToMatch.transform.rotation.eulerAngles - temp.transform.rotation.eulerAngles).magnitude > 0.01f)
		{
			objectToMatch.transform.rotation = Quaternion.Slerp (objectToMatch.transform.rotation, temp.transform.rotation, Time.deltaTime * 3.0f);
			yield return null;
		}
		objectToMatch.transform.rotation = temp.transform.rotation;
		GameObject.Destroy (temp);
		yield return null;

	}
	public void StartGame()
	{
		pixelCount = shapeTexture.width * shapeTexture.height;
		startTest = true;
		poseTexture = new Texture2D(shapeTexture.width,shapeTexture.height,TextureFormat.ARGB32,false);

	}

}
