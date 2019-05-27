using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public Bloom[] blooms;
	public Manager manager;
	public Render render;

	public Text bloomEnabledText;
	public Text bloomStrengthText;
	public Text autoZoomText;
	public Text autoThrustText;
	public Text resetGalaxyText;

	public bool resetConfirm = false;

	void Update () {
		if (blooms[0].enabled) {
			bloomEnabledText.text = "Bloom: Enabled";
		} else {
			bloomEnabledText.text = "Bloom: Disabled";
		}
		if (render.autoZoom) {
			autoZoomText.text = "Auto-Zoom:\nEnabled";
		} else {
			autoZoomText.text = "Auto-Zoom:\nDisabled";
		}
		if (manager.autoThrust) {
			autoThrustText.text = "Auto-Thrust:\nEnabled";
		} else {
			autoThrustText.text = "Auto-Thrust:\nDisabled";
		}
		if (resetConfirm) {
			resetGalaxyText.text = "Click Again to Confirm";
		} else {
			resetGalaxyText.text = "New Galaxy\nLose Progress";

		}
		if (blooms[0].bloomIntensity == 0.25f) {
			bloomStrengthText.text = "Bloom Strength: Subtle";
		} else if (blooms[0].bloomIntensity == 0.5f) {
			bloomStrengthText.text = "Bloom Strength:\nMild";
		} else if (blooms[0].bloomIntensity == 1) {
			bloomStrengthText.text = "Bloom Strength: Moderate";
		} else if (blooms[0].bloomIntensity == 2) {
			bloomStrengthText.text = "Bloom Strength: Strong";
		} else if (blooms[0].bloomIntensity == 3) {
			bloomStrengthText.text = "Bloom Strength: Very Strong";
		} 
	}

	public void ToggleSetting (int index) {
		if (index == 0) {
			foreach (Bloom bloom in blooms) {
				bloom.enabled = !bloom.enabled;
			}
		} else if (index == 1) {
			render.autoZoom = !render.autoZoom;
		} else if (index == 2) {
			manager.autoThrust = !manager.autoThrust;
		} else if (index == 3) {
			float intensity;
			if (blooms[0].bloomIntensity == 0.25f) {
				intensity = 0.5f;
			} else if (blooms[0].bloomIntensity == 0.5f) {
				intensity = 1;
			} else if (blooms[0].bloomIntensity == 1f) {
				intensity = 2;
			} else if (blooms[0].bloomIntensity == 2f) {
				intensity = 3;
			} else if (blooms[0].bloomIntensity == 3f) {
				intensity = 0.25f;
			} else {
				intensity = 0.25f;
				Debug.Log ("Unexpected bloom intensity");
			}
			foreach (Bloom bloom in blooms) {
				bloom.bloomIntensity = intensity;
			}
		} else if (index == 4) {
			if (resetConfirm) {
				manager.Awake ();
				resetConfirm = false;
			} else {
				resetConfirm = true;
			}
		}
	}
}
