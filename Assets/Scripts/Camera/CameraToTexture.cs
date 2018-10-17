using UnityEngine;
using UnityEngine.UI;

public class CameraToTexture : MonoBehaviour
{
    private bool m_camAvailable;
    private WebCamTexture m_backCam;
    //private Texture m_defaultBackground;

    public RawImage m_background;

    public AspectRatioFitter m_fit;
    private float lastCalcTime;

    private void Start()
    {
        lastCalcTime = Time.time;
        //m_defaultBackground = m_background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            m_camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                m_backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (m_backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        m_backCam.Play();
        m_background.texture = m_backCam;

        m_camAvailable = true;
    }

    private void Update()
    {
        if (!m_camAvailable)
        {
            return;
        }

        float ratio = (float)m_backCam.width / (float)m_backCam.height;
        m_fit.aspectRatio = ratio;

        float scaleY = m_backCam.videoVerticallyMirrored ? -1f : 1f;

        m_background.rectTransform.localScale = new Vector3(1f, scaleY);
        m_background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -m_backCam.videoRotationAngle;
        m_background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    private float GetTextureLuminance(WebCamTexture texture)
    {
        const int STEP_SIZE = 10;
        var pixels = texture.GetPixels();

        int horizontanlSteps = texture.width / STEP_SIZE;
        int verticalSteps = texture.height / STEP_SIZE;

        float averageLuminance = 0f;
        int counter = 0;

        for (int horIndex = 1; horIndex < horizontanlSteps; horIndex++)
        {
            for (int vertIndex = 1; vertIndex < verticalSteps; vertIndex++)
            {
                int pixelPos = horIndex * 10 + (vertIndex - 1) * 10 * texture.width;
                //int pixelPos = horIndex * 10;
                averageLuminance += GetColorLumincane(pixels[pixelPos]);
                counter++;
            }
        }

        if (counter > 0)
        {
            averageLuminance /= counter;
        }

        return averageLuminance;
    }

    // using Relative Luminance formula https://en.wikipedia.org/11wiki/Relative_luminance
    // Luminance = 0.2126R + 0.7152G + 0.0722B
    private float GetColorLumincane(Color color)
    {
        return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
    }
}
