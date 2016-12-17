using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRStandardAssets.Utils
{
	public class TypeContent : MonoBehaviour {

		private static TypeContent  m_instance;

		public GameObject go_2DBtn;
		public GameObject go_3DBtn;
		public GameObject go_CloseBtn;

		private MediaPlayerCtrl m_MediaPlayerCtrl;
		private MediaPlayerCtrlSphere m_MediaPlayerCtrlSphere;
		private string MovieUrl = null;
		private string MovieName = null;
		private Text m_Text;
		// Use this for initialization
		void Awake()
		{
			if (m_instance == null) 
			{
				m_instance = this;
			}
		}

		void Start () 
		{

		}

		// Update is called once per frame
		void Update () 
		{

		}

		public void SetMovie(string sMovieUrl,string sMovieName)
		{
			MovieUrl = sMovieUrl;
			MovieName = sMovieName;
		}
		public void GroundPlay()
		{
			SetCloseMenu ();
			SetPlayPlanarFilm (MovieUrl);
		}

		public void ReticklePlay()
		{
			SetCloseMenu ();
			SetPlayWholeScene (MovieUrl);
		}

		public void SetPlayPlanarFilm(string url)
		{
			MediaPlayerCtrlSphere.f_instance.Stop();
			VREyeRaycaster.f_instance.DicReceiveSetive[VREyeRaycaster.f_instance.go_Sphere] = false;
			VREyeRaycaster.f_instance.DicReceiveSetive[VREyeRaycaster.f_instance.go_DVedio] = true;
			VREyeRaycaster.f_instance.MenuSet();
			VREyeRaycaster.f_instance.m_GoReceive = VREyeRaycaster.f_instance.Vedio;
			if (VREyeRaycaster.f_instance.m_CurrentInteractible.VedioName != null)
			{
				MediaPlayerCtrl.f_instance.MovieName(url);
                VREyeRaycaster.f_instance.SetViewActive(false);
                VREyeRaycaster.f_instance.go_EyeCountBox.SetActive(true);
                if (SCR_GUIReticle.f_instance != null)
                {
                    SCR_GUIReticle.f_instance.go_Play.SetActive(false);
                    SCR_GUIReticle.f_instance.go_Pause.SetActive(true);
                }
            }
			VREyeRaycaster.f_instance.f_strVedioName = url;
		}
		public void SetPlayWholeScene(string url)
		{
			MediaPlayerCtrl.f_instance.Stop();
			VREyeRaycaster.f_instance.DicReceiveSetive[VREyeRaycaster.f_instance.go_Sphere] = true;
			VREyeRaycaster.f_instance.DicReceiveSetive[VREyeRaycaster.f_instance.go_DVedio] = false;
			VREyeRaycaster.f_instance.MenuSet();
			VREyeRaycaster.f_instance.m_GoReceive = VREyeRaycaster.f_instance.Sphere;
			if (VREyeRaycaster.f_instance.m_CurrentInteractible.VedioName != null)
			{
				MediaPlayerCtrlSphere.f_instance.MovieName (url);
                VREyeRaycaster.f_instance.SetViewActive(false);
                VREyeRaycaster.f_instance.go_EyeCountBox.SetActive(true);
                if (SCR_GUIReticle.f_instance != null)
                {
                    SCR_GUIReticle.f_instance.go_Play.SetActive(false);
                    SCR_GUIReticle.f_instance.go_Pause.SetActive(true);
                }
            }
			VREyeRaycaster.f_instance.f_strVedioName = url;
		}
		public void SetCloseMenu()
		{
			gameObject.SetActive (false);
        }	

		public string f_MovieUrl
		{
			get
			{
				return MovieUrl;
			}
			set
			{
				MovieUrl = value;
			}
		}
		public string f_MovieName
		{
			get
			{
				return MovieName;
			}
			set
			{
				MovieName = value;
			}
		}
		public static TypeContent f_instance
		{
			get
			{
				return m_instance;
			}
		}
	}
}
