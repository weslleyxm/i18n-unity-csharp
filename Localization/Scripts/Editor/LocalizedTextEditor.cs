using UnityEditor;
using UnityEngine;


namespace Utilities.Localization.InspectorEditor
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : Editor
    {
        SerializedProperty key;

        void OnEnable()
        {
            key = serializedObject.FindProperty("key");
        }

        public override void OnInspectorGUI()
        {
            if (Localization.WasInitialized)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.HelpBox("\"Localization\" was not initialized correctly", MessageType.Error);
                EditorGUILayout.Space(5);
            }
            else
            {
                if (string.IsNullOrEmpty(Localization.internal_t(key.stringValue)))
                {
                    EditorGUILayout.Space(5);
                    EditorGUILayout.HelpBox("The current localized text key was not found", MessageType.Warning);
                    EditorGUILayout.Space(5);
                }
            } 

            serializedObject.Update();
            EditorGUILayout.PropertyField(key);
            serializedObject.ApplyModifiedProperties(); 
        }
    }
}