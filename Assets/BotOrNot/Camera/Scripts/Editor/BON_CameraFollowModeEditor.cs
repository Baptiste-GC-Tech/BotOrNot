using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BON_CameraFollowMode))]
public class BON_CameraFollowModeEditor : Editor
{
    private Texture2D _bannerTexture;

    private void OnEnable()
    {
        _bannerTexture = Resources.Load<Texture2D>("Editor/CameraBanner");
    }

    public override void OnInspectorGUI()
    {
        // Synchronise les données avant d’afficher
        serializedObject.Update();

        BON_CameraFollowMode _script = (BON_CameraFollowMode)target;

        if (_bannerTexture != null)
        {
            float aspectRatio = (float)_bannerTexture.width / _bannerTexture.height;
            float width = EditorGUIUtility.currentViewWidth - 40;
            float height = width / aspectRatio;
            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, _bannerTexture, ScaleMode.ScaleToFit);
            GUILayout.Space(10);
        }

        // Tous les champs à éditer
        PRIVAfficherChamp("_player", "Transform du joueur contrôlé.");
        PRIVAfficherChamp("_otherTarget", "Objet secondaire à suivre (optionnel).");
        PRIVAfficherChamp("_followTarget", "Transform intermédiaire suivi par la caméra.");

        GUILayout.Space(5);
        PRIVAfficherChamp("_offsetX", "Décalage horizontal appliqué à la caméra.");
        PRIVAfficherChamp("_offsetY", "Décalage vertical.");
        PRIVAfficherChamp("_offsetZ", "Décalage en profondeur.");

        GUILayout.Space(5);
        PRIVAfficherChamp("_followLerpSpeed", "Vitesse de transition du FollowTarget.");
        PRIVAfficherChamp("_offsetLerpSpeed", "Vitesse de transition des offsets caméra.");

        GUILayout.Space(10);
        GUILayout.Label("Mode de Suivi Actuel", EditorStyles.boldLabel);
        PRIVAfficherChamp("_currentMode", "Mode de suivi actif.");

        // Appliquer toutes les modifications à la fin
        serializedObject.ApplyModifiedProperties();
    }


    private void PRIVAfficherChamp(string nomPropriété, string infoBulle)
    {
        SerializedProperty propriété = serializedObject.FindProperty(nomPropriété);
        if (propriété != null)
        {
            GUIContent etiquette = new GUIContent(ObjectNames.NicifyVariableName(nomPropriété), infoBulle);
            EditorGUILayout.PropertyField(propriété, etiquette);
        }
    }
}
