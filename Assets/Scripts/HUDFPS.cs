using UnityEngine;
using System.Collections;
 
public class HUDFPS : MonoBehaviour 
{
 
// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
 
	public  float updateInterval = 0.5F;
	 
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	private string fpsText = "";
 
	void Start()
	{
	    /*if( !guiText )
	    {
	        Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
	        enabled = false;
	        return;
	    }*/
	    timeleft = updateInterval;  
	}
	 
	void Update()
	{
	    timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;
	 
	    // Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 )
	    {
	        // display two fractional digits (f2 format)
		float fps = accum/frames;
		string format = System.String.Format("{0:F2} FPS",fps);
		fpsText = format;
	 
		/*if(fps < 30)
			guiText.material.color = Color.yellow;
		else 
			if(fps < 10)
				guiText.material.color = Color.red;
			else
				guiText.material.color = Color.green;
		//	DebugConsole.Log(format,level);
	        timeleft = updateInterval;
	        accum = 0.0F;
	        frames = 0;*/
	    }
	}
	
	public void OnGUI() {
		// höhe und Breite
		int w = Screen.width;
		int h = 20;
		
		// einfarbige Textur 1
		Texture2D rgb_texture = new Texture2D(w, h);
	    Color rgb_color = new Color(0.9f, 0.9f, 0.9f,0.5f);
	    int i = 0;
	    int j = 0;
	    for(i = 0;i<w;i++)
	    {
	        for(j = 0;j<h;j++)
	        {
	            rgb_texture.SetPixel(i, j, rgb_color);
	        }
	    }
	    rgb_texture.Apply();
		
		GUIStyle generic_style = new GUIStyle();
   	 	GUI.skin.box = generic_style;
		GUI.skin.textArea = generic_style;
		
		GUI.BeginGroup(new Rect(0.0f, (float) Screen.height - h, (float) Screen.width, (float) h));
			GUI.Box (new Rect (0f,0f, (float)Screen.width, (float) h),rgb_texture);
			 GUI.TextArea(new Rect(5.0f,2.0f,100.0f,20.0f),fpsText);
		GUI.EndGroup();
	}
}