using System;
using System.Collections.Generic;
using System.Text;

namespace Agava.YandexGames.Utility
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
            {
                string maskedValue = MaskJsonString(pref.Value);
                jsonStringBuilder.Append($"\"{pref.Key}\":\"{maskedValue}\",");
            }

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

                            string unmaskedValue = UnmaskJsonString(value.ToString());
                            s_prefs[key.ToString()] = unmaskedValue;
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

        private static string MaskJsonString(string value)
        {
            const string openBracketMask = "#OBM#";
            const string closeBracketMask = "#CBM#";
            const string quoteMask = "#QuM#";
            
            var stringBuilder = new StringBuilder();
            
            int characterIterator = 0;
            while (characterIterator < value.Length)
            {
                char character = value[characterIterator];
                
                switch (character)
                {
                    case '{':
                        stringBuilder.Append(openBracketMask);
                        break;
                    case '}':
                        stringBuilder.Append(closeBracketMask);
                        break;
                    case '"':
                        stringBuilder.Append(quoteMask);
                        break;
                    default:
                        stringBuilder.Append(character);
                        break;
                }

				characterIterator += 1;
            }
            
            return stringBuilder.ToString();
        }

        private static string UnmaskJsonString(string value)
        {
            const string openBracketMask = "#OBM#";
            const string closeBracketMask = "#CBM#";
            const string quoteMask = "#QuM#";
            const char checker = '#';
            const int maskSize = 5;
            
            var stringBuilder = new StringBuilder();
            
            int characterIterator = 0;
            while (characterIterator < value.Length)
            {
                char character = value[characterIterator];

                if (characterIterator <= value.Length - maskSize && character == checker)
                {
                    string subString = value.Substring(characterIterator, maskSize);
                    
                    switch (subString)
                    {
                        case openBracketMask:
                            stringBuilder.Append('{');
                            characterIterator += maskSize;
                            break;
                        case closeBracketMask:
                            stringBuilder.Append('}');
                            characterIterator += maskSize;
                            break;
                        case quoteMask:
                            stringBuilder.Append('"');
                            characterIterator += maskSize;
                            break;
                        default:
                            stringBuilder.Append(character);
                            characterIterator += 1;
                            break;
                    }
                }
                else
                {
                    stringBuilder.Append(character);
                    characterIterator += 1;
                }
            }
            
            return stringBuilder.ToString();
        }
    }
}
