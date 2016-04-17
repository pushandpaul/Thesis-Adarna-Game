using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectiveMapper))]
public class ListInspector : Editor {
	/*public override void OnInspectorGUI(){
		serializedObject.Update();
		EditorList.Show(serializedObject.FindProperty("partObjective"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("isNPC"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("isObject"));
		serializedObject.ApplyModifiedProperties();
	}*/
}
