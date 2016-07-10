using UnityEngine;
using System.Collections;

[System.Serializable]
public class Talasalitaan{
	public string salita;
	public string kasingKahulugan;
	public string halimbawa;
	public bool activated;
	public bool newlyActivated;

	public Talasalitaan(string salita, string kasingKahulugan, string halimbawa, bool activated, bool newlyActivated){
		this.salita = salita;
		this.kasingKahulugan = kasingKahulugan;
		this.halimbawa = halimbawa;
		this.activated = activated;
		this.newlyActivated = newlyActivated;
	}
}