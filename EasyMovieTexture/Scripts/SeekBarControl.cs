using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class SeekBarControl : MonoBehaviour ,IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler{

	private static SeekBarControl m_instance;
	public MediaPlayerCtrl m_srcVideo;
	public Slider m_srcSlider;
	public float m_fDragTime = 0.2f;
    public GameObject go_Text;
    public GameObject go_TotalTime;

	bool m_bActiveDrag = true;
	bool m_bUpdate = true;

	float m_fDeltaTime = 0.0f;
	float m_fLastValue = 0.0f;
	float m_fLastSetValue = 0.0f;

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (m_bActiveDrag == false) {
			m_fDeltaTime += Time.deltaTime;
			if (m_fDeltaTime > m_fDragTime) {
				m_bActiveDrag = true;
				m_fDeltaTime = 0.0f;
				//if(m_fLastSetValue != m_fLastValue)
				//	m_srcVideo.SetSeekBarValue (m_fLastValue);

			}
		}



		if (m_bUpdate == false)
			return;

		if (m_srcVideo != null) {

			if (m_srcSlider != null)
            {
				m_srcSlider.value = m_srcVideo.GetSeekBarValue();
                SetTime(m_srcVideo.GetDuration()* m_srcVideo.GetSeekBarValue(), m_srcVideo.GetDuration());
			}

		}
	}
    public void SetTime(float Current_Time, float Total_Time)
    {
        float CurrentTime = Current_Time / 1000;
        float TotalTime = Total_Time / 1000;
        Text m_Text = go_Text.GetComponent<Text>();
        Text m_TotalText = go_TotalTime.GetComponent<Text>();

        m_Text.text = SetTimeHour(Mathf.RoundToInt(CurrentTime)).ToString("#00") + ":" + SetTimeMinute(Mathf.RoundToInt(CurrentTime)).ToString("#00") + ":" + SetTimeSecond(Mathf.RoundToInt(CurrentTime)).ToString("#00");
        m_TotalText.text = SetTimeHour(Mathf.RoundToInt(TotalTime)).ToString("#00") + ":" + SetTimeMinute(Mathf.RoundToInt(TotalTime)).ToString("#00") + ":" + SetTimeSecond(Mathf.RoundToInt(TotalTime)).ToString("#00");
    }

    public int SetTimeHour(int Time)
    {
        Time = Time / 3600;
        return Time;
    }
    public int SetTimeMinute(int Time)
    {
        int Num = Time % 3600;
        Time = Num / 60;
        return Time;
    }
    public int SetTimeSecond(int Time)
    {
        int Num = Time % 3600;
        Time = Num % 60;
        return Time;
    }
    public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("OnPointerEnter:");  
		m_bUpdate = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("OnPointerExit:");
		m_bUpdate = true;
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_srcVideo.SetSeekBarValue (m_srcSlider.value);
	}


	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("OnDrag:"+ eventData);   

		if (m_bActiveDrag == false) 
		{
			m_fLastValue = m_srcSlider.value;
			return;
		}

		m_srcVideo.SetSeekBarValue (m_srcSlider.value);
		m_fLastSetValue = m_srcSlider.value;
		m_bActiveDrag = false;

	}
	public static SeekBarControl f_instance
	{
		get
		{
			return m_instance;
		}
	}
}

