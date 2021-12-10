using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StylizedIce_Plane_InWorldSpace : MonoBehaviour 
{
	[Range(0, 3)]
	public int tangentBaseCorrection = 0;

	private MaterialPropertyBlock mpb = null;
	
	private int spacePropertyId = 0;

	private int tangentCorrectionPropId = 0;

	private void Awake()
	{
		spacePropertyId = Shader.PropertyToID("SPACE");
		tangentCorrectionPropId = Shader.PropertyToID("_TangentCorrection");
	}

	private void Update () 
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		if(mr != null && mr.sharedMaterial != null)
		{
			if((int)mr.sharedMaterial.GetFloat(spacePropertyId) == 0)
			{
				mr.SetPropertyBlock(null);
			}
			else
			{
				if(mpb == null)
				{
					mpb = new MaterialPropertyBlock(); 
				}

				mpb.SetFloat(tangentCorrectionPropId, -transform.eulerAngles.y * Mathf.Deg2Rad + tangentBaseCorrection * (Mathf.PI * 0.5f));
				mr.SetPropertyBlock(mpb); 
			}
		}
	}

	private void OnDestroy()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		if(mr != null)
		{
			mr.SetPropertyBlock(null);
		}
	}
}
