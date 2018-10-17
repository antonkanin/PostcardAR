using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ShowVideo : MonoBehaviour
{
    [SerializeField] private float m_FadeInTime = 2f;

    private Material m_VideoMaterial;
    private float m_AlphaValue;

    void Start()
    {
        m_VideoMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        m_AlphaValue = 0;
        //UpdateVideoAlpha(m_AlphaValue);
        StartCoroutine(VideoFadeIn());
    }

    private IEnumerator VideoFadeIn()
    {
        float time = 0f;
        while (time <= m_FadeInTime)
        {
            time += Time.deltaTime;
            m_AlphaValue += Time.deltaTime / m_FadeInTime;
            UpdateVideoAlpha(m_AlphaValue);
            yield return null;
        }
    }

    private void UpdateVideoAlpha(float alpha)
    {
        if (m_VideoMaterial != null)
        {
            m_VideoMaterial.SetFloat("_AlphaRange", alpha);
        }
    }
}
