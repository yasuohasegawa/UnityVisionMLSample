#import "UnityCoreML-Swift.h"
#import <ARKit/ARKit.h>
#import "UnityXRNativePtrs.h"

extern "C" {
    void _SetUnityARSession(void* session) {
        UnityXRNativeSession_1* unityXRSession = (UnityXRNativeSession_1*) session;
        ARSession* sess = (__bridge ARSession*)unityXRSession->sessionPtr;
        CoreMLHelper *helper = [[CoreMLHelper alloc] initWithArsession:sess];
        [helper setUp];
    }
}