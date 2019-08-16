using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif

public class VisionML : MonoBehaviour
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _SetUnityARSession(IntPtr session);
#endif

    [SerializeField]
    ARSession m_session;

    [SerializeField]
    Text m_result;

    private bool m_isSessionSend = false;

    public string m_identifier { set; get; } = "";

    static private VisionML instance = null;
    static public VisionML GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
#if UNITY_IOS
        var sessionSubsystem = (ARKitSessionSubsystem)m_session.subsystem;
#else
        XRSessionSubsystem sessionSubsystem = null;
#endif
        if (sessionSubsystem == null)
            return;
        if (!m_isSessionSend)
        {
            IntPtr session = sessionSubsystem.nativePtr;
            #if UNITY_IOS && !UNITY_EDITOR
                _SetUnityARSession(session);
            #endif
            m_isSessionSend = true;
        }
    }

    public void ClassificationResult(string mess)
    {
        m_identifier = mess;
        m_result.text = "Result:" + m_identifier;
    }
}
