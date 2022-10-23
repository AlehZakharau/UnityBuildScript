using System;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    public static class Reporter
    {
        public static void CheckReport(BuildReport report)
        {
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("||| Build succeeded: " + summary.totalSize + " bytes |||");
                Debug.Log($"||| Build time {summary.totalTime} |||");
                Debug.Log($"||| Errors: {summary.totalErrors} Warnings: {summary.totalWarnings} |||");
            }

            if (summary.result == BuildResult.Failed)
            {
                throw new Exception("Android Build failed");
            }
        }

    }
}