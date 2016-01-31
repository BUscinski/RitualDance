using UnityEngine;
using System.Collections;

public class ColorDepthImage : MonoBehaviour {

	public DisplayColor colorImg;
	public DisplayDepth depthImg;

	private Texture2D colorDepthImg;
	public Texture2D savedCopy;

	// Use this for initialization
	void Start () {
		colorDepthImg = new Texture2D(320,240,TextureFormat.ARGB32,false);
		savedCopy = new Texture2D(320,240,TextureFormat.ARGB32,false);
	}
	
	// Update is called once per frame
	void Update () {
		if(colorImg != null && depthImg != null){
			computeImg();
		}
	}

	//Compute image from depoth and color image
	void computeImg(){

		Texture2D color = colorImg.GetCurrentTexture();
		Texture2D depth = depthImg.GetCurrentTexture();

		for(int x = 0;x< color.width;x++) {
			for(int y = 0;y< color.height;y++) {
				colorDepthImg.SetPixel(x,y, new Color(255,0,0,0));
				if(depth.GetPixel(x,y).a == 1) {
					Color color1 = color.GetPixel(x,y);
					color1.a = 1;
					colorDepthImg.SetPixel(x,y,color1);
				} 
			}
		}
		colorDepthImg.Apply(false);
		GetComponent<Renderer>().material.mainTexture = colorDepthImg;

	}

	//saving screen shot of current pose
	public Texture2D SaveCopyImg() {
		savedCopy.SetPixels32(colorDepthImg.GetPixels32());
		savedCopy.Apply(false);

		return savedCopy;
	}
}
