using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using System.Threading;

namespace VRStandardAssets.Utils
{
    enum GetSaveType
    {
        Download = 0,
        LocalSave =1,
    }
    public class ViewMenuMgr : MonoBehaviour
    {

        private static ViewMenuMgr m_instance;

        public GameObject go_RecordMgr;
        public GameObject go_ClassificationMgr;
        public GameObject go_LocalDataNgr;


        public GameObject go_MenuLine = null;
        public GameObject go_VedioList = null;
        public GameObject go_RecordLst = null;
        public GameObject go_LocalDatalst = null;

        public GameObject go_LeftBtn = null;
        public GameObject go_RightBtn = null;
        public GameObject go_Page = null;
        public GameObject go_select = null;

        public GameObject go_LeftBtntwo = null;
        public GameObject go_RightBtntwo = null;
        public GameObject go_Pagetwo = null;
        public GameObject go_selecttwo = null;
		public GameObject go_TypeContent = null;

        public GameObject ReturnBtn = null;
        private List<GameObject> m_lstMenuObj = new List<GameObject>();
        private List<GameObject> m_lstVedioObj = new List<GameObject>();
        private List<GameObject> m_lstTitle = new List<GameObject>();
        private GameObject m_goMenuLine = null;
        private Text m_Text;
        private Text m_CurrentText = null;
        private string m_Currentstr = null;
        private GameObject m_goVideoPage = null;


        private int MenuDataId;
        private int MenuDataLimit = 12;
        private int menuDataPage;

        private int LocalDataPage;
        private int LocalDatatotalPage;
        private GameObject m_LocalDataPageObj;


        private int Totalpages;
        private int MoviePlayId;
        private bool SetEnable = true;
        private bool MoviePlay = false;
        private JSON_Reader Json;
        private JSON_MovieReader JsonMovie;
        private Vector3 m_scale = Vector3.one;

        private string CurrentData = null;
        private object data;

        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            m_lstMenuObj.Clear();
            m_lstVedioObj.Clear();
            MoviePlay = false;
        }
        // Use this for initialization
        void Start()
        {
           
        }
        void OnEnable()
        {
            MenuDataId = 10;
            MenuDataLimit = 12;
            menuDataPage = 1;
            SetEnable = true;
            for (int i = 0; i < 12; i++)
            {
                if(RecorderMgr.f_instance!=null)
                RecorderMgr.f_instance.ClearMovieData(i, false);
                if(LocalDataMgr.f_instance!=null)
                LocalDataMgr.f_instance.ClearMovieData(i, false);
            }
            go_selecttwo.SetActive(false);
            go_Pagetwo.SetActive(false);
            m_lstTitle.Add(go_ClassificationMgr);
            m_lstTitle.Add(go_RecordMgr);
            m_lstTitle.Add(go_LocalDataNgr);
            m_lstTitle.Add(ReturnBtn);
            UI_server_sideData.SendData(MenuDataId, MenuDataLimit, menuDataPage-1);
        }
        // Update is called once per frame
		public void SetTypeContent( bool Active)
		{
			go_TypeContent.SetActive (Active);
		}
        public void ClickMenuSendData(int id,int limit,int Idpage)
        {
            SetEnable = false;
            if (id == MenuDataId)
            {
                Idpage = menuDataPage-1;            
            }
            else
            {
                MenuDataId = id;
                menuDataPage = 1;
                Idpage = menuDataPage - 1;
            }
            limit = MenuDataLimit;
            UI_server_sideData.SendData(id, limit, Idpage);
        }
        public void SetPageBtnSendData(int page)
        {
            int id = MenuDataId;
            int limit = MenuDataLimit;
            int IdPage = page - 1;
            UI_server_sideData.SendData(id, limit, IdPage);
        }

