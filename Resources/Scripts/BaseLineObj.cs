using UnityEngine;
using System.Collections;

namespace VRStandardAssets.Utils
{
    public class BaseLineObj : MonoBehaviour
    {
        private static BaseLineObj m_instance;

        public GameObject go_BaseMenuLine; 
        public int id = 0;
        private bool Setactive = false;
        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }
        public static BaseLineObj f_instance
        {
            get
            {
                return m_instance;
            }
        }
        public bool f_Setactive
        {
            get
            {
                return Setactive;
            }
            set
            {
                Setactive = value;
            }        
        }
    }
}
