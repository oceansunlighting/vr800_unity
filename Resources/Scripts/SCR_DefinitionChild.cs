using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRStandardAssets.Utils
{
    public class SCR_DefinitionChild : MonoBehaviour
    {
        private static SCR_DefinitionChild m_instance;

        public GameObject go_image;
        private string m_Str;

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

        // Update is called once per frame
        void Update()
        {

        }

        public string f_Str
        {
            get
            {
                return m_Str;
            }
            set
            {
                m_Str = value;
            }
        }
        public static SCR_DefinitionChild f_instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}
