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
        // Charge l�image depuis Resources/Editor/TriggerBanner.png
        _bannerTexture = Resources.Load<Texture2D>("Editor/TriggerBanner");
    }

    public override void OnInspectorGUI()
    {
        BON_TriggerNotifier _script = (BON_TriggerNotifier)target;

        // Afficher la banni�re si elle existe
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
            EditorGUILayout.HelpBox("Image non trouv�e dans Resources/Editor/TriggerBanner.png", MessageType.Info);
        }

        // Champs principaux avec tooltips
        PRIVAfficherChamp("LinkedObject", "L'objet avec lequel la cam�ra fera un barycentre lorsqu'on entre dans cette zone.");
        PRIVAfficherChamp("_cameraFollow", "R�f�rence vers le script CameraFollowMode � notifier.");
        PRIVAfficherChamp("_playerTag", "Le tag que le joueur doit avoir pour activer cette zone (par d�faut : 'Player').");

        GUILayout.Space(10);
        PRIVAfficherChamp("_duration", "Dur�e pendant laquelle la cam�ra reste focalis�e sur le barycentre. 0 = jusqu�� sortie du trigger.");
        PRIVAfficherChamp("_overrideOffset", "Active un offset personnalis� pour la cam�ra lorsque ce trigger est actif.");
        PRIVAfficherChamp("_offsetX", "D�calage horizontal (X) de la cam�ra lors du barycentre.");
        PRIVAfficherChamp("_offsetY", "D�calage en hauteur (Y) de la cam�ra lors du barycentre.");
        PRIVAfficherChamp("_offsetZ", "D�calage en profondeur (Z) de la cam�ra lors du barycentre.");
        PRIVAfficherChamp("_focusOnly", "Si activ�, la cam�ra se concentre uniquement sur l'objet au lieu du barycentre.");

        serializedObject.ApplyModifiedProperties();
    }

    /*
     *  CLASS METHODS
     */

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
