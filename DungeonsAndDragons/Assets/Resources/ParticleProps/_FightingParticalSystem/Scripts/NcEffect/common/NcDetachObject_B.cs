// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class NcDetachObject_B : NcEffectBehaviour_B
{
	// Attribute ------------------------------------------------------------------------
	public		GameObject			m_LinkGameObject;

	// Property -------------------------------------------------------------------------
	public static void Create(GameObject parentObj, GameObject linkObject)
	{
        NcDetachObject_B deObj = parentObj.AddComponent<NcDetachObject_B>();
		deObj.m_LinkGameObject = linkObject;
	}

	// Function --------------------------------------------------------------------
	// Loop Function --------------------------------------------------------------------
	// Control Function -----------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		if (bRuntime)
 			AdjustSpeedRuntime(m_LinkGameObject, fSpeedRate);
	}

	// utility fonction ----------------------------------------------------------------
}
