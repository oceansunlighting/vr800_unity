using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRStandardAssets.Utils
{
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VREyeRaycaster : MonoBehaviour
    {
        private static VREyeRaycaster m_instance;

        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

        public GameObject go_CameraCurrentMenu;
        public GameObject go_CameraClickFx;
        public GameObject go_VedioMenu;
        public GameObject go_GuiTickle;

        public GameObject go_Sphere;
        public GameObject go_DVedio;
        public GameObject go_LocalViewMenu;
        public GameObject go_EyeCountBox;



        public GameObject Sphere = null;
        public GameObject Vedio = null;
        public GameObject m_GoReceive = null;
        public GameObject m_goHitObj;
        public Text go_text = null;
        private string str = "";
        public VRInteractiveItem m_CurrentInteractible;
        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField] private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;        // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 500f;              // How far into the scene the ray is cast.
       
                      //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item
        private GUIText TextDebug;
        private float m_LastTime=-99;
        private float m_fscaleTime = 0;
        private float m_Fillamount=0;
        private float m_float = 0;
        private string m_strVedioName = ""; 
        private bool m_bCameraClick = false;
        private bool m_bEyeCountBox = false;
        private int udefinition = 0;

        private float m_Duration = 5f;
        private float m_CurrentLastTime = 0;
        public Dictionary<GameObject, bool> DicReceiveSetive = new Dictionary<GameObject, bool>();
        private Dictionary<GameObject, string> m_dReceiveData = new Dictionary<GameObject, string>();
        private Array[] m_arrControl;
        private Vector3 LocalScale;


        private string[] BtnGetFect = new string[] { "BaseMenuLine", "SelectionMovie(Clone)", "ClassificationMgr", "RecordMgr", "LocalDataMgr", "ReturnBtnMgr", "LeftBtn", "RightBtn" , "LeftBtntwo", "RightBtntwo","2DBtn","3DBtn","CloseBtn","TypeContent" , "Lock", "Pause","Play","Menu", "LeftBtnthree", "LeftBtnthree", "MovieTagDefinition(Clone)", "EyeCountBox" };
        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }

        void Awake()
        {
            if (m_instance==null)
            {
                m_instance = this;
            }
            DicReceiveSetive.Clear();
            m_dReceiveData.Clear();
            udefinition = 0;
            
        }
        void Start()
        {
            CreatManage();
            CreatSphere();
            if (m_dReceiveData[m_GoReceive].Contains("MediaPlayerCtrlSphere"))
            {
                m_strVedioName=MediaPlayerCtrlSphere.f_instance.m_strFileName;
            }
            else
            {
                m_strVedioName = MediaPlayerCtrl.f_instance.m_strFileName;
            }
            ReceiveSetive();
            TextDebug = gameObject.transform.GetComponent<GUIText>();
            //str = "";
            //go_text.text = str;
        }
        private void OnEnable()
        {
            m_VrInput.OnClick += HandleClick;
            m_VrInput.OnDoubleClick += HandleDoubleClick;
            m_VrInput.OnUp += HandleUp;
            m_VrInput.OnDown += HandleDown;
        }
        //接收状态；
        void ReceiveSetive()
        {
            DicReceiveSetive.Add(go_CameraCurrentMenu, false);
            DicReceiveSetive.Add(go_CameraClickFx, false);
            DicReceiveSetive.Add(go_VedioMenu, false);
            DicReceiveSetive.Add(go_GuiTickle, true);
            DicReceiveSetive.Add(go_Sphere, false);
            DicReceiveSetive.Add(go_DVedio, false);
            DicReceiveSetive.Add(go_LocalViewMenu, true);     
        }
        public void MenuSet()
        {
            foreach (GameObject go in DicReceiveSetive.Keys)
            {
                if (go.activeSelf != DicReceiveSetive[go])
                {
                    go.SetActive(DicReceiveSetive[go]);
                }
            }     
        }
    
        private void OnDisable ()
        {
            m_VrInput.OnClick -= HandleClick;
            m_VrInput.OnDoubleClick -= HandleDoubleClick;
            m_VrInput.OnUp -= HandleUp;
            m_VrInput.OnDown -= HandleDown;
        }

        private void Update()
        {
            EyeRaycast();
            MenuSetControl();
        }
      
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;
            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object       
                m_CurrentInteractible = interactible;
                m_goHitObj = hit.collider.gameObject;
                m_bCameraClick = true;
                DicReceiveSetive[go_CameraClickFx] = m_bCameraClick;
                if (m_goHitObj.name == BtnGetFect[21])
                {
                    if (!m_bEyeCountBox)
                    {
                        DicReceiveSetive[go_CameraCurrentMenu] = true;
                        m_bEyeCountBox = true;
                        MenuSet();
                        if (m_GoReceive == Vedio)
                        {
                            m_GoReceive.GetComponent<MediaPlayerCtrl>().go_slider.SetActive(true);
                        }
                        if (m_GoReceive == Sphere)
                        {
                            m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().go_slider.SetActive(true);
                        }
                        m_CurrentLastTime = Time.realtimeSinceStartup;
                        return;
                    }
                    return;
                }
                
                foreach (string str in BtnGetFect)
                {
                    if (m_CurrentInteractible.BtnName == str)
                    {
                        if (interactible.VedioName != "")
                        {
                            interactible.BackFx.SetActive(true);
                        }

                        else
                            m_goHitObj.GetComponent<Image>().color = Color.blue;
                    }
                }
                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                    interactible.Over(); 

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();
                m_LastInteractible = interactible;
                // Something was hit, set at the hit position.
                if (m_Reticle)
                    m_Reticle.SetPosition(hit);
                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
                MenuSet();
            }
            else
            {
                if (m_goHitObj != null)
                {
                    m_CurrentInteractible = m_goHitObj.GetComponent<VRInteractiveItem>();
                    if (m_CurrentInteractible.VedioName != "")
                    {
                        m_CurrentInteractible.BackFx.SetActive(false);
                    }
                    else
                        m_goHitObj.GetComponent<Image>().color = Color.white;
                }
                m_goHitObj = null;
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition();
                m_bCameraClick = false;
                DicReceiveSetive[go_CameraClickFx] = m_bCameraClick;
                MenuSet();
                if (Time.realtimeSinceStartup - m_LastTime >= 0.5)
                {
                    if (ViewMenuMgr.f_instance != null)
                    {
                        if (ViewMenuMgr.f_instance.f_MoviePlay)
                        {
                            ViewMenuMgr.f_instance.SetData();
                        }
                    }
                }
                if (m_bEyeCountBox)
                {
                    if (Time.realtimeSinceStartup - m_CurrentLastTime > m_Duration)
                    {
                        DicReceiveSetive[go_CameraCurrentMenu] = false;
                        m_bEyeCountBox = false;
                        MenuSet();
                        if (m_GoReceive == Vedio)
                        {
                            m_GoReceive.GetComponent<MediaPlayerCtrl>().go_slider.SetActive(false);
                        }
                        if (m_GoReceive == Sphere)
                        {
                            m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().go_slider.SetActive(false);
                        }
                    }
                }
            }

            CameraBarFx();
        }
        void CameraBarFx()
        {
            if (m_goHitObj == null)
            {
                DicReceiveSetive[go_CameraClickFx] = false;
                m_Fillamount = 0;
                m_bCameraClick = false;
                go_CameraClickFx.GetComponent<Image>().fillAmount = m_Fillamount;
                return;
            }
            if (m_bCameraClick)
            {
                
                if (m_Fillamount <= 1)
                    m_Fillamount += 0.01f;
                go_CameraClickFx.GetComponent<Image>().fillAmount = m_Fillamount;
                if (m_Fillamount >= 1 && m_float==0)
                {
                    m_float += 1;
					if (m_goHitObj.name == BtnGetFect[13] )
					{
						m_float = 0;
						m_Fillamount = 0;
						return;
					}
                    HandleDown();
                }
            }
            else
            {
                m_Fillamount = 0;
            }
        }
        private void MenuSetControl()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                DicReceiveSetive[go_CameraCurrentMenu] = true;
                DicReceiveSetive[go_LocalViewMenu] = true;
            }
        }
        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }

        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
            {
                m_CurrentInteractible.Up();
            }       
        }

        private void HandleDown()
        {
            if (m_goHitObj != null)
            {
                m_CurrentInteractible = m_goHitObj.GetComponent<VRInteractiveItem>();
                if (m_CurrentInteractible != null)
                {
                    SetVedioName();
                    for (int i = 0; i < BtnGetFect.Length; i++)
                    {
                        if (m_CurrentInteractible.BtnName == BtnGetFect[i])
                        {
                            List<Dictionary<GameObject, bool>> diclst = new List<Dictionary<GameObject, bool>>();
                            if (i == 0)
                            {
                                //UI界面，按钮菜单选项选中发送数据，初始页面为1;
                                int id = m_goHitObj.transform.parent.transform.gameObject.GetComponent<BaseLineObj>().id;
                                int limit = 12;
                                int page = 0;
                                ViewMenuMgr.f_instance.ClickMenuSendData(id, limit, page);
                                Dictionary<GameObject, bool> m_dicMenuBaseLine = new Dictionary<GameObject, bool>();
                                m_dicMenuBaseLine = MenuDic(m_dicMenuBaseLine, ViewMenuMgr.f_instance.f_lstMenuObj);
                            }
                            if (i == 1)
                            {
                                //点击电影按钮 ,播放电影
								SelectionMovie m_SelectionMovie = m_goHitObj.GetComponent<SelectionMovie>();
								int MovieId = m_SelectionMovie.MovieId;
								string MovieUrl = m_SelectionMovie.f_MovieUrl;
								string MovieName = m_SelectionMovie.f_MovieName;
                                m_fscaleTime = m_SelectionMovie.fscale;
								if (MovieUrl == null || MovieUrl == "") 
								{
									ViewMenuMgr.f_instance.SendMovieData (MovieId);
								}
								else 
								{
									ViewMenuMgr.f_instance.SetTypeContent (true);
                                    SetViewMgrActive(false);
                                    TypeContent.f_instance.SetMovie (MovieUrl, MovieName);
								}

                                Dictionary<GameObject, bool> m_dicSelectionMovie = new Dictionary<GameObject, bool>();
                                m_dicSelectionMovie = MenuDic(m_dicSelectionMovie, ViewMenuMgr.f_instance.f_lstVedioObj);

                                Dictionary<GameObject, bool> m_dicRecordMovie = new Dictionary<GameObject, bool>();
                                m_dicRecordMovie = MenuDic(m_dicRecordMovie, RecorderMgr.f_instance.f_MovieDataObj);
                            }
                            if (i >= 2 && i <= 5)
                            {
                                if (i == 2)
                                {
                                    //分类
                                    ClassificationMgr.f_instance.SetPrefabData();
                                    LocalDataMgr.f_instance.gameObject.GetComponent<VRInteractiveItem>().go_Desc.SetActive(false);
                                }
                                if (i == 3)
                                {
                                    //记录
                                    RecorderMgr.f_instance.SetPrefabData(0);
                                    RecorderMgr.f_instance.SetPageData();
                                    LocalDataMgr.f_instance.gameObject.GetComponent<VRInteractiveItem>().go_Desc.SetActive(false);
                                }
                                if (i == 4)
                                {
                                    //本地
                                    LocalDataMgr.f_instance.SetPrefabData();
                                    LocalDataMgr.f_instance.gameObject.GetComponent<VRInteractiveItem>().go_Desc.SetActive(true);
                                }
                                if (i == 5)
                                {
                                    SetViewActive(false);
                                }
                                Dictionary<GameObject, bool> m_dicTitleMenu = new Dictionary<GameObject, bool>();
                                m_dicTitleMenu = MenuDic(m_dicTitleMenu, ViewMenuMgr.f_instance.f_lstTitle);
                                str = RecorderMgr.f_instance.f_RecordMoviePage + " " + RecorderMgr.f_instance.f_RecordMovieTotalPage + "哈";
                                //go_text.text = str.ToString();
                                foreach (GameObject go in m_dicTitleMenu.Keys)
                                {
                                    str = go.name + " " + m_goHitObj.name + go.GetComponent<VRInteractiveItem>().go_BackTitle.activeSelf+ " 中";
                                    //go_text.text = str.ToString();
                                    if (go == m_goHitObj)
                                    {
                                        go.GetComponent<VRInteractiveItem>().go_BackTitle.SetActive(true);
                                    }
                                    else
                                    {
                                        go.GetComponent<VRInteractiveItem>().go_BackTitle.SetActive(false);
                                    }
                                }
                            }                         
                            if (i == 6)
                            {
                                //设置左侧页面切换；
                                ViewMenuMgr.f_instance.SetLeftClickPageData();
                            }                           
                            if (i == 7)
                            {
                                //设置右侧页面切换；
                                ViewMenuMgr.f_instance.SetRightClickPageData();
                            }
                            
                            if (i == 8)
                            {
                                //设置左侧页面切换;
                                ViewMenuMgr.f_instance.SetClickLeftBtntwo();
                            }
                            
                            if (i == 9)
                            {
                                //设置右侧页面切换;
                                ViewMenuMgr.f_instance.SetClickRightBtntwo();
                            }
                            if (i == 10)
                            {
								// 平面播放
								TypeContent.f_instance.GroundPlay ();
                            }
                            if (i == 11)
                            {
								// VR播放
								TypeContent.f_instance.ReticklePlay ();
                            }
                            if (i == 12)
                            {
								// 关闭子菜单
								ViewMenuMgr.f_instance.SetTypeContent (false);
                                ViewMenuMgr.f_instance.gameObject.SetActive(true);
                                SetViewMgrActive(true);
                                SetViewActive(true);
                            }
                            if (i == 13)
                            {
								// 
								
                            }
                            if (i == 14)
                            {
                                // Lock"
                                //if (m_GoReceive == Vedio)
                                //{
                                //    m_GoReceive.GetComponent<MediaPlayerCtrl>().Stop();
                                //}
                                //else if (m_GoReceive == Sphere)
                                //{
                                //    m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().Stop();
                                //}

                            }
                            if (i == 15)
                            {
                                // ,"Pause"
                                if (m_GoReceive == Vedio)
                                {
                                    m_GoReceive.GetComponent<MediaPlayerCtrl>().Pause();
                                }
                                else if (m_GoReceive == Sphere)
                                {
                                    m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().Pause();
                                }
                                if (SCR_GUIReticle.f_instance != null)
                                {
                                    SCR_GUIReticle.f_instance.go_Play.SetActive(true);
                                    SCR_GUIReticle.f_instance.go_Pause.SetActive(false);
                                }

                            }
                            if (i == 16)
                            {
                                // ,"Play"
                                if (m_GoReceive == Vedio)
                                {
                                    m_GoReceive.GetComponent<MediaPlayerCtrl>().Play();
                                }
                                else if(m_GoReceive == Sphere)
                                {
                                    m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().Play();
                                }
                                if (SCR_GUIReticle.f_instance != null)
                                {
                                    SCR_GUIReticle.f_instance.go_Play.SetActive(false);
                                    SCR_GUIReticle.f_instance.go_Pause.SetActive(true);
                                }
                            }
                            if (i == 17)
                            {
                                // ,"Menu"
                                SetViewActive(true);
                                SetViewMgrActive(true);
                                VREyeRaycaster.f_instance.go_EyeCountBox.SetActive(false);
                                DicReceiveSetive[go_CameraCurrentMenu] = false;
                                m_bEyeCountBox = false;
                                
                                if (m_GoReceive == Sphere)
                                {
                                    if (m_GoReceive.GetComponent<MediaPlayerCtrlSphere>() != null)
                                        m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().Stop();
                                    DicReceiveSetive[go_Sphere] = false;
                                }
                                if (m_GoReceive == Vedio)
                                {
                                    if (m_GoReceive.GetComponent<MediaPlayerCtrl>() != null)
                                        m_GoReceive.GetComponent<MediaPlayerCtrl>().Stop();
                                    DicReceiveSetive[go_DVedio] = false;
                                }
                                MenuSet();
                            }
                            if (i == 18)
                            {
                                //记录点击左标签;
                                RecorderMgr.f_instance.ClickLeftBtn();
                            }
                            if (i == 19)
                            {
                                //记录点击右标签;
                                RecorderMgr.f_instance.ClickRightBtn();
                            }
                            if (i == 20)
                            {
                                //MovieTagDefinition;
                                udefinition += 1;
                                if (udefinition == 1)
                                {
                                    SCR_DefinitionMgr.f_instance.SetCurrentActive(true);
                                }
                                if (udefinition == 2)
                                {
                                    string str = m_goHitObj.GetComponent<SCR_DefinitionChild>().f_Str;
                                    SCR_DefinitionMgr.f_instance.SetCurrentActive(false);
                                    SCR_DefinitionMgr.f_instance.SetData(str);
                                    udefinition = 0;
                                }
                            }
                            if (i==21)
                            {
                                // 此处未实现加载进度碰撞效果，只实现即刻效果。
                                SetCurrentMoviePlayActive(true);
                            }
                            MenuMgr(m_goHitObj, m_CurrentInteractible.BtnName, i, diclst);
                        } 
                        m_CurrentInteractible.Down();
                        m_float = 0;
                        m_Fillamount = 0;
                    } 
                }
                else
                {
                    DicReceiveSetive[go_CameraCurrentMenu] = false;
                    MenuSet();
                }
            }
        }
		public void SetViewActive(bool Active)
		{
            LocalScale = new Vector3(9, 7, 1);
            if (Active==true)
            {
                go_LocalViewMenu.GetComponent<RectTransform>().localScale = LocalScale;
            }
            else
            {
                go_LocalViewMenu.GetComponent<RectTransform>().localScale = Vector3.zero;
            }
		}
        public void SetViewMgrActive(bool Active)
        {
            if (Active == true)
                ViewMenuMgr.f_instance.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            else
                ViewMenuMgr.f_instance.gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        public void SetCurrentMoviePlayActive(bool Active)
        {
            DicReceiveSetive[go_CameraCurrentMenu] = Active;
            MenuSet();
        }

        private void SetVedioName()
        {
            if (m_dReceiveData[m_GoReceive].Contains("MediaPlayerCtrlSphere"))
            {
                m_GoReceive.GetComponent<MediaPlayerCtrlSphere>().m_strFileName = m_strVedioName;
            }
            else
            {
                m_GoReceive.GetComponent<MediaPlayerCtrl>().m_strFileName = m_strVedioName;
            }
        }
        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }

        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
            {
                m_CurrentInteractible.DoubleClick();
            }
        }

        //创建360°效果的球体预设；
         void CreatSphere()
        {
            UnityEngine.Object go = Instantiate(Resources.Load("Prefab/Sphere"));
            m_GoReceive = (GameObject)go;
            m_GoReceive.transform.parent = go_Sphere.transform;
            m_GoReceive.transform.localPosition = Vector3.zero;
            m_GoReceive.transform.localScale = Vector3.one;
            Sphere = (GameObject)go;
            m_dReceiveData.Add(Sphere, Sphere.GetComponent<MediaPlayerCtrlSphere>().ToString());
        }
        void Distroy()
        {
            Destroy(m_GoReceive);
        }
        //创建平面2D类型的电视效果物体；
         void CreatManage()
        {
            UnityEngine.Object go = Instantiate(Resources.Load("Prefab/VideoManager"));
            m_GoReceive = (GameObject)go;
            m_GoReceive.transform.parent = go_DVedio.transform;
            m_GoReceive.transform.localPosition = Vector3.zero;
            m_GoReceive.transform.localScale = Vector3.one;
            Vedio = (GameObject)go;
            m_dReceiveData.Add(Vedio, Vedio.GetComponent<MediaPlayerCtrl>().ToString());
        }
        public string f_strVedioName
        {
            get
            {
                return m_strVedioName;
            }
            set
            {
                m_strVedioName = value;
            }
        }
       public static VREyeRaycaster f_instance
        {
            get
            {
                return m_instance;
            }
        }
        public void MenuMgr(GameObject Click_Go,string Btn_Name,int m,List<Dictionary<GameObject,bool>> diclst)
        {   
            Dictionary<GameObject, bool> DicBtn = SetDicData(diclst, Click_Go);
            foreach (GameObject go in DicBtn.Keys)
            {
                if (go == Click_Go)
                {

                }
                else
                {

                }
            }
        }
        public Dictionary<GameObject,bool> MenuDic (Dictionary<GameObject,bool> dic,List<GameObject> lst)
        {
            if (lst.Count > 0)
            {
                foreach (GameObject go in lst)
                {
                    dic.Add(go, go.activeSelf);
                }
            }
            else
            {
                
            }
            return dic;
        }
        public Dictionary<GameObject, bool> SetDicData(List<Dictionary<GameObject,bool>> lst,GameObject Click_go)
        {
            Dictionary<GameObject, bool> dic = new Dictionary<GameObject, bool>();

            foreach (Dictionary<GameObject, bool> dr in lst)
            {
                foreach (GameObject go in dr.Keys)
                {
                    if (Click_go.name == go.name)
                    {
                        dic = dr;
                    }
                }
            }
            return dic;
        }
        public float f_fscaleTime
        {
            get
            {
                return m_fscaleTime;
            }
        }
    }
}