using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BON_TriggerNotifier))]
public class BON_TriggerNotifierEditor : Editor
{
    /*
     *  FIELDS
     */

    private Texture2D _bannerTexture;

    /*
     *  UNITY METHODS
     */

    private void OnEnable()
    {
        // Charge l’image depuis Resources/Editor/TriggerBanner.png
        _bannerTexture = Resources.Load<Texture2D>("Editor/TriggerBanner");
    }

    public override void OnInspectorGUI()
    {
        BON_TriggerNotifier _script = (BON_TriggerNotifier)target;

        // Afficher la bannière si elle existe
        if (_bannerTexture != null)
        {
            float aspectRatio = (float)_bannerTexture.width / _bannerTexture.height;
            float width = EditorGUIUtility.currentViewWidth - 40;
            float height = width / aspectRatio;
            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, _bannerTexture, ScaleMode.ScaleToFit);
            GUILayout.Space(10);
        }
        else
        {
            EditorGUILayout.HelpBox("Image non trouvée dans Resources/Editor/TriggerBanner.png", MessageType.Info);
        }

        // Champs principaux avec tooltips
        PRIVAfficherChamp("LinkedObject", "L'objet avec lequel la caméra fera un barycentre lorsqu'on entre dans cette zone.");
        PRIVAfficherChamp("_cameraFollow", "Référence vers le script CameraFollowMode à notifier.");
        PRIVAfficherChamp("_playerTag", "Le tag que le joueur doit avoir pour activer cette zone (par défaut : 'Player').");

        GUILayout.Space(10);
        PRIVAfficherChamp("_duration", "Durée pendant laquelle la caméra reste focalisée sur le barycentre. 0 = jusqu’à sortie du trigger.");
        PRIVAfficherChamp("_overrideOffset", "Active un offset personnalisé pour la caméra lorsque ce trigger est actif.");
        PRIVAfficherChamp("_offsetX", "Décalage horizontal (X) de la caméra lors du barycentre.");
        PRIVAfficherChamp("_offsetY", "Décalage en hauteur (Y) de la caméra lors du barycentre.");
        PRIVAfficherChamp("_offsetZ", "Décalage en profondeur (Z) de la caméra lors du barycentre.");
        PRIVAfficherChamp("_focusOnly", "Si activé, la caméra se concentre uniquement sur l'objet au lieu du barycentre.");

        serializedObject.ApplyModifiedProperties();
    }

    /*
     *  CLASS METHODS
     */

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
