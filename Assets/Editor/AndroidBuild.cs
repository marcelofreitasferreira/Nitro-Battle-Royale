#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class AndroidBuild
{
    public static void Build()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.nitro.battleroyale");
        PlayerSettings.productName = "Nitro Battle Royale";
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
        System.IO.Directory.CreateDirectory("Builds");
        var result = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { "Assets/Scenes/PrototypeArena.unity" },
            locationPathName = "Builds/NitroBattleRoyale-v0.2.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        });
        if (result.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            throw new System.Exception("Android build failed: " + result.summary.result);
    }
}
#endif
