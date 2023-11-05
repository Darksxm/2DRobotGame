using UnityEditor;
using UnityEngine;

public class GitBranchTool : EditorWindow
{
    private string gitBranch;
    private GUIStyle labelStyle;

    [MenuItem("Window/Git Branch Tool")]
    private static void ShowWindow()
    {
        GetWindow<GitBranchTool>("Git Branch Tool");
    }

    private void OnEnable()
    {
        RefreshGitBranch();
        EditorApplication.update += AutoRefreshGitBranch;

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.padding = new RectOffset(0, 0, 0, 0);

    }
    private void OnDisable()
    {
        // Remove the update callback when the window is closed
        EditorApplication.update -= AutoRefreshGitBranch;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Current Git Branch:", EditorStyles.boldLabel);
        GUIStyle largeFontStyle = new GUIStyle(GUI.skin.textArea);
        largeFontStyle.fontSize = 16;
        GUILayout.Label(gitBranch, largeFontStyle);
    }
    private void AutoRefreshGitBranch()
    {
        // Check for Git branch changes at regular intervals
        if (EditorApplication.timeSinceStartup % 5.0f < 0.1f) // Refresh every 5 seconds
        {
            RefreshGitBranch();
            Repaint(); // Force the window to repaint and show the updated branch
        }
    }

    private void RefreshGitBranch()
    {
        // Use System.Diagnostics.Process to execute Git command to get the current branch
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "git",
            Arguments = "rev-parse --abbrev-ref HEAD", // Git command to get the current branch
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;
        process.Start();

        // Read the output of the Git command
        gitBranch = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();
    }
}
