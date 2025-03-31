using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraFollowMode))]
public class CameraFollowModeEditor : Editor
{
    private Texture2D bannerTexture;

    private void OnEnable()
    {
        // Charger l'image depuis le dossier Resources
        bannerTexture = Resources.Load<Texture2D>("Editor/CameraBanner");
    }

    public override void OnInspectorGUI()
    {
        // Afficher l'image si elle est chargée
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
            EditorGUILayout.HelpBox("Image non trouvée dans Resources/Editor/CameraBanner.png", MessageType.Info);
        }

        DrawDefaultInspector();

        CameraFollowMode script = (CameraFollowMode)target;

        GUILayout.Space(10);
        GUILayout.Label("Mode de Suivi", EditorStyles.boldLabel);

        if (GUILayout.Button("Suivre le Player"))
        {
            script.currentMode = CameraFollowMode.FollowMode.Player;
        }

        if (GUILayout.Button("Suivre l'objet"))
        {
            script.currentMode = CameraFollowMode.FollowMode.OtherObject;
        }

        if (GUILayout.Button("Suivre le Barycentre"))
        {
            script.currentMode = CameraFollowMode.FollowMode.Barycenter;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Mode Actuel :", script.currentMode.ToString());
    }
}
