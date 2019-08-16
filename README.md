# UnityVisionMLSample
This project is the sample code of working with ARFoiundation, Vision framework, and CoreML.<br>
<br>
This sample app is almost as same as the Apple sample below.<br>
https://developer.apple.com/documentation/arkit/recognizing_and_labeling_arbitrary_objects?language=objc<br>
<br>

The point is the app sends the ARSession pointer to the native side from Unity side when the app starts, so the app can share the ARSession between the Unity side and Native side.
In this way, we can easily manage the ARSession from Native side.<br>
<br>
The ARSession pointer has the structure.<br>
We have to write the following struct class in the .h or .mm files to cast the pointer.<br>

```C#
Here is the C# session pointer to get.

ARSession m_session;
var sessionSubsystem = (ARKitSessionSubsystem)m_session.subsystem;
sessionSubsystem.nativePtr

xxxxx.h
typedef struct UnityXRNativeSession_1
{
    int version;
    void* sessionPtr;
} UnityXRNativeSession_1;


Here is the how to cast from the C# session pointer to Native ARSession.

xxxxx.mm
UnityXRNativeSession_1* unityXRSession = (UnityXRNativeSession_1*) session;
ARSession* sess = (__bridge ARSession*)unityXRSession->sessionPtr;

The session is the pointer from the Unity.
```
<br>
This project is very simple that you can modify whatever you want. <br>
It's good to start with the simple one always.

## References:
https://github.com/chenjd/Unity-ARFoundation-HandDetection<br>
https://medium.com/s23nyc-tech/using-machine-learning-and-coreml-to-control-arkit-24241c894e3b<br>
https://forum.unity.com/threads/getting-access-to-the-arkit-arframe-pointer-with-arfoundation.589390/

## Tested on:
Unity 2019.1.5f1<br>
iPhoneXS Max iOS 12.2
