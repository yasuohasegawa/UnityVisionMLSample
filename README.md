# UnityVisionMLSample
This project is the sample code of working with ARFoiundation, Vision framework, and CoreML.<br>
<br>
The sample app is almost as same as the Apple sample below.<br>
https://developer.apple.com/documentation/arkit/recognizing_and_labeling_arbitrary_objects?language=objc<br>
<br>

The point is the app sends the ARSession pointer to the native side from Unity side when the app starts, so the app can share the ARSession between the Unity side and Native side.
In this way, we can easily manage the ARSession.

## References:
https://github.com/chenjd/Unity-ARFoundation-HandDetection<br>
https://medium.com/s23nyc-tech/using-machine-learning-and-coreml-to-control-arkit-24241c894e3b

## Tested on:
Unity 2019.1.5f1<br>
iPhoneXS Max iOS 12.2
