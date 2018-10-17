using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using Image = Vuforia.Image;

public class LuminanceConroller : MonoBehaviour
{
    [SerializeField] private FloatVariable m_LuminanceValue = null;
    public RawImage m_cameraImage;

    private bool m_camAvailable;
    private WebCamTexture m_backCam;
    private float m_lastCalcTime;

#if UNITY_EDITOR
    private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGBA8888;
#else
    //private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
    private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB565;
#endif

    private bool m_formatRegistered = false;

    private void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        m_lastCalcTime = Time.time;
        m_LuminanceValue.Value = 0f;
    }

    private void OnVuforiaStarted()
    {
        if (CameraDevice.Instance.SetFrameFormat(m_PixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + m_PixelFormat.ToString());
            Debugger.Instance.LogLine("Registered");
            m_formatRegistered = true;
        }
        else
        {
            Debug.LogError(
                "Failed to register pixel format " + m_PixelFormat.ToString() +
                "\n the format may be unsupported by your device;" +
                "\n consider using a different pixel format.");
            Debugger.Instance.LogLine("Failed to register");
            m_formatRegistered = false;
        }
    }

    private void Update()
    {
        if (m_formatRegistered)
        {
            if (Time.time - m_lastCalcTime >= 2)
            {
                Debugger.Instance.Log(Time.time.ToString("0.000"));
                Vuforia.Image image =
                    CameraDevice.Instance.GetCameraImage(m_PixelFormat);

                if (image != null)
                {
                    Debugger.Instance.Log(" Got the image object ");
                }
                else
                {
                    Debugger.Instance.Log(" Didn't get the image object ");
                }

                var cameraTexture = new Texture2D(0, 0);
                image.CopyToTexture(cameraTexture);

                if (m_cameraImage != null)
                {
                    m_cameraImage.texture = cameraTexture;
                }

                var luminance = GetTextureLuminance(cameraTexture);
                m_LuminanceValue.Value = luminance;
                m_lastCalcTime = Time.time;

                Debugger.Instance.LogLine(luminance.ToString("0.00"));
            }
        }
        else
        {
            Debugger.Instance.LogLine("Did not register");
        }
    }

    private float GetTextureLuminance(Texture2D texture)
    {
        const int STEP_SIZE = 10;
        var pixels = texture.GetPixels();
        int horizontanlSteps = texture.width / STEP_SIZE;
        int verticalSteps = texture.height / STEP_SIZE;
        float averageLuminance = 0f;

        for (int horIndex = 1; horIndex < horizontanlSteps; horIndex++)
        {
            for (int vertIndex = 1; vertIndex < verticalSteps; vertIndex++)
            {
                int pixelPos = horIndex * STEP_SIZE +
                               (vertIndex - 1) * STEP_SIZE * texture.width;

                averageLuminance += GetColorLuminance(pixels[pixelPos]);
            }
        }

        averageLuminance /= ((horizontanlSteps - 1) * (verticalSteps - 1));

        return averageLuminance;
    }

    // using Relative Luminance formula https://en.wikipedia.org/11wiki/Relative_luminance
    // Luminance = 0.2126R + 0.7152G + 0.0722B
    private float GetColorLuminance(Color color)
    {
        return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
    }
}
