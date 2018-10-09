using UnityEngine;

public class HandleTargetImage : DefaultTrackableEventHandler
{
    [Tooltip("Delay before video will be played (seconds)")]
    [SerializeField]
    private float m_VideoDelay = 1f;

    private float m_TimeStartPlayingVideo = 0;

    private bool m_checkTimer = false;

    protected override void OnTrackingFound()
    {
        m_checkTimer = true;
        m_TimeStartPlayingVideo = Time.time + m_VideoDelay;
    }

    void Update()
    {
        if (m_checkTimer && m_TimeStartPlayingVideo < Time.time)
        {
            EnableAll();
            m_checkTimer = false;
        }
    }

    protected override void OnTrackingLost()
    {
        DisableAll();
    }

    private void EnableAll()
    {
        Debug.Log("Enabling children");
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(true);
        }
    }

    private void DisableAll()
    {
        Debug.Log("Disabling children");
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(false);
        }
    }
}
