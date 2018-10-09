using UnityEngine;
using UnityEngine.Video;

public class ShowVideo : MonoBehaviour {

    [SerializeField] private VideoPlayer m_VideoPlayer = null;
    [SerializeField] private float m_FadeOutTime = 1f;

    private float m_EnabledTime;

    void Start ()
	{
	    m_VideoPlayer = GetComponent<VideoPlayer>();
	}

    private void OnEnable()
    {
        m_EnabledTime = Time.time;
        m_VideoPlayer.targetCameraAlpha = 0;
    }

    void Update()
    {
        float currentAlpha = m_VideoPlayer.targetCameraAlpha;
        float alphaDiff = Mathf.Abs(1.0f - currentAlpha);
        const float m_fadeRate = 10;
        Debug.Log(m_VideoPlayer.targetCameraAlpha);

        if (alphaDiff > 0.00001f)
        {
            m_VideoPlayer.targetCameraAlpha = currentAlpha + Time.deltaTime / 3.0f;
        }

        //if ((m_EnabledTime + m_FadeOutTime < Time.time) && (m_VideoPlayer.isPlaying == false))
        //{
        //    m_VideoPlayer.Play();
        //}
    }

    // Update is called once per frame
}
