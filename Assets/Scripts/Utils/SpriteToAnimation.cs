using UnityEditor;
using UnityEngine;

public class AnimationClipNamingWindow : EditorWindow
{
    private Object[] selectedSprites;
    private string[] animationNames;
    private string outputFolderPath;

    [MenuItem("Tools/Create Animations From Selected Sprites")]
    public static void ShowWindow()
    {
        var window = GetWindow<AnimationClipNamingWindow>();
        window.titleContent = new GUIContent("Create Animations");
        window.selectedSprites = Selection.objects;
        window.animationNames = new string[window.selectedSprites.Length];
        window.outputFolderPath = "Assets"; // Default to Assets folder
        window.InitializeNames();
        window.Show();
    }

    private void InitializeNames()
    {
        for (int i = 0; i < selectedSprites.Length; i++)
        {
            if (selectedSprites[i] is Sprite sprite)
            {
                animationNames[i] = sprite.name; // Default to sprite name
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Animation Clips", EditorStyles.boldLabel);

        if (selectedSprites == null || selectedSprites.Length == 0)
        {
            GUILayout.Label("No sprites selected.");
            return;
        }

        // Display output folder selection
        GUILayout.Label("Output Folder:");
        EditorGUILayout.BeginHorizontal();
        outputFolderPath = EditorGUILayout.TextField(outputFolderPath);
        if (GUILayout.Button("Browse", GUILayout.Width(75)))
        {
            string selectedFolder = EditorUtility.OpenFolderPanel("Select Output Folder", outputFolderPath, "");
            if (!string.IsNullOrEmpty(selectedFolder))
            {
                outputFolderPath = FileUtil.GetProjectRelativePath(selectedFolder);
            }
        }
        EditorGUILayout.EndHorizontal();

        // Display the list of sprites with input fields
        for (int i = 0; i < selectedSprites.Length; i++)
        {
            if (selectedSprites[i] is Sprite sprite)
            {
                GUILayout.BeginHorizontal();

                // Display the sprite preview
                GUILayout.Label(AssetPreview.GetAssetPreview(sprite), GUILayout.Width(50), GUILayout.Height(50));

                // Display input field for the full animation name
                animationNames[i] = EditorGUILayout.TextField(animationNames[i], GUILayout.Width(250));

                GUILayout.EndHorizontal();
            }
        }

        // Create animations when the button is clicked
        if (GUILayout.Button("Create Animations"))
        {
            for (int i = 0; i < selectedSprites.Length; i++)
            {
                if (selectedSprites[i] is Sprite sprite)
                {
                    string animationName = animationNames[i];
                    CreateAnimationClip(sprite, animationName);
                }
            }
            Close();
        }
    }

    private void CreateAnimationClip(Sprite sprite, string animationName)
    {
        // Create a new animation clip
        AnimationClip clip = new AnimationClip
        {
            frameRate = 24 // Set the frame rate
        };

        // Create the animation curve
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[1];
        keyframes[0] = new ObjectReferenceKeyframe
        {
            time = 0,
            value = sprite
        };

        // Create the animation binding
        EditorCurveBinding curveBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        // Set the keyframes to the clip
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);

        // Save the clip
        string directory = string.IsNullOrEmpty(outputFolderPath) ? "Assets" : outputFolderPath;
        string animationClipPath = System.IO.Path.Combine(directory, animationName + ".anim");
        animationClipPath = AssetDatabase.GenerateUniqueAssetPath(animationClipPath);
        AssetDatabase.CreateAsset(clip, animationClipPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Created animation clip: " + animationClipPath);
    }
}