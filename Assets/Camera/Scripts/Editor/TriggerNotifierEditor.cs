using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerNotifier))]
public class TriggerNotifierEditor : Editor
{
    private Texture2D bannerTexture;

    private void OnEnable()
    {
        // Charge l’image depuis Resources/Editor/TriggerBanner.png
        bannerTexture = Resources.Load<Texture2D>("Editor/TriggerBanner");
    }

    public override void OnInspectorGUI()
    {
        TriggerNotifier script = (TriggerNotifier)target;

        // Afficher la bannière si elle existe
        if (bannerTexture != null)
        {
            float aspectRatio = (float)bannerTexture.width / bannerTexture.height;
            float width = EditorGUIUtility.currentViewWidth - 40;
            float height = width / aspectRatio;
            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, bannerTexture, ScaleMode.ScaleToFit);
            GUILayout.Space(10);
        }
        else
        {
            EditorGUILayout.HelpBox("Image non trouvée dans Resources/Editor/TriggerBanner.png", MessageType.Info);
        }

        // Champs principaux avec tooltips
        DrawTooltipField("linkedObject", "L'objet avec lequel la caméra fera un barycentre lorsqu'on entre dans cette zone.");
        DrawTooltipField("cameraFollow", "Référence vers le script CameraFollowMode à notifier.");
        DrawTooltipField("playerTag", "Le tag que le joueur doit avoir pour activer cette zone (Ici 'Player').");

        GUILayout.Space(10);
        DrawTooltipField("duration", "Durée pendant laquelle la caméra reste focalisée sur le barycentre. 0 = jusqu’à sortie du trigger.");
        DrawTooltipField("overrideOffset", "Active un offset personnalisé pour la caméra lorsque ce trigger est actif.");
        DrawTooltipField("offsetX", "Décalage horizontal (X) de la caméra lors du barycentre.");
        DrawTooltipField("offsetZ", "Décalage en profondeur (Z) de la caméra lors du barycentre.");
        DrawTooltipField("focusOnly", "Si activé, la caméra se concentre uniquement sur l'objet au lieu du barycentre.");


        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTooltipField(string propertyName, string tooltip)
    {
        SerializedProperty prop = serializedObject.FindProperty(propertyName);
        if (prop != null)
        {
            GUIContent content = new GUIContent(ObjectNames.NicifyVariableName(propertyName), tooltip);
            EditorGUILayout.PropertyField(prop, content);
        }
    }
}