        public void SetLeftClickPageData()
        {
            if (menuDataPage >= 2 & menuDataPage <= Totalpages)
            {
                menuDataPage -= 1;
                SetPageBtnSendData(menuDataPage);
            }
            else
            {
                menuDataPage = 1;
                PageSet();
            }
        }
        public void SetRightClickPageData()
        {
            if (menuDataPage >= 1 & menuDataPage < Totalpages)
            {
                menuDataPage += 1;
                SetPageBtnSendData(menuDataPage);
            }
            else
            {
                menuDataPage = Totalpages;
                PageSet();
            }
        }
        public void ReceiveData()
        {

            Json = new JSON_Reader();
            Json = JsonUtility.FromJson<JSON_Reader>(UI_server_sideData.JsonStr);
            if (SetEnable)
            {
                MenuDataId = Json.data.channelList[0].id;
                if (m_lstMenuObj.Count <= Json.data.channelList.Length)
                {
                    SetMenuObj();
                }
                if (m_lstVedioObj.Count <= Json.data.videoList.Length)
                {
                    SetMovieObj();
                }
                SetEnable = false;
            }    
            MovieSet();
            MenuSet();
            PageSet();
            ResetScale();
        }
        public void ResetScale()
        {
            gameObject.GetComponent<RectTransform>().localScale = m_scale;
        }
        public void MenuSet()
        {
            for (int i = 0; i < Json.data.channelList.Length; i++)
            {
                GameObject go = m_lstMenuObj[i].GetComponent<BaseLineObj>().go_BaseMenuLine;
                m_lstMenuObj[i].GetComponent<BaseLineObj>().id = Json.data.channelList[i].id;
                m_lstMenuObj[i].SetActive(true);
                if (m_lstMenuObj[i].GetComponent<BaseLineObj>().id == MenuDataId)
                {
                    m_lstMenuObj[i].GetComponent<BaseLineObj>().f_Setactive = true;
                    m_lstMenuObj[i].GetComponent<BaseLineObj>().go_BaseMenuLine.GetComponent<Text>().color = new Color((float)255 / 255, (float)51/255, (float)15 / 255, (float)255 / 255);
                }
                else
                {
                    m_lstMenuObj[i].GetComponent<BaseLineObj>().f_Setactive = false;
                    m_lstMenuObj[i].GetComponent<BaseLineObj>().go_BaseMenuLine.GetComponent<Text>().color = Color.white;
                }
                string title = Json.data.channelList[i].title;
                go.GetComponent<Text>().text = title;
            }
            m_scale = gameObject.GetComponent<RectTransform>().localScale;
        }
        public void MovieSet()
        {        

            for (int i = 0; i < Json.data.videoList.Length; i++)
            {
                m_lstVedioObj[i].SetActive(true);
                VRInteractiveItem m_VRInteractiveItem = m_lstVedioObj[i].GetComponent<VRInteractiveItem>();
                SelectionMovie m_SelectionMovie = m_lstVedioObj[i].GetComponent<SelectionMovie>();
                m_SelectionMovie.MovieId = Json.data.videoList[i].id;
                if (Json.data.videoList[i].img != null && Json.data.videoList[i].img != "")
                {
                    StartCoroutine(SetData(i));
                }
            }
            for (int i = 0; i < Json.data.videoList.Length; i++)
            {
                m_lstVedioObj[i].SetActive(true);
                VRInteractiveItem m_VRInteractiveItem = m_lstVedioObj[i].GetComponent<VRInteractiveItem>();
                SelectionMovie m_SelectionMovie = m_lstVedioObj[i].GetComponent<SelectionMovie>();
                m_SelectionMovie.MovieId = Json.data.videoList[i].id;
                if (Json.data.videoList[i].img != null && Json.data.videoList[i].img != "")
                {
                    StartCoroutine(Picture(i,GetSaveType.Download));
                }
            }
        }
        public void PageSet()
        {
            string page = menuDataPage.ToString();
			Totalpages = Json.data.totalPages;
			VideoPage(page,Totalpages);
        }
        private void SetMenuObj()
        {
            for (int i = 0; i < Json.data.channelList.Length ;i++)
            {
                if (m_lstMenuObj.Count - 1 < i)
                {
                    m_lstMenuObj.Add(CreatMenuBaseLine("Prefab/BaseLineObj", go_MenuLine.transform));
                }
                m_lstMenuObj[i].SetActive(false);
                GameObject go = m_lstMenuObj[i].GetComponent<BaseLineObj>().go_BaseMenuLine;
            }
        
        }
        public void SetMovieObj()
        {
            for (int i = 0; i < Json.data.videoList.Length; i++)
            {
                if (m_lstVedioObj.Count - 1 < i)
                {
                    m_lstVedioObj.Add(CreatMenuBaseLine("Prefab/SelectionMovie", go_VedioList.transform));
                }
                m_lstVedioObj[i].SetActive(false);
            }
        }
		public void VideoPage(string page,int totalpage)
        {
            if (m_goVideoPage == null)
            {
                m_goVideoPage = CreatMenuBaseLine("Prefab/BaseLineObj", go_Page.transform);
            }
            m_goVideoPage.SetActive(true);
            go_Page.SetActive(true);
            go_select.SetActive(true);
            BaseLineObj menubase = m_goVideoPage.GetComponent<BaseLineObj>();
            VRInteractiveItem m_VRInteractiveItem = menubase.go_BaseMenuLine.GetComponent<VRInteractiveItem>();
            menubase.go_BaseMenuLine.GetComponent<BoxCollider>().enabled = false;
            menubase.go_BaseMenuLine.GetComponent<Text>().text = page+ "/"+ totalpage.ToString();
            menubase.go_BaseMenuLine.GetComponent<Text>().fontSize = 1;
            menubase.go_BaseMenuLine.GetComponent<Text>().color = Color.white;
            m_VRInteractiveItem.go_Background.SetActive(false);
            m_VRInteractiveItem.BackFx.SetActive(false);       
        }
        IEnumerator SetData(int i)
        {
            Sprite sprite = m_lstVedioObj[i].GetComponent<SpriteRenderer>().sprite;
            float Num1 = sprite.texture.width;
            float Num2 = sprite.texture.height;
            float NumX = scale(true, Num1, 0);
            float NumY = scale(true, 0, Num2);
            float ChildX = scale(false, Num1, 0);
            float ChildY = scale(false, 0, Num2);
            SelectionMovie m_SelectionMovie = m_lstVedioObj[i].GetComponent<SelectionMovie>();
            m_SelectionMovie.go_MovieName.GetComponent<Text>().text = Json.data.videoList[i].title.ToString();
            m_lstVedioObj[i].GetComponent<SpriteRenderer>().sprite = sprite;
            m_lstVedioObj[i].GetComponent<RectTransform>().localScale = new Vector3(0.14f * NumX, 0.14f * NumY, 1f);

            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localScale = new Vector3(2f * ChildX, 2 * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localPosition = new Vector3(3.5f * ChildX, 2.6f * ChildY, 1f);

            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localPosition = new Vector3(3.5f * ChildX, 2.6f * ChildY, 1f);

            m_lstVedioObj[i].GetComponent<BoxCollider>().size = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<BoxCollider>().center = new Vector3(3.5f * ChildX, 2.6f * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<SelectionMovie>().go_Scale.SetActive(false);
            m_lstVedioObj[i].SetActive(true);
            yield return true;
        }
        IEnumerator Picture(int i,GetSaveType getType)
        {
            WWW www = new WWW(Json.data.videoList[i].img);
            Texture2D TypeImg;
            yield return www;
            //下载图片电影到指定路径;
            //Texture2D Img = null;
            //bool DownloadOK = false;
            //if (www.isDone && www.error == null)
            //{
            //    switch (getType)
            //    {
            //        case GetSaveType.Download:
            //            {
            //                Img = www.texture;
            //                TypeImg = Img;
            //                Debug.Log(TypeImg.width + "标记1  " + TypeImg.height);
            //                break;
            //            }
            //        case GetSaveType.LocalSave:
            //            Img = www.texture;
            //            TypeImg = Img;
            //            Debug.Log(TypeImg.width + " 标记2 " + TypeImg.height);
            //            break;
            //        default:
            //            TypeImg = null;
            //            break;
            //    }
            //    if (TypeImg != null)
            //    {
            //        byte[] data = TypeImg.EncodeToPNG();
            //        string PngName = Json.data.videoList[i].id + ".png";
            //        File.WriteAllBytes(Application.streamingAssetsPath + "/" + PngName, data);
            //        Debug.Log(Application.streamingAssetsPath + "/"+ PngName + "标记3 ");
            //        DownloadOK = true;
            //    }
            //}
            //本地缓存图片调整;
            m_lstVedioObj[i].SetActive(false);
            StartCoroutine(SetPicture(i, www));
           
        }
        IEnumerator SetPicture(int i, WWW www)
        {
            Sprite sprite = new Sprite();
            sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100.0f);
            float Num1 = sprite.texture.width;
            float Num2 = sprite.texture.height;
            float NumX = scale(true, Num1, 0);
            float NumY = scale(true, 0, Num2);
            float ChildX = scale(false, Num1, 0);
            float ChildY = scale(false, 0, Num2);
            SelectionMovie m_SelectionMovie = m_lstVedioObj[i].GetComponent<SelectionMovie>();
            m_SelectionMovie.go_MovieName.GetComponent<Text>().text = Json.data.videoList[i].title.ToString();
            m_lstVedioObj[i].GetComponent<SpriteRenderer>().sprite = sprite;
            m_lstVedioObj[i].GetComponent<RectTransform>().localScale = new Vector3(0.14f * NumX, 0.14f * NumY, 1f);

            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localPosition = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);

            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localPosition = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);

