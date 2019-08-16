using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;

public class PostProcess 
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if(buildTarget != BuildTarget.iOS)
        {
            return;
        }

        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        
        var proj = new PBXProject();
        proj.ReadFromFile(projPath);
        var targetGUID = proj.TargetGuidByName("Unity-iPhone");

        proj.AddBuildProperty(targetGUID, "SWIFT_VERSION", "5.0");
        proj.SetBuildProperty(targetGUID, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/iOS/Bridging-Header.h");
        proj.SetBuildProperty(targetGUID, "SWIFT_OBJC_INTERFACE_HEADER_NAME","UnityCoreML-Swift.h");
        proj.SetBuildProperty(targetGUID, "COREML_CODEGEN_LANGUAGE", "Swift");
        
        // add mlmodel to xcode proj build phase.
        var buildPhaseGUID = proj.AddSourcesBuildPhase(targetGUID);
        var handModelPath = Application.dataPath + "/../CoreML/Inceptionv3.mlmodel";
        var fileGUID = proj.AddFile(handModelPath, "/Inceptionv3.mlmodel");
        proj.AddFileToBuildSection(targetGUID, buildPhaseGUID, fileGUID);
        
        proj.WriteToFile(projPath);

    }
}
#endif