using UnityEngine;
using System.Collections;


namespace VRStandardAssets.Utils
{
    public class SelectionMovie : MonoBehaviour
    {
        private static SelectionMovie m_instance;
        public GameObject go_MovieName;
        public GameObject go_Scale;
        public int MovieId = 0;
        public float fscale= 0;
		private string MovieUrl=null;
		private string MovieName=null;
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
			MovieUrl = null;
			MovieName = null;
        }
        // Update is called once per frame
        void Update()
        {

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
        public static SelectionMovie f_instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}
