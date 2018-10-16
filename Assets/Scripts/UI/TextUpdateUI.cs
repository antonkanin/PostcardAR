using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextUpdateUI : MonoBehaviour
{
    [SerializeField] private FloatVariable m_FloatValue = null;

    private Text m_Text;

    private void Start()
    {
        m_Text = GetComponent<Text>();
    }

    private void Update()
    {
        m_Text.text = m_FloatValue.Value.ToString("0.00");
    }
}
