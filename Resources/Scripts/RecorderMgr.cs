using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VRStandardAssets.Utils
{
    public class RecorderMgr : MonoBehaviour
    {

        public Dictionary<int, string> DicMovieUrl = new Dictionary<int, string>();
        public Dictionary<int, float> DicMovieScale = new Dictionary<int, float>();
        public Dictionary<int, string> DicMovieTitle = new Dictionary<int, string>();

        public GameObject go_SelectThree = null;
        public GameObject go_VideoPageThree = null;
        public GameObject LeftBtnThree = null;
        public GameObject RightBtnThree = null;
        public Text go_text;

        private List<int> Movielst = new List<int>();
        private static RecorderMgr m_instance;
        private int MovieId = 0;
        private int FullView = 0;
        private int ViewType = 0;
        private string Url = null;
        private string Title = null;
        private List<GameObject> m_MovieDataObj = new List<GameObject>();

        private int RecordMoviePage;
        private int RecordMovieTotalPage ;
        private List<GameObject> go_PrefabData = new List<GameObject>();
        

        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }
        // Update is called once per frame
        void Update()
        {

        }
        public void ClearMovieData(int i, bool Active)
        {
            if (m_MovieDataObj.Count > 0)
            {
                m_MovieDataObj[i].SetActive(true);
                m_MovieDataObj[i].GetComponent<BoxCollider>().enabled = Active;
                m_MovieDataObj[i].GetComponent<SpriteRenderer>().enabled = Active;
                m_MovieDataObj[i].GetComponent<SelectionMovie>().go_Scale.SetActive(Active);
                m_MovieDataObj[i].GetComponent<SelectionMovie>().go_MovieName.SetActive(Active);
                m_MovieDataObj[i].GetComponent<VRInteractiveItem>().go_Background.SetActive(Active);
            }
            go_SelectThree.SetActive(Active);
            go_VideoPageThree.SetActive(Active);
            if (go_PrefabData.Count > 0)
                go_PrefabData[0].SetActive(Active);
        }
        public void GetRecentData(int Movieid, float TimeScale, string Url,string title)
        {
            if (DicMovieUrl.ContainsKey(Movieid))
            {
                if (!Movielst.Contains(Movieid))
                {
                    Movielst.Add(Movieid);
                }
                DicMovieUrl[Movieid] = Url;
                DicMovieScale[Movieid] = TimeScale;
                DicMovieTitle[Movieid] = title;
            }
            else
            {
                DicMovieUrl.Add(Movieid, Url);
                DicMovieScale.Add(Movieid, TimeScale);
                DicMovieTitle.Add(Movieid, title);
                if (!Movielst.Contains(Movieid))
                {
                    Movielst.Add(Movieid);
                }
            }
        }
        public void SetPrefabData(int k)
        {
            ViewMenuMgr.f_instance.PrefabReset(false);
            for (int i = 0; i < 12; i++)
            {
                LocalDataMgr.f_instance.ClearMovieData(i, false);
                if (m_MovieDataObj.Count-1<i)
                {
                    m_MovieDataObj.Add(ViewMenuMgr.f_instance.CreatMenuBaseLine("Prefab/SelectionMovie", ViewMenuMgr.f_instance.go_RecordLst.transform));
                }
                ClearMovieData(i,false);
                if (i <= Movielst.Count-1- k * 12)
                {
                    if (DicMovieUrl.ContainsKey(Movielst[i+k*12]))
                    {
                        ClearMovieData(i, true);
                        StartCoroutine(Picture(i,k));
                    }
                }    
            }
            SetRecordData();    
        }
        private float scale(bool SetData = false, float widthX = 0, float widthY = 0)
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
        IEnumerator  Picture(int i,int k)
        {
            WWW www = new WWW(DicMovieUrl[Movielst[i+k*12]]);
            //Texture2D TypeImg;
            yield return www;
            Sprite sprite = new Sprite();
            sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100.0f);
            float Num1 = sprite.texture.width;
            float Num2 = sprite.texture.height;
            //Debug.Log(Num1 + "  " + Num2);
            float NumX = scale(true, Num1, 0);
            float NumY = scale(true, 0, Num2);
            float ChildX = scale(false, Num1, 0);
            float ChildY = scale(false, 0, Num2);    
            SelectionMovie m_SelectionMovie = m_MovieDataObj[i].GetComponent<SelectionMovie>();
            m_SelectionMovie.fscale = DicMovieScale[Movielst[i + k * 12]];
            m_SelectionMovie.MovieId = Movielst[i + k * 12];
            m_SelectionMovie.go_MovieName.GetComponent<Text>().text = DicMovieTitle[Movielst[i + k * 12]].ToString();
            m_MovieDataObj[i].GetComponent<SpriteRenderer>().sprite = sprite;
            m_MovieDataObj[i].GetComponent<RectTransform>().localScale = new Vector3(0.14f * NumX, 0.14f * NumY, 1f);

            m_MovieDataObj[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_MovieDataObj[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localPosition = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);

            m_MovieDataObj[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_MovieDataObj[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localPosition = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);

            m_MovieDataObj[i].GetComponent<BoxCollider>().size = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            m_MovieDataObj[i].GetComponent<BoxCollider>().center = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);
            float Num = DicMovieScale[Movielst[i + k * 12]]*1000;
            int Numtwo = Mathf.RoundToInt(Num);
            float Numtyhree = (float)Numtwo / 10;
            m_MovieDataObj[i].GetComponent<SelectionMovie>().go_Scale.GetComponent<Text>().text = Numtyhree+"%".ToString();       
        }
        public void SetRecordData()
        {
            for (int i = 0; i < 1; i++)
            {
                if(go_PrefabData.Count!=1)
                go_PrefabData.Add(ViewMenuMgr.f_instance.CreatMenuBaseLine("Prefab/VideoPage", go_VideoPageThree.transform));
            }
        }
        public void ClickLeftBtn()
        {
            if (RecordMovieTotalPage > 1)
            {
                if (RecordMoviePage > 1 && RecordMoviePage <= RecordMovieTotalPage)
                {
                    RecordMoviePage -= 1;
                    SetPrefabData(RecordMoviePage-1);
                    SetPage();
                }
                else
                    return;
            }
        }
        public void ClickRightBtn()
        {
            if (RecordMovieTotalPage > 1)
            {
                if (RecordMoviePage >= 1 && RecordMoviePage < RecordMovieTotalPage)
                {
                    RecordMoviePage += 1;
                    SetPrefabData(RecordMoviePage-1);
                    SetPage();
                }
                else
                    return;
            }
        }
        public void SetPageData()
        {
            if (LocalDataMgr.f_instance.f_MovieUrllst.Count > 0)
            {
                float Page = (float)Movielst.Count / (float)12;
                int OtherNum = Movielst.Count % 12;
                if (OtherNum == 0)
                {
                    RecordMovieTotalPage = (int)Page;
                }
                if (OtherNum > 0)
                {
                    RecordMovieTotalPage = (int)Page + 1;
                }
                if (RecordMovieTotalPage == 0)
                    RecordMovieTotalPage = 1;
                RecordMoviePage = 1;
            }
            else
            {
                RecordMovieTotalPage = 1;
                RecordMoviePage = 1;
            }
            SetRecordData();
            SetPage();
        }
        public void SetPage()
        {
            go_SelectThree.SetActive(true);
            go_VideoPageThree.SetActive(true);
            if (go_PrefabData.Count > 0)
                go_PrefabData[0].SetActive(true);
            int page = RecordMoviePage;
            int totalpage = RecordMovieTotalPage;
            Text m_goText = go_PrefabData[0].GetComponent<Text>();
            m_goText.text = page+"/"+ totalpage.ToString();
        }
        public static RecorderMgr f_instance
        {
            get
            {
                return m_instance;
            }
        }
        public int f_MovieId
        {
            get
            {
                return MovieId;
            }
            set
            {
                MovieId = value;
            }
        }
        public int f_FullView
        {
            get
            {
                return FullView;
            }
            set
            {
                FullView = value;
            }
        }
        public int f_ViewType
        {
            get
            {
                return ViewType;
            }
            set
            {
                ViewType = value;
            }
        }
        public string f_Url
        {
            get
            {
                return Url;
            }
            set
            {
                Url = value;
            }
        }
        public string f_Title
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }
        public int f_RecordMoviePage
        {
            get
            {
                return RecordMoviePage;
            }
        }
        public int f_RecordMovieTotalPage
        {
            get
            {
                return RecordMovieTotalPage;
            }
        }
        public List<GameObject> f_MovieDataObj
        {
            get
            {
                return m_MovieDataObj;
            }
        }
    }
}

