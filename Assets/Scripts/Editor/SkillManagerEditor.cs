using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SkillManager))]
public class SkillManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		SkillManager me = (SkillManager)target;
		if (GUILayout.Button("Update Kids"))
		{
			me.UpdateSkillKids ();
		}
	}
}