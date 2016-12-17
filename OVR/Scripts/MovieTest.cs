using UnityEngine;
using System.Collections;
public class MovieTest : MonoBehaviour
{
	private  static MovieTest m_instance;
	void Awake(){
		
		if(m_instance==null)
		{
			m_instance = this;
		}
	}
	// Use this for initialization
	void Start () 
	{

	}// Update is called once per frame
	void Update () 
	{
		
	}
	void OnGUI()
	{

		if (GUI.Button(new Rect(20, 10, 200, 50), "(h)CancelOnTouch"))  
		{  
			Handheld.PlayFullScreenMovie("Test.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);  
		}  

		if (GUI.Button(new Rect(20,90,200,25), "(h)Full"))  
		{  
			Handheld.PlayFullScreenMovie("Test.mp4", Color.black, FullScreenMovieControlMode.Full);  
		}  

		if (GUI.Button(new Rect(20,170,200,25), "(h)Hidden"))  
		{  
			Handheld.PlayFullScreenMovie("Test.mp4", Color.black, FullScreenMovieControlMode.Hidden);  
		}  

		if (GUI.Button(new Rect(20,250,200,25), "(h)Minimal"))  
		{  
			Handheld.PlayFullScreenMovie("Test.mp4", Color.black, FullScreenMovieControlMode.Minimal);  
		}  
	}
		
	public static MovieTest f_instance
	{
		get
		{
			return m_instance;
		}
	}

}
