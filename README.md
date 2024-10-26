# i18n-unity-csharp
Lightweight internationalization for use with C#

## Features

- **Dynamic Locale Management**: Load and change locales at runtime
- **INI File Support**: Easily read localization data from `.ini` files
- **Event Notification**: Subscribe to updates when the locale changes
- **Available Locales Retrieval**: Get a list of available locales based on existing `.ini` files

## Getting Started

### Installation

1. **Import the package**: Include the package in your project
2. **Create Locale Files**: Add your `.ini` files in the `StreamingAssets/locales` directory. Each file should be named according to the locale (e.g., `en-US.ini`, `pt-BR.ini`)

### Sample INI File Structure

locale INI file example:

```ini
[settings]
localeName = English (United States)
abbreviation = en-US

[ui]
startButton = Start
exitButton = Exit
```

### Usage

1. **Translate Strings**:

   Use the `Localization.t("key")` method to get the localized string for a given key

   ```csharp
   string startButtonText = Localization.t("UI.startButton");
   ```

2. **Get Current Locale**:

   Use `Localization.GetCurrentLocale()` to retrieve the currently active locale

   ```csharp
   string currentLocale = Localization.GetCurrentLocale();
   ```

3. **Change Locale**:

   To switch to a different locale, use `Localization.SetLocal("locale")`.

   ```csharp
   Localization.SetLocal("pt-BR"); // Switch to Portuguese (Brazil)
   ``` 

### Events

You can subscribe to locale updates via the `OnUpdateLocale` event:

```csharp
Localization.OnUpdateLocale += () =>
{
    // Handle locale update
    Debug.Log("Locale has been updated to: " + Localization.GetCurrentLocale());
};
```

# LocalizedText Component

The `LocalizedText` component allows you to easily display localized text in your Unity project using TextMeshPro

## Script Overview

```csharp
using TMPro;
using UnityEngine;

namespace Utilities.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private string key; 

        private void OnValidate()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshProUGUI>() as TMP_Text
                                     ?? GetComponent<TextMeshPro>();
            }
        }

        private void OnDisable()
        {
            Localization.OnUpdateLocale -= Translate; 
        }

        private void OnEnable()
        {
            Localization.OnUpdateLocale += Translate; 
        }

        void Start()
        {
            Translate();
        }

        /// <summary>
        /// Updates the text with the translation corresponding to the key.
        /// </summary>
        public void Translate()
        {
            if (!string.IsNullOrEmpty(key))
            {
                textMesh.text = Localization.t(key);
            }
        }
    }
}
```

### How to Use

1. **Add the Component**: Attach the `LocalizedText` script to a GameObject that has a TextMeshPro component (like `TextMeshProUGUI` or `TextMeshPro`)
2. **Set the Key**: In the Inspector, set the `key` field to the corresponding localization key you want to display
3. **Initialize Localization**: Ensure that localization has been initialized in your scene by calling `Localization.Init()` before using this component

## Example Repository
  
For practical examples of how to use the localization utility, including switching languages via a menu and common script usage, please visit our [Example Repository](https://github.com/weslleyxm/localization-examples)


This repository includes:

- **Language Switcher Menu**: A functional UI that allows users to change languages in real-time
- **Common Script Examples**: Sample scripts demonstrating how to implement localization in various scenarios

Feel free to clone the repository and explore the examples provided to better understand how to integrate and use the localization utility in your own projects


## API

### Methods

- `static void Init()`: Initializes localization data from the current locale file
- `static Locale[] AvaliableLocales()`: Retrieves available locales from `.ini` files
- `static void SetLocal(string locale)`: Sets the current locale and reinitializes the localization data
- `static string t(string key)`: Translates the given key to the corresponding localized string

### Properties

- `static bool WasInitialized`: Indicates whether the localization has been initialized
- `static string GetCurrentLocale()`: Gets the current locale

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for suggestions and improvements

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details
