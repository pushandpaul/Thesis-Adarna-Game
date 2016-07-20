using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	private Light light;
	public float lightIntensity;

	void Awake(){
		light = this.GetComponent<Light>();
		lightIntensity = light.intensity;
	}

	public void setLightIntensity(float intensity){
		light.intensity = intensity;
	}

	public void fadeLightIntensity(float targetIntensity, float duration){
		StartCoroutine(startFading(light.intensity, targetIntensity, duration));
	}

	IEnumerator startFading(float initialIntensity, float targetIntensity, float duration){
		float startTime = Time.time;
		float endTime = startTime + duration;
		while(Time.time <= endTime){
			float t = (Time.time - startTime)/duration;
			light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t);
			yield return null;
		}
	}
}
