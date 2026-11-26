using System;
using UnityEngine;

public static class GameBootstrap
{
    private const int TargetFps = 60;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Setup()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFps;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (!(Debug.unityLogger.logHandler is QuietLogHandler))
        {
            Debug.unityLogger.logHandler = new QuietLogHandler(Debug.unityLogger.logHandler);
        }

        Application.focusChanged -= OnFocusChanged;
        Application.focusChanged += OnFocusChanged;
    }

    private static void OnFocusChanged(bool hasFocus)
    {
        if (!hasFocus) PlayerPrefs.Save();
    }

    private class QuietLogHandler : ILogHandler
    {
        private readonly ILogHandler _original;

        public QuietLogHandler(ILogHandler original)
        {
            _original = original;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (logType == LogType.Log && IsNoise(format, args)) return;

            _original.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            _original.LogException(exception, context);
        }

        private bool IsNoise(string format, object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0 && args[0] != null)
            {
                message = args[0].ToString();
            }

            if (message == null) return false;

            return message.StartsWith("Showing window:")
                || message.StartsWith("Hiding window:")
                || message.StartsWith("Window is currently showing");
        }
    }
}
