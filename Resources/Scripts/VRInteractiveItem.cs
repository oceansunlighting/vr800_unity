using System;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // This class should be added to any gameobject in the scene
    // that should react to input based on the user's gaze.
    // It contains events that can be subscribed to by classes that
    // need to know about input specifics to this gameobject.
    public class VRInteractiveItem : MonoBehaviour
    {
        public event Action OnOver;             // Called when the gaze moves over this object
        public event Action OnOut;              // Called when the gaze leaves this object
        public event Action OnClick;            // Called when click input is detected whilst the gaze is over this object.
        public event Action OnDoubleClick;      // Called when double click input is detected whilst the gaze is over this object.
        public event Action OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
        public event Action OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.

        public string VedioName = null;
        public GameObject BackFx = null;
        public GameObject go_Background = null;
        public GameObject go_Desc = null;
        public GameObject go_BackTitle = null;
        protected bool m_IsOver;
        public string BtnName = null;
        private int BtnId = 0;
        public bool IsOver
       
        {
            get { return m_IsOver; }              // Is the gaze currently over this object?
        }

        void Awake()
        {
        }
        void Start()
        {
            SetBtnName();
        }
        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        public void Over()
        {
            m_IsOver = true;

            if (OnOver != null)
                OnOver();
        }
        public void Out()
        {
            m_IsOver = false;

            if (OnOut != null)
                OnOut();
        }

        private void SetBtnName()
        {
            BtnName = gameObject.name;
        }
        public void Click()
        {
            if (OnClick != null)
                OnClick();
        }


        public void DoubleClick()
        {
            if (OnDoubleClick != null)
                OnDoubleClick();
        }


        public void Up()
        {
            if (OnUp != null)
                OnUp();
        }


        public void Down()
        {
            if (OnDown != null)
                OnDown();
        }
    }
}