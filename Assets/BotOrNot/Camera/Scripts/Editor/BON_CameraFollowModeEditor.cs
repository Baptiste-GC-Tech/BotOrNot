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
        // Synchronise les donn�es avant d�afficher
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

        // Tous les champs � �diter
        PRIVAfficherChamp("_player", "Transform du joueur contr�l�.");
        PRIVAfficherChamp("_otherTarget", "Objet secondaire � suivre (optionnel).");
        PRIVAfficherChamp("_followTarget", "Transform interm�diaire suivi par la cam�ra.");

        GUILayout.Space(5);
        PRIVAfficherChamp("_offsetX", "D�calage horizontal appliqu� � la cam�ra.");
        PRIVAfficherChamp("_offsetY", "D�calage vertical.");
        PRIVAfficherChamp("_offsetZ", "D�calage en profondeur.");

        GUILayout.Space(5);
        PRIVAfficherChamp("_followLerpSpeed", "Vitesse de transition du FollowTarget.");
        PRIVAfficherChamp("_offsetLerpSpeed", "Vitesse de transition des offsets cam�ra.");

        GUILayout.Space(10);
        GUILayout.Label("Mode de Suivi Actuel", EditorStyles.boldLabel);
        PRIVAfficherChamp("_currentMode", "Mode de suivi actif.");

        // Appliquer toutes les modifications � la fin
        serializedObject.ApplyModifiedProperties();
    }


    private void PRIVAfficherChamp(string nomPropri�t�, string infoBulle)
    {
        SerializedProperty propri�t� = serializedObject.FindProperty(nomPropri�t�);
        if (propri�t� != null)
        {
            GUIContent etiquette = new GUIContent(ObjectNames.NicifyVariableName(nomPropri�t�), infoBulle);
            EditorGUILayout.PropertyField(propri�t�, etiquette);
        }
    }
}
