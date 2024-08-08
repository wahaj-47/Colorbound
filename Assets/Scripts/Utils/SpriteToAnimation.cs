using UnityEditor;
using UnityEngine;

public class AnimationClipNamingWindow : EditorWindow
{
    private Object[] selectedSprites;
    private string[] animationNames;
    private string outputFolderPath;
    private int animationLength = 1;
    private int spriteSkipCount = 0;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Create Animations From Selected Sprites")]
    public static void ShowWindow()
    {
        var window = GetWindow<AnimationClipNamingWindow>();
        window.titleContent = new GUIContent("Create Animations");
        window.selectedSprites = Selection.objects;
        window.animationNames = new string[window.selectedSprites.Length / window.animationLength];
        window.outputFolderPath = "Assets"; // Default to Assets folder
        window.InitializeNames();
        window.Show();
    }

    private void InitializeNames()
    {
        for (int i = 0; i < animationNames.Length; i++)
        {
            animationNames[i] = "Animation_" + i; // Default to "Animation_X"
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

        // Animation length and sprite skip count fields
        animationLength = Mathf.Max(EditorGUILayout.IntField("Animation Length", animationLength), 1);
        spriteSkipCount = EditorGUILayout.IntField("Sprites to Skip", spriteSkipCount);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Show frames that will be included in each animation
        GUILayout.Label("Preview of Frames in Animations:");
        for (int i = 0; i < selectedSprites.Length / animationLength; i++)
        {
            GUILayout.BeginHorizontal();

            for (int j = 0; j < animationLength; j++)
            {
                int spriteIndex = i + j * (spriteSkipCount + 1);
                if (spriteIndex < selectedSprites.Length && selectedSprites[spriteIndex] is Sprite sprite)
                {
                    GUILayout.Label(AssetPreview.GetAssetPreview(sprite), GUILayout.Width(50), GUILayout.Height(50));
                }
            }

            Debug.Log(i);

            // Display input field for the animation name
            animationNames[i] = EditorGUILayout.TextField(animationNames[i], GUILayout.Width(250));

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        // Create animations when the button is clicked
        if (GUILayout.Button("Create Animations"))
        {
            for (int i = 0; i < selectedSprites.Length / animationLength; i++)
            {
                string animationName = animationNames[i];
                CreateAnimationClip(i, animationName);
            }
            Close();
        }
    }

    private void CreateAnimationClip(int startIndex, string animationName)
    {
        // Create a new animation clip
        AnimationClip clip = new AnimationClip
        {
            frameRate = 24 // Set the frame rate
        };

        // Create the animation keyframes
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[animationLength];
        for (int j = 0; j < animationLength; j++)
        {
            int spriteIndex = startIndex + j * (spriteSkipCount + 1);
            if (spriteIndex < selectedSprites.Length && selectedSprites[spriteIndex] is Sprite sprite)
            {
                keyframes[j] = new ObjectReferenceKeyframe
                {
                    time = j / clip.frameRate,
                    value = sprite
                };
            }
            else
            {
                // If we run out of sprites, shorten the keyframe array
                System.Array.Resize(ref keyframes, j);
                break;
            }
        }

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