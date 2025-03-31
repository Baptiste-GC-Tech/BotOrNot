using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraFollowMode))]
public class CameraFollowModeEditor : Editor
{
    private Texture2D bannerTexture;

    private void OnEnable()
    {
        bannerTexture = Resources.Load<Texture2D>("Editor/CameraBanner");
    }

    public override void OnInspectorGUI()
    {
        CameraFollowMode script = (CameraFollowMode)target;

        // Bannière en haut
        if (bannerTexture != null)
        {
            float aspectRatio = (float)bannerTexture.width / bannerTexture.height;
            float width = EditorGUIUtility.currentViewWidth - 40;
            float height = width / aspectRatio;
            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, bannerTexture, ScaleMode.ScaleToFit);
            GUILayout.Space(10);
        }

        // Champs manuels avec tooltips
        DrawField("player", "Transform du joueur contrôlé.");
        DrawField("otherTarget", "Objet secondaire à suivre (optionnel).");
        DrawField("followTarget", "Transform intermédiaire suivi par la caméra (ex: Follow_Target).");

        GUILayout.Space(5);
        DrawField("offsetX", "Décalage horizontal lors du suivi.");
        DrawField("offsetZ", "Décalage de profondeur (Z).");

        GUILayout.Space(5);
        DrawField("followLerpSpeed", "Vitesse de transition de position du FollowTarget.");
        DrawField("offsetLerpSpeed", "Vitesse de transition des offsets caméra.");

        GUILayout.Space(10);
        GUILayout.Label("Mode de Suivi Actuel", EditorStyles.boldLabel);

        if (GUILayout.Button("Suivre le Player"))
            script.currentMode = CameraFollowMode.FollowMode.Player;

        if (GUILayout.Button("Suivre l'objet"))
            script.currentMode = CameraFollowMode.FollowMode.OtherObject;

        if (GUILayout.Button("Suivre le Barycentre"))
            script.currentMode = CameraFollowMode.FollowMode.Barycenter;

        if (GUILayout.Button("Mode Automatique (Trigger Zones)"))
            script.currentMode = CameraFollowMode.FollowMode.Auto;

        GUILayout.Space(5);
        EditorGUILayout.LabelField("Mode actif :", script.currentMode.ToString());
    }

    private void DrawField(string propertyName, string tooltip)
    {
        SerializedProperty property = serializedObject.FindProperty(propertyName);
        if (property != null)
        {
            GUIContent label = new GUIContent(ObjectNames.NicifyVariableName(propertyName), tooltip);
            EditorGUILayout.PropertyField(property, label);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
