using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRStandardAssets.Utils
{
    public class SCR_DefinitionMgr : MonoBehaviour
    {
        private static SCR_DefinitionMgr m_instance;

        private string m_str = null;
        private string[] m_arrstr = new string[] { "超清", "高清", "标清" };
        private List<GameObject> m_goDefinitionlst = new List<GameObject>(); 

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
            SetCurrentData();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCurrentData()
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_goDefinitionlst.Count - 1 < i)
                {
                    m_goDefinitionlst.Add(CreatMenuBaseLine("Prefab/MovieTagDefinition",gameObject.transform));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                CurrentData(m_goDefinitionlst[i].GetComponent<SCR_DefinitionChild>().go_image.GetComponent<SpriteRenderer>(), m_arrstr[i]);
                m_goDefinitionlst[i].GetComponent<SCR_DefinitionChild>().f_Str = m_arrstr[i];
            }
            SetCurrentActive(false);
            CurrentData(m_goDefinitionlst[3].GetComponent<SCR_DefinitionChild>().go_image.GetComponent<SpriteRenderer>(), m_arrstr[2]);
            m_goDefinitionlst[3].GetComponent<SCR_DefinitionChild>().f_Str = m_arrstr[2];
            m_goDefinitionlst[3].SetActive(true);
        }
        public void SetData(string str)
        {
            CurrentData(m_goDefinitionlst[3].GetComponent<SCR_DefinitionChild>().go_image.GetComponent<SpriteRenderer>(), str);
            m_goDefinitionlst[3].SetActive(true);
        }
        public SpriteRenderer CurrentData(SpriteRenderer spr,string str)
        {
            Texture2D texture2d = (Texture2D)Resources.Load("Picture/"+str);//更换为红色主题英雄角色图片  
            if (texture2d == null)
                Debug.LogError("aa");
            Sprite sp = Sprite.Create(texture2d, new Rect(0,0,texture2d.width,texture2d.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值  
            spr.sprite = sp;
            return spr;
        }
        public void SetCurrentActive(bool active)
        {
            for (int i = 0; i < 4; i++)
            {
                m_goDefinitionlst[i].SetActive(active);
            }
        }
        public GameObject CreatMenuBaseLine(string Path, Transform parent)
        {
            UnityEngine.Object go = Instantiate(Resources.Load(Path));
            GameObject goObj = (GameObject)go;
            goObj.transform.parent = parent;
            goObj.transform.localPosition = Vector3.zero;
            goObj.transform.localScale = Vector3.one;
            return goObj;
        }

        public string f_str
        {
            get
            {
                return m_str;
            }
            set
            {
                m_str = value;
            }
        }
        public static SCR_DefinitionMgr f_instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}
