using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Editor
{
    public static class Builder
    {

        private static string gameName = "game";
        private static string version = "";
        private static string debugMode = "";
        private static string windowsArch = "win32";

        private static string KeyStorePass = "";
        private static string KeyAliasPass = "";
        private static string KeyAliasName = "";
        private static string AndroidType = "1";

        private static Dictionary<string, string> arguments;


        [MenuItem("Build/Android")]
        public static void BuildAndroid()
        {
            Argumentator.SetArguments();
            GetArguments();
            SetPassword();

            //var report = BuildPipeline.BuildPlayer(CreateAndroidBuildOptions());
            ChooseAndroidTypeFile(int.Parse(AndroidType));
            //Reporter.CheckReport(report);
        }

        [MenuItem("Build/Win64")]
        public static void BuildWind()
        {
            Argumentator.SetArguments();
            GetArguments();

            var report = BuildPipeline.BuildPlayer(CreateWindowsBuildOptions());

            Reporter.CheckReport(report);
        }

        private static BuildPlayerOptions CreateAndroidBuildOptions()
        {
            var buildOpt = new BuildPlayerOptions()
            {
                target = BuildTarget.Android,
                locationPathName = $"artifacts/{gameName}.{version}.apk",
                scenes = GetScenesFromBuildSettings(),
                options = debugMode.Equals("true") ? BuildOptions.Development : BuildOptions.None
            };
            return buildOpt;
        }

        private static BuildPlayerOptions CreateWindowsBuildOptions()
        {
            var buildOpt = new BuildPlayerOptions()
            {
                locationPathName = $"artifacts/{gameName}.{version}.{windowsArch}/Game.{version}.exe",
                scenes = GetScenesFromBuildSettings(),
            };
            buildOpt.options = debugMode.Equals("true") ? BuildOptions.Development : BuildOptions.None;
            buildOpt.target = windowsArch.Equals("win64")
                ? BuildTarget.StandaloneWindows64
                : BuildTarget.StandaloneWindows;
            return buildOpt;
        }

        private static void GetArguments()
        {
            gameName = Argumentator.GetArgument("gameName");
            version = Argumentator.GetArgument("version");
            debugMode = Argumentator.GetArgument("debugMode");
            windowsArch = Argumentator.GetArgument("winArch");
            KeyStorePass = Argumentator.GetArgument("keyStore");
            KeyAliasName = Argumentator.GetArgument("aliasName");
            KeyAliasPass = Argumentator.GetArgument("aliasPass");
            AndroidType = Argumentator.GetArgument("androidType");
        }

        private static void SetPassword()
        {
            
            PlayerSettings.Android.keystorePass = KeyStorePass;
            PlayerSettings.Android.keyaliasName = KeyAliasName;
            PlayerSettings.Android.keyaliasPass = KeyAliasPass;
        }

        private static void ChooseAndroidTypeFile(int type)
        {
            BuildReport report = null;
            switch (type)
            {
                case 1 : 
                    EditorUserBuildSettings.buildAppBundle = true;
                    report = BuildPipeline.BuildPlayer(CreateAndroidBuildOptions());
                    EditorUserBuildSettings.buildAppBundle = false;
                    Reporter.CheckReport(report);
                    report = BuildPipeline.BuildPlayer(CreateAndroidBuildOptions());
                    Reporter.CheckReport(report);
                    break;
                case 0 :
                    EditorUserBuildSettings.buildAppBundle = false;
                    report =  BuildPipeline.BuildPlayer(CreateAndroidBuildOptions());
                    Reporter.CheckReport(report);
                    break;
                case -1 : 
                    EditorUserBuildSettings.buildAppBundle = true;
                    report =  BuildPipeline.BuildPlayer(CreateAndroidBuildOptions());
                    Reporter.CheckReport(report);
                    break;
            }
            
        } 

        private static string[] GetScenesFromBuildSettings()
        {
            return EditorBuildSettings.scenes
                .Where( scene => scene.enabled )
                .Select( scene => scene.path )
                .ToArray();
        }
    }
}