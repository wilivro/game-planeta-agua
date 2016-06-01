 using UnityEngine;
 using UnityEditor;
 
using UnityEngine;
using UnityEditor;

// Simple script that creates a new non-dockable window
public class EditorWindowTest : EditorWindow {

    [MenuItem("Dialogues/Create")]
	static void Initialize() {
		EditorWindowTest window  = (EditorWindowTest)EditorWindow.GetWindow(typeof(EditorWindowTest), true, "Create a dialogue");
	}
}