using System;
using UnityEngine;

public class StaticLogDisplay : MonoBehaviour
{
    public static StaticLogDisplay Instance;

    [SerializeField]
    private TMPro.TextMeshProUGUI _text;
    private string _log = "";

    private static string _lastLog = "";

    private float nospamTim;

    private void Awake()
    {
        Instance = this;
    }

    private static void UpdateText()
    {
        Instance._text.text = Instance._log;
    }

    private void Update()
    {
        nospamTim -= Time.deltaTime;
        if(nospamTim< 0)
        {
            _lastLog = "";
            nospamTim = 0.5f;
        }
    }

    /// <summary>
    /// use from Util.Log. 
    /// why not just here? idk
    /// </summary>
    public static void Log(string message, string color)
    {
        if (Instance == null || !Instance.isActiveAndEnabled)
            return;

        string log = $"<color={color}>{message}</color>\n";

        if(_lastLog == log)
        {
            return;
        }
        _lastLog = log;

        Instance._log += log;
        UpdateText();

    }

    public void Clear()
    {
        _log = "";
        _lastLog = "";
    }
}
