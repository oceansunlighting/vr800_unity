using UnityEngine;
using System.Collections;
using System.Threading;

namespace VRStandardAssets.Utils
{
    public class SCR_Thread : MonoBehaviour
    {
        private static SCR_Thread m_instance;

        private Thread m_ScrThread;
        private WWW CurrentWWW = null;
        private string StarUrl = null;
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

        public void Current(string url)
        {
            StarUrl = url;
            m_ScrThread = new Thread(new ThreadStart(HelloWorld));
            m_ScrThread.Start();
        }

        public void HelloWorld()
        {
            StartCoroutine(Picture());
        }
        IEnumerator Picture()
        {
            CurrentWWW = new WWW(StarUrl);
            yield return CurrentWWW;
            if (CurrentWWW.isDone && CurrentWWW.error == null)
            {
                
            }
            SetProperty();
        }
        public void SetProperty()
        {
            if (m_ScrThread.IsAlive)
            {
                m_ScrThread.Abort();
            }
        }

        public static SCR_Thread f_instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}
