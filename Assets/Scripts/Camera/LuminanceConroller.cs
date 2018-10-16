using UnityEngine;
using Vuforia;

public class LuminanceConroller : MonoBehaviour
{
    [SerializeField] private FloatVariable m_LuminanceValue = null;

    private bool m_camAvailable;
    private WebCamTexture m_backCam;
    private float m_lastCalcTime;
    private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGBA8888;
    private bool m_formatRegistered = false;

    private void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        m_lastCalcTime = Time.time;
    }

    private void OnVuforiaStarted()
    {
        if (CameraDevice.Instance.SetFrameFormat(m_PixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + m_PixelFormat.ToString());
            m_formatRegistered = true;
        }
        else
        {
            Debug.LogError(
                "Failed to register pixel format " + m_PixelFormat.ToString() +
                "\n the format may be unsupported by your device;" +
                "\n consider using a different pixel format.");
            m_formatRegistered = false;
        }
    }

    private void Update()
    {
        if (m_formatRegistered)
        {
            if (Time.time - m_lastCalcTime >= 2)
            {
                Vuforia.Image image =
                    CameraDevice.Instance.GetCameraImage(m_PixelFormat);

                var cameraTexture = new Texture2D(0, 0);
                image.CopyToTexture(cameraTexture);

                m_lastCalcTime = Time.time;
                var luminance = GetTextureLuminance(cameraTexture);
                m_LuminanceValue.Value = luminance;
            }
        }
    }

    private float GetTextureLuminance(Texture2D texture)
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
                averageLuminance += GetColorLuminance(pixels[pixelPos]);
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
    private float GetColorLuminance(Color color)
    {
        return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
    }
}
