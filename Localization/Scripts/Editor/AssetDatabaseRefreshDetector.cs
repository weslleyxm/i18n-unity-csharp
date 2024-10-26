using UnityEditor;
using UnityEngine;


namespace Utilities.Localization.InspectorEditor
{ 
    public class AssetDatabaseRefreshDetector : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            Localization.Refresh();
        } 
    }
}