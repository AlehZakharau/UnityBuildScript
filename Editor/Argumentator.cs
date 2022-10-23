using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    public static class Argumentator
    {
        private static Dictionary<string, string> arguments;

        public static string GetArgument(string key)
        {
            arguments.TryGetValue(key, out var value);
            return value;
        }
        public static void SetArguments()
        {
            var data = Environment.GetCommandLineArgs();

            arguments = new Dictionary<string, string>();
            foreach (var s in data)
            {
                if (s[0] == '-' && s[1] == '-')
                {
                    var newPair = s.Split('=');
                    newPair[0] = newPair[0].Remove(0, 2);
                    arguments.Add(newPair[0], newPair[1]);
                    Debug.Log($"{newPair[0]} == {newPair[1]}");
                }
            }
        }
    }
}