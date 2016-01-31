using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class DisplayDepth : MonoBehaviour {
	
	public DepthWrapper dw;
	public List<int> dirtyPixels;
	public Texture2D tex;
	public ShapeMatch shapesMatcher;
	public int maxDistance = 70;
	public int minDistance = 50;

	// Use this for initialization
	void Start () {
		tex = new Texture2D(320,240,TextureFormat.ARGB32,false);
		// = true;
		//tex = new Texture2D(80, 60, TextureFormat.ARGB32, false);
		GetComponent<Renderer>().material.mainTexture = tex;
		dirtyPixels = new List<int>();
	}
	
	// Update is called once per frame
	void Update () {
		if (dw.pollDepth())
		{
			Color32[] depth = convertDepthToColor(dw.depthImg);
			Texture2D result = shapesMatcher.getShapeDifference(depth);
			if(result != null){
				GetComponent<Renderer>().material.mainTexture = result;
			} else {
				tex.SetPixels32(depth);
				tex.Apply(false);
			}
		}

	}
	void OnGUI()
	{
	}
	private Color32[] convertDepthToColor(short[] depthBuf)
	{
		Color32[] img = new Color32[depthBuf.Length];
		short[]  flippedBits = new short[depthBuf.Length];
		// flipping depth img
		for (int i = 0; i < depthBuf.Length; i += tex.width) {
			for (int j = 0, k = tex.width - 1; j < tex.width; ++j, --k) {
				flippedBits[i + j] = depthBuf[i + k];
			}
		}
		for (int pix = 0; pix < flippedBits.Length; pix++)
		{
			int divisor = flippedBits[pix] / 32;
			if(divisor < maxDistance && divisor > minDistance)
			{
				img[pix].r = (byte)(0);
				img[pix].g = (byte)(150);
				img[pix].b = (byte)(150);
				img[pix].a = (byte)(255);
				dirtyPixels.Add (pix);
			}
			else
			{
				img[pix].r = (byte)255;
				img[pix].g = (byte)255;
				img[pix].b = (byte)255;
				img[pix].a = 0;
			}
		}
		dirtyPixels.Clear ();

		return img;
	}
	
	private Color32[] convertPlayersToCutout(bool[,] players)
	{
		Color32[] img = new Color32[320*240];
		for (int pix = 0; pix < 320*240; pix++)
		{
			if(players[0,pix]|players[1,pix]|players[2,pix]|players[3,pix]|players[4,pix]|players[5,pix])
			{
				img[pix].a = (byte)255;
			} else {
				img[pix].a = (byte)0;
			}
		}
		return img;
	}

	public Texture2D GetCurrentTexture()
	{
		return tex;
	}
}
