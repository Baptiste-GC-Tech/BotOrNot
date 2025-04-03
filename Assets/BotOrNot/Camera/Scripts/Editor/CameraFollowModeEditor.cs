using UnityEngine;
using UnityEditor;
using System.Drawing.Printing;

[CustomEditor(typeof(CameraFollowMode))]
public class CameraFollowModeEditor : Editor
{

    /*
     *  FIELDS
     */

    private Texture2D _bannerTexture;

    /*
    [SerializeField] private CameraFollowMode camerashake;
    [SerializeField] private float shakeIntensity = 0.0f;
    [SerializeField] private float shakeTime = 0.0f;
    */

    /*
     *  CLASS METHODS
     */

    private void OnEnable()
    {
        _bannerTexture = Resources.Load<Texture2D>("Editor/CameraBanner");
    }

    public override void OnInspectorGUI()
    {
        CameraFollowMode script = (CameraFollowMode)target;

        /* Banni�re */
        if (_bannerTexture != null)
        {
            float aspectRatio = (float)_bannerTexture.width / _bannerTexture.height;
            float width = EditorGUIUtility.currentViewWidth - 40;
            float height = width / aspectRatio;
            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, _bannerTexture, ScaleMode.ScaleToFit);
            GUILayout.Space(10);
        }

        /* Tooltips */
        DrawField("player", "Transform du joueur contr�l�.");
        DrawField("player", "Transform du joueur contr�l�.");
        DrawField("otherTarget", "Objet secondaire � suivre (optionnel).");
        DrawField("followTarget", "Transform interm�diaire suivi par la cam�ra (ex: Follow_Target).");

        GUILayout.Space(5);
        DrawField("offsetX", "D�calage horizontal appliqu� � la cam�ra quand elle suit le joueur. Est oppos� en fonction du sens du joueur.");
        DrawField("offsetY", "D�calage vertical lors du suivi.");
        DrawField("offsetZ", "D�calage de profondeur lors du suivi.");

        GUILayout.Space(5);
        DrawField("followLerpSpeed", "Vitesse de transition de position du FollowTarget.");
        DrawField("offsetLerpSpeed", "Vitesse de transition des offsets cam�ra.");

        GUILayout.Space(10);
        GUILayout.Label("Mode de Suivi Actuel", EditorStyles.boldLabel);

        if (GUILayout.Button("Suivre le Player"))
            script.currentMode = CameraFollowMode.FollowMode.Player;

        if (GUILayout.Button("Suivre l'objet"))
            script.currentMode = CameraFollowMode.FollowMode.OtherObject;

        if (GUILayout.Button("Suivre le Barycentre"))
            script.currentMode = CameraFollowMode.FollowMode.Barycenter;

        if (GUILayout.Button("Mode Automatique (Sensible Trigger Zones)"))
            script.currentMode = CameraFollowMode.FollowMode.Auto;

        GUILayout.Space(5);
        EditorGUILayout.LabelField("Mode actif :", script.currentMode.ToString());
        
        GUILayout.Space(5);
        DrawField("intensity", "intensity of the shake");
        DrawField("shaketime", "duration of the shake");


        /*
        if (GUILayout.Button("Shake"))
        {
            camerashake.ShakeCamera(shakeIntensity, shakeTime);
        }
        */

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
