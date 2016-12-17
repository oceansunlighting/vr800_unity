using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRStandardAssets.Utils
{
    public class SCR_GUIReticle : MonoBehaviour
    {
        private static SCR_GUIReticle m_instance;
        public GameObject go_Play;
        public GameObject go_Lock;
        public GameObject go_Pause;
        public GameObject go_Menu;
        public GameObject go_MovieTag;

        private string[] ArrStr = new string[] { "Lock", "Pause", "Play", "Menu", "MovieTag" };
        private List<GameObject> go_Lst = new List<GameObject>();
        void Awake()
        {
            if (m_instance ==null)
            {
                m_instance = this;
            }
            go_Lst.Clear();
        }
        // Use this for initialization
        void Start()
        {
            go_Lst.Add(go_Play);
            go_Lst.Add(go_Lock);
            go_Lst.Add(go_Pause);
            go_Lst.Add(go_Menu);
            go_Lst.Add(go_MovieTag);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public string[] f_ArrStr
        {
            get
            {
                return ArrStr;
            }
        }
        public static SCR_GUIReticle f_instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}
