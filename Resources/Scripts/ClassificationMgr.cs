using UnityEngine;
using System;
using System.IO;


namespace VRStandardAssets.Utils
{
    public class ClassificationMgr : MonoBehaviour
    {
       
        private static ClassificationMgr m_instance;
        

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
        // Update is called once per frame
        void Update()
        {

        }

        void OnDisable()
        {

        }
        public void SetPrefabData()
        {
            for (int i = 0; i < 12; i++)
            {
                RecorderMgr.f_instance.ClearMovieData(i,false);
                LocalDataMgr.f_instance.ClearMovieData(i, false);
            }
            ViewMenuMgr.f_instance.f_MenuDataId = 10;
            ViewMenuMgr.f_instance.f_MenuDataLimit = 12;
            ViewMenuMgr.f_instance.f_menuDataPage = 1;
            ViewMenuMgr.f_instance.f_SetEnable = false;
            UI_server_sideData.SendData(ViewMenuMgr.f_instance.f_MenuDataId, ViewMenuMgr.f_instance.f_MenuDataLimit, ViewMenuMgr.f_instance.f_menuDataPage - 1);
        }
        public static ClassificationMgr f_instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}
