using TMPro;
using UnityEngine;

namespace Utilities.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh; // Reference to the TMP_Text component for displaying translated text
        [SerializeField] private string key; // Key used to look up the translation

        private void OnValidate()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshProUGUI>() as TMP_Text
                                     ?? GetComponent<TextMeshPro>();
            }
             
#if UNITY_EDITOR 
            Translate();
            Localization.OnUpdateLocale += Translate; 
#endif

        }

        private void OnDisable()
        {
            Localization.OnUpdateLocale -= Translate; 
        }

        private void OnEnable()
        {
            Translate();
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
                textMesh.text = Localization.internal_t(key); 
            }
        }
    }
}
