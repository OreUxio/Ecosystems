using System.Collections;
using System.Collections.Generic;

interface Ecosystem {
	void AddSpecies(string name, float selfInteraction, float birthRate, float population=0f);
	void AddSpecies(string name, float m_i, float b_0, float d_0, float a_ii0, float gamma=0.5f, float beta=0.75f, float population=0f);
	void RemoveSpecies(string name);
	
	void AddInteraction(string predator, string prey, float interactionRate, float conversionEfficiency);
	void AddInteraction(string predator, string prey, float a_0, float e, float s, float k);

	void Integrate(float timestep);

	float GetPopulation(string name);
	Dictionary<string, float> GetAllPopulations();

	float GetInteraction(string predator, string prey);

	void SetPopulation(string name, float population);
	void ChangePopulation(string name, float amount);
}
