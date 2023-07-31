using System;
using System.Collections.Generic;
using System.Text;

namespace Agava.YandexGames
{
    public static class PlayerPrefs
    {
        private static Action s_onSaveSuccessCallback;
        private static Action<string> s_onSaveErrorCallback;

        private static Action s_onLoadSuccessCallback;
        private static Action<string> s_onLoadErrorCallback;

        private static readonly Dictionary<string, string> s_prefs = new Dictionary<string, string>();

        public static void Save(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            var jsonStringBuilder = new StringBuilder();
            jsonStringBuilder.Append('{');

            foreach (KeyValuePair<string, string> pref in s_prefs)
                jsonStringBuilder.Append($"\"{pref.Key}\":\"{pref.Value}\",");

            if (s_prefs.Count > 0)
                jsonStringBuilder.Length -= 1;

            jsonStringBuilder.Append('}');

            string jsonData = jsonStringBuilder.ToString();

            s_onSaveSuccessCallback = onSuccessCallback;
            s_onSaveErrorCallback = onErrorCallback;

            PlayerAccount.SetCloudSaveData(jsonData, OnSaveSuccessCallback, OnSaveErrorCallback);
        }

        private static void OnSaveSuccessCallback()
        {
            s_onSaveSuccessCallback?.Invoke();
        }

        private static void OnSaveErrorCallback(string errorMessage)
        {
            s_onSaveErrorCallback?.Invoke(errorMessage);
        }

        public static void Load(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onLoadSuccessCallback = onSuccessCallback;
            s_onLoadErrorCallback = onErrorCallback;

            PlayerAccount.GetCloudSaveData(OnLoadSuccessCallback, OnLoadErrorCallback);
        }

        enum IterationState
        {
            StartingKeyQuote,
            Key,
            StartingValueQuote,
            Value
        }

        private static void OnLoadSuccessCallback(string jsonData)
        {
            ParseAndApplyData(jsonData);

            s_onLoadSuccessCallback?.Invoke();
        }

        public static void ParseAndApplyData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
                jsonData = "{}";

            s_prefs.Clear();

            string unparsedData = jsonData.Trim('{', '}');

            var key = new StringBuilder();
            var value = new StringBuilder();

            IterationState iterationState = IterationState.StartingKeyQuote;

            int characterIterator = 0;
            while (characterIterator < unparsedData.Length)
            {
                char character = unparsedData[characterIterator];

                switch (iterationState)
                {
                    case IterationState.StartingKeyQuote:
                        if (character == '"')
                        {
                            iterationState = IterationState.Key;
                        }

                        break;

                    case IterationState.Key:
                        if (character == '"')
                        {
                            iterationState = IterationState.StartingValueQuote;
                        }
                        else
                        {
                            key.Append(character);
                        }

                        break;

                    case IterationState.StartingValueQuote:
                        if (character == '"')
                        {
                            iterationState = IterationState.Value;
                        }

                        break;

                    case IterationState.Value:
                        if (character == '"')
                        {
                            iterationState = IterationState.StartingKeyQuote;

                            s_prefs[key.ToString()] = value.ToString();
                            key.Clear();
                            value.Clear();
                        }
                        else
                        {
                            value.Append(character);
                        }

                        break;
                }

                characterIterator += 1;
            }
        }

        private static void OnLoadErrorCallback(string errorMessage)
        {
            s_onLoadErrorCallback?.Invoke(errorMessage);
        }

        public static bool HasKey(string key)
        {
            return s_prefs.ContainsKey(key);
        }

        public static void DeleteKey(string key)
        {
            s_prefs.Remove(key);
        }

        public static void DeleteAll()
        {
            s_prefs.Clear();
        }

        public static void SetString(string key, string value)
        {
            if (s_prefs.ContainsKey(key))
                s_prefs[key] = value;
            else
                s_prefs.Add(key, value);
        }

        public static string GetString(string key, string defaultValue)
        {
            if (s_prefs.ContainsKey(key))
                return s_prefs[key];
            else
                return defaultValue;
        }

        public static string GetString(string key)
        {
            string defaultValue = "";
            return GetString(key, defaultValue);
        }

        public static void SetInt(string key, int value)
        {
            if (s_prefs.ContainsKey(key))
                s_prefs[key] = value.ToString();
            else
                s_prefs.Add(key, value.ToString());
        }

        public static int GetInt(string key, int defaultValue)
        {
            if (s_prefs.ContainsKey(key) && int.TryParse(s_prefs[key], out int value))
                return value;
            else
                return defaultValue;
        }

        public static int GetInt(string key)
        {
            int defaultValue = 0;
            return GetInt(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            if (s_prefs.ContainsKey(key))
                s_prefs[key] = value.ToString();
            else
                s_prefs.Add(key, value.ToString());
        }

        public static float GetFloat(string key, float defaultValue)
        {
            if (s_prefs.ContainsKey(key) && float.TryParse(s_prefs[key], out float value))
                return value;
            else
                return defaultValue;
        }

        public static float GetFloat(string key)
        {
            float defaultValue = 0;
            return GetFloat(key, defaultValue);
        }
    }
}
