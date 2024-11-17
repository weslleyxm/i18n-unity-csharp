using UnityEngine;
using IniParser;
using IniParser.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Localization
{
    public class Locale
    {
        public string localeName;
        public string abbreviation;
    }

    public static class Localization
    {
        private static IniData LocaleData; // Holds the localization data
        private static string CurrentLocale = "es-ES"; // Default locale
        public static Action OnUpdateLocale; // Action to notify when locale is updated
        public static string LocalePath = $"{Application.streamingAssetsPath}/locales"; // Path to locale files

        /// <summary>
        /// Indicates whether the localization has been initialized
        /// </summary>
        public static bool WasInitialized
        {
            get
            {
                return LocaleData == null; // Returns true if localization data has not been initialized
            }
        }

        /// <summary>
        /// Initializes the localization data from the current locale file
        /// </summary>
        public static void Init()
        {
            if (LocaleData == null)
            {
                string langPath = $"{LocalePath}/{CurrentLocale}.ini"; // Path to the current locale file

                if (File.Exists(langPath)) // Checks if the locale file exists
                {
                    try
                    {
                        var parser = new FileIniDataParser();

                        string fileContent;
                        using (var reader = new StreamReader(langPath, new UTF8Encoding(false)))
                        {
                            fileContent = reader.ReadToEnd();
                        }

                        LocaleData = parser.Parser.Parse(fileContent);
                    }
                    catch (System.Exception)
                    {
                        LocaleData = null;
                        Debug.LogError("<color=red>Localization: </color>There was a problem reading the translation file");
                    }
                }
                else
                {
                    LocaleData = null;
                    Debug.LogError("<color=red>Localization: </color>The selected translation file was not found");
                }

                if (LocaleData != null)
                    OnUpdateLocale?.Invoke(); // Invokes the action to update locale
            }
        }

        /// <summary>
        /// Retrieves available locales from .ini files in the specified directory and returns an array of <see cref="Locale"/> objects
        /// </summary>
        /// <returns>An array of <see cref="Locale"/> objects with populated <c>localeName</c> and <c>abbreviation</c></returns>
        public static Locale[] AvaliableLocales()
        {
            string[] files = Directory.GetFiles(LocalePath, "*.ini");
            List<Locale> locales = new List<Locale>();

            foreach (var file in files)
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(file);

                Locale locale = new Locale
                {
                    localeName = data["settings"]["localeName"],
                    abbreviation = data["settings"]["abbreviation"]
                };

                if (!string.IsNullOrEmpty(locale.localeName) || !string.IsNullOrEmpty(locale.abbreviation))
                {
                    locales.Add(locale);
                }
            }

            return locales.ToArray();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Reinitializes the localization data to load new data
        /// </summary>
        public static void Refresh()
        {
            if (LocaleData != null)
            {
                LocaleData = null;
                Init();
            }
        }
#endif

        /// <summary>
        /// Sets the path for the locale files
        /// </summary>
        /// <param name="localePath">Optional custom locale path</param>
        public static void SetPath(string localePath = null)
        {
            LocalePath = localePath; // Updates the locale path
        }

        /// <summary>
        /// Gets the current locale.
        /// </summary>
        /// <returns>The current locale as a string</returns>
        public static string GetCurrentLocale()
        {
            return CurrentLocale; // Returns the current locale
        }

        /// <summary>
        /// Sets the current locale and reinitializes the localization data
        /// </summary>
        /// <param name="locale">The locale to set</param>
        public static void SetLocal(string locale)
        {
            LocaleData = null; //Set LocaleData to null to reinitializes the localization data
            CurrentLocale = locale; // Updates the current locale
            Init(); // Reinitializes the localization data
        }

        /// <summary>
        /// Translates the given key to the corresponding localized string
        /// </summary>
        /// <param name="key">The key to translate.</param>
        /// <returns>The translated string, or the key if translation fails</returns>
        internal static string internal_t(string key)
        {
            Init(); // Ensure localization data is initialized

            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            var keys = key.Split('.'); // Splits the key into parts

            if (LocaleData != null)
            {
                if (LocaleData.Sections.ContainsSection(keys[0]))
                {
                    if (keys.Length > 1)
                    {
                        if (LocaleData[keys[0]].ContainsKey(keys[1]))
                        {
                            return LocaleData[keys[0]][keys[1]];
                        }
                    }
                }
            }

            return string.Empty; // Returns an empty string if the key was not found
        }

        public static bool Contains(string key)
        {
            return !string.IsNullOrEmpty(internal_t(key));
        }

        public static string t(string key)
        {
            string tValue = internal_t(key);

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(tValue))
            {
                Debug.LogError($"The localized text key \"{key}\" was not found");
                return "";
            }

            return tValue;
        }
    }
}
