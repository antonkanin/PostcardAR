using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    [SerializeField] private GameObject m_textArea = null;

    private static Debugger m_instance;

    public static Debugger Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Debugger>();
            }

            return m_instance;
        }
    }

    public void Log(string message)
    {
        m_textArea.GetComponent<Text>().text += message;
        Debug.Log(message);
    }

    public void LogLine(string message)
    {
        Log(message + Environment.NewLine);
    }

}