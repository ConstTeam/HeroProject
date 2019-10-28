using MS;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SpawnNormal))]
public class SpwanEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		SerializedProperty sp = EditorTools.DrawProperty("Shape", serializedObject, "m_Shape");
		SpawnBase.SpawnShape shape = (SpawnBase.SpawnShape)sp.intValue;

		if (shape == SpawnBase.SpawnShape.Rectangle)
		{
			EditorTools.DrawProperty("SizeX", serializedObject, "m_fSizeX");
			EditorTools.DrawProperty("SizeY", serializedObject, "m_fSizeY");
		}
		else if(shape == SpawnBase.SpawnShape.Circle)
		{
			EditorTools.DrawProperty("Radius", serializedObject, "m_fRadius");
		}

		EditorTools.DrawPropertyIncludeChildren("MonsterPoints", serializedObject, "MonsterSpawn");
		EditorTools.DrawPropertyIncludeChildren("BossPoints", serializedObject, "BossSpawn");

		serializedObject.ApplyModifiedProperties();
	}
}
