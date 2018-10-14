using UnityEngine;
using UnityEngine.Video;

public class Test : MonoBehaviour
{
    public float m_alpha = 1f;

	// Update is called once per frame
	void Update ()
	{
	    GetComponent<VideoPlayer>().targetCameraAlpha = m_alpha;
	}
}
