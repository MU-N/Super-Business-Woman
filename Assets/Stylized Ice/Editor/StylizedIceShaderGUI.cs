using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StylizedIceShaderGUI : ShaderGUI 
{
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		base.OnGUI(materialEditor, properties);

		OnGUI_Snow(materialEditor, properties);

		OnGUI_LocalSpaceOrWorldSpace(materialEditor, properties);
	}

	private void OnGUI_LocalSpaceOrWorldSpace(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		BeginBox();
		{
			OnGUI_DrawShaderProperty("SPACE", "Space", materialEditor, properties);

			if(IsKeywordEnabled(materialEditor, "SPACE_LOCAL"))
			{
				// Do nothing
			}

			if(IsKeywordEnabled(materialEditor, "SPACE_WORLD") || IsKeywordEnabled(materialEditor, "SPACE_WORLD_TRIPLANAR"))
			{
				if(IsSpaceWorldTriplanar(materialEditor))
				{
					OnGUI_DrawShaderProperty("_WorldSpaceTriplanarScale", "World Space Triplanar Scale", materialEditor, properties);
					OnGUI_DrawShaderProperty("_WorldSpaceTriplanarBlend", "World Space Triplanar Blend", materialEditor, properties);
				}
				else
				{
					OnGUI_DrawShaderProperty("_WorldSpaceScale", "World Space Scale", materialEditor, properties);
				}
				OnGUI_DrawShaderProperty("_WorldSpaceRotation", "World Space Rotation", materialEditor, properties);
			}
		}
		EndBox();
	}

	private bool IsSpaceWorld(MaterialEditor materialEditor)
	{
		if(materialEditor == null || materialEditor.target == null)
		{
			return false;
		}

		Material mtrl = materialEditor.target as Material;
		if(mtrl == null)
		{
			return false;
		}

		return mtrl.IsKeywordEnabled("SPACE_WORLD") || mtrl.IsKeywordEnabled("SPACE_WORLD_TRIPLANAR");
	}

	private bool IsSpaceWorldTriplanar(MaterialEditor materialEditor)
	{
		if(materialEditor == null || materialEditor.target == null)
		{
			return false;
		}

		Material mtrl = materialEditor.target as Material;
		if(mtrl == null)
		{
			return false;
		}

		return mtrl.IsKeywordEnabled("SPACE_WORLD_TRIPLANAR");
	}

	private void OnGUI_Snow(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		BeginBox();
		{
			OnGUI_DrawShaderProperty("SNOW", "Snow", materialEditor, properties);
			if (IsKeywordEnabled(materialEditor, "SNOW_ON") || IsKeywordEnabled(materialEditor, "SNOW_ON_MASK"))
			{
				OnGUI_DrawShaderProperty("_SnowColor", "Color", materialEditor, properties);
				OnGUI_DrawShaderProperty("_SnowDir", "Direction", materialEditor, properties);
				OnGUI_DrawShaderProperty("_SnowMult", "Intensity1", materialEditor, properties);
				OnGUI_DrawShaderProperty("_SnowPow", "Intensity2", materialEditor, properties);
				OnGUI_DrawShaderProperty("_SnowIntensity", "Intensity3", materialEditor, properties);
				if (IsKeywordEnabled(materialEditor, "SNOW_ON_MASK"))
				{
					if(IsSpaceWorld(materialEditor))
					{
						materialEditor.TexturePropertySingleLine(new GUIContent("Mask (R)"), FindProperty("_SnowMask", properties));
					}
					else
					{
						OnGUI_DrawShaderProperty("_SnowMask", "Mask (R)", materialEditor, properties);
					}
				}
			}
		}
		EndBox();
    }

	private bool OnGUI_DrawShaderProperty(string propertyName, string label, MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty mp = FindProperty(propertyName, properties, false);
        if(mp != null)
        {
            materialEditor.ShaderProperty(mp, label);
        }
        return mp != null;
    }

	private bool IsKeywordEnabled(MaterialEditor materialEditor, string keyword)
    {
        if(materialEditor == null || materialEditor.target == null)
        {
            return false;
        }

        Material mtrl = materialEditor.target as Material;
        if(mtrl == null)
        {
            return false;
        }

        return mtrl.IsKeywordEnabled(keyword);
    }

	private void BeginBox()
	{
		EditorGUILayout.BeginVertical(GUI.skin.GetStyle("Box"));
		EditorGUILayout.Space();
	}

	private void EndBox()
	{
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}
}