            m_lstVedioObj[i].GetComponent<BoxCollider>().size = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<BoxCollider>().center = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);
            m_lstVedioObj[i].GetComponent<SelectionMovie>().go_Scale.SetActive(false);
            m_lstVedioObj[i].SetActive(true);
            yield return true;
        }
        private float scale(bool SetData = false, float widthX = 0,float widthY = 0)
        {
            float Num3 = 0;
            float DataX = 750;
            float DataY = 420;
            if (SetData)
            {
                if (widthX != 0)
                {
                    Num3 = widthX / DataX;
                    float Num4 = 1 / Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
                if (widthY != 0)
                {
                    Num3 = widthY / DataY;
                    float Num4 = 1 / Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
            }
            else
            {
                if (widthX != 0)
                {
                    Num3 = widthX / DataX;
                    float Num4 = Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
                if (widthY != 0)
                {
                    Num3 = widthY / DataY;
                    float Num4 = Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
            }
            return Num3;
        }
        public GameObject CreatMenuBaseLine(string Path,Transform parent)
        {      
            UnityEngine.Object go = Instantiate(Resources.Load(Path));
            GameObject goObj = (GameObject)go;
            goObj.transform.parent = parent;
            goObj.transform.localPosition = Vector3.zero;
            goObj.transform.localScale = Vector3.one;
            return goObj;
        }
        public static ViewMenuMgr f_instance
        {
            get
            {
                return m_instance;
            }
        }
        public void PrefabReset(bool Active)
        {
            for (int i = 0; i < m_lstMenuObj.Count; i++)
            {
                m_lstMenuObj[i].SetActive(Active);
            }
            for (int i = 0; i < m_lstVedioObj.Count; i++)
            {
                m_lstVedioObj[i].SetActive(Active);
            }
            go_Page.SetActive(Active);
            go_select.SetActive(Active);
        }
        public void SendMovieData(int MovieId)
        {
            MoviePlayId = MovieId;
            MoviePlay = false;
            UI_MoviePlayData.SendData(MovieId);
        }
        public void SetMoviePlay()
        {
            JsonMovie = JsonUtility.FromJson<JSON_MovieReader>(UI_MoviePlayData.JsonStr);
            int FullView = JsonMovie.data.fullView;
            //播放记录
            RecorderMgr.f_instance.f_FullView = FullView;
            RecorderMgr.f_instance.f_MovieId = MoviePlayId;
            RecorderMgr.f_instance.f_ViewType = JsonMovie.data.viewType;
            RecorderMgr.f_instance.f_Url = JsonMovie.data.img;
            RecorderMgr.f_instance.f_Title = JsonMovie.data.title;
            if (RecorderMgr.f_instance.f_Url==null|| RecorderMgr.f_instance.f_Url=="")
            {
                MoviePlay = false;
                return;
            }           
            int[] FullType = new int[] {0,1 };
            if (FullView == FullType[0])
            {
                //0 播放非全景;
                SetPlayPlanarFilm(JsonMovie.data.playUrl);
            }
            if (FullView == FullType[1])
            {
                //1 播放全景;
                //viewType：1:左右360   2:上下360   3:左右180    4:上下180   5:全屏360   6:全屏180
                int[] viewType = new int[] { 1, 2, 3, 4, 5, 6 };
                int view = JsonMovie.data.viewType;
                if (view == viewType[0])
                {

                }
                if (view == viewType[1])
                {

                }
                if (view == viewType[2])
                {

                }
                if (view == viewType[3])
                {

                }
                if (view == viewType[4])
                {

                }
                if (view == viewType[5])
                {

                }
                SetPlayWholeScene(JsonMovie.data.playUrl);
            }
            
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
                if (VREyeRaycaster.f_instance.f_fscaleTime != 0)
                {
                    MediaPlayerCtrl.f_instance.f_bSetValue = true;
                    MediaPlayerCtrl.f_instance.f_fGetTimeScale = VREyeRaycaster.f_instance.f_fscaleTime;
                }
                else
                {
                    MediaPlayerCtrl.f_instance.f_bSetValue = false;
                    MediaPlayerCtrl.f_instance.f_fGetTimeScale = VREyeRaycaster.f_instance.f_fscaleTime;
                }
                MediaPlayerCtrl.f_instance.f_SpecialTime = Time.realtimeSinceStartup;
                MediaPlayerCtrl.f_instance.Load(url);
                if (SCR_GUIReticle.f_instance != null)
                {
                    SCR_GUIReticle.f_instance.go_Play.SetActive(false);
                    SCR_GUIReticle.f_instance.go_Pause.SetActive(true);
                }
                MoviePlay = true;
                VREyeRaycaster.f_instance.SetViewActive(false);
                VREyeRaycaster.f_instance.go_EyeCountBox.SetActive(true);
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
                if (VREyeRaycaster.f_instance.f_fscaleTime != 0)
                {
                    MediaPlayerCtrlSphere.f_instance.f_bSetValue = true;
                    MediaPlayerCtrlSphere.f_instance.f_fGetTimeScale = VREyeRaycaster.f_instance.f_fscaleTime;
                }
                else
                {
                    MediaPlayerCtrlSphere.f_instance.f_bSetValue = false;
                    MediaPlayerCtrlSphere.f_instance.f_fGetTimeScale = VREyeRaycaster.f_instance.f_fscaleTime;
                }
                MediaPlayerCtrlSphere.f_instance.f_SpecialTime = Time.realtimeSinceStartup;
                MediaPlayerCtrlSphere.f_instance.Load(url);
                if (SCR_GUIReticle.f_instance != null)
                {
                    SCR_GUIReticle.f_instance.go_Play.SetActive(false);
                    SCR_GUIReticle.f_instance.go_Pause.SetActive(true);
                }
                MoviePlay = true;
                VREyeRaycaster.f_instance.SetViewActive(false);
                VREyeRaycaster.f_instance.go_EyeCountBox.SetActive(true);
            }
            VREyeRaycaster.f_instance.f_strVedioName = url;
        }
        public  void CloseMenu()
        {

        }
        public void ReSetMenu()
        {
                       
        }
        public void SetData()
        {
            float scale = 0;
            if (MoviePlayId != 0)
            {
                if (VREyeRaycaster.f_instance.m_GoReceive == VREyeRaycaster.f_instance.Sphere)
                {
                    if (SeekBarCtrl.f_instance!=null)
                        scale = SeekBarCtrl.f_instance.m_srcSlider.value;
                }
                if (VREyeRaycaster.f_instance.m_GoReceive == VREyeRaycaster.f_instance.Vedio)
                {
                    if (SeekBarControl.f_instance != null)
                        scale = SeekBarControl.f_instance.m_srcSlider.value;
                }
                RecorderMgr.f_instance.GetRecentData(RecorderMgr.f_instance.f_MovieId, scale, RecorderMgr.f_instance.f_Url, RecorderMgr.f_instance.f_Title);
            }
        }
        public void SetClickLeftBtntwo()
        {
            if (LocalDataPage<=1)
            {
                return;
            }
            if (LocalDataPage>1&&LocalDataPage<LocalDatatotalPage)
            {
                LocalDataPage -= 1;
            }
            if (LocalDataPage == LocalDatatotalPage)
            {
                LocalDataPage -= 1;
            }
            LocalPageSet();
        }
        public void SetClickRightBtntwo()
        {
            if (LocalDataPage <= 1&&LocalDataPage < LocalDatatotalPage)
            {
                LocalDataPage += 1;
            }
            if (LocalDataPage > 1 && LocalDataPage < LocalDatatotalPage)
            {
                LocalDataPage += 1;
            }
            if (LocalDataPage == LocalDatatotalPage)
            {
                return;
            }
            LocalPageSet();
        }
        public void SetCurrentLocalData()
        {
            if (LocalDataMgr.f_instance.f_MovieUrllst.Count > 0)
            {
                float Page = (float)LocalDataMgr.f_instance.f_MovieUrllst.Count / (float)12;
                int OtherNum = LocalDataMgr.f_instance.f_MovieUrllst.Count % 12;
                if (OtherNum == 0)
                {
                    LocalDatatotalPage = (int)Page;
                }
                if (OtherNum > 0)
                {
                    LocalDatatotalPage = (int)Page + 1;
                }
                LocalDataPage = 1;
            }
            else
            {
                LocalDatatotalPage = 1;
                LocalDataPage = 1;
            }
            m_Text = LocalDataMgr.f_instance.f_LocalDataPage[0].GetComponent<Text>();
            LocalPageSet();   
        }
        public void LocalPageSet()
        {
            m_Text.text = LocalDataPage + "/" + LocalDatatotalPage.ToString();
            ViewMenuMgr.f_instance.go_selecttwo.SetActive(true);
            ViewMenuMgr.f_instance.go_Pagetwo.SetActive(true);
            if (LocalDataMgr.f_instance.f_LocalDataPage.Count >= 1)
                LocalDataMgr.f_instance.f_LocalDataPage[0].SetActive(true);

        }
        public void CloseMovie()
        {

        }
        public  int f_MenuDataId
        {
            get
            {
                return MenuDataId;
            }
            set
            {
                MenuDataId = value;
            }
        }
        public int f_MenuDataLimit
        {
            get
            {
                return MenuDataLimit;
            }
            set
            {
                MenuDataLimit = value;
            }
        }
        public int f_menuDataPage
        {
            get
            {
                return menuDataPage;
            }
            set
            {
                menuDataPage = value;
            }
        }
        public int f_Totalpages
        {
            get
            {
                return Totalpages;
            }
            set
            {
                Totalpages = value;
            }
        }

        public bool f_SetEnable
        {
            get
            {
                return SetEnable;
            }
            set
            {
                SetEnable = value;
            }
        }
        public bool f_MoviePlay
        {
            get
            {
                return MoviePlay;
            }
            set
            {
                MoviePlay = value;
            }
        }
        public int f_LocalDataPage
        {
            get
            {
                return LocalDataPage;
            }
            set
            {
                LocalDataPage = value;
            }
        }
        public int f_LocalDatatotalPage
        {
            get
            {
                return LocalDatatotalPage;
            }
        }
        public GameObject f_LocalDataPageObj
        {
            get
            {
                return m_LocalDataPageObj;
            }
        }
        public List<GameObject> f_lstMenuObj
        {
            get
            {
                return m_lstMenuObj;
            }
        }
        public List<GameObject> f_lstVedioObj
        {
            get
            {
                return m_lstVedioObj;
            }
        }
        public List<GameObject> f_lstTitle
        {
            get
            {
                return m_lstTitle;
            }
        }
    }
}
