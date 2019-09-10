using UnityEditor;
using UnityEngine;


public static class EditorTools
{
	static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
	{
		return DrawProperty(label, serializedObject, property, false, options);
	}

	static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
	{
		SerializedProperty sp = serializedObject.FindProperty(property);

		if (sp != null)
		{
			if (label != null)
				EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
			else
				EditorGUILayout.PropertyField(sp, options);
		}
		return sp;
	}

	static public SerializedProperty DrawPropertyIncludeChildren(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
	{
		SerializedProperty sp = serializedObject.FindProperty(property);

		if (sp != null)
		{
			EditorGUILayout.PropertyField(sp, true);
		}
		return sp;
	}
}
