using System.Collections;
using System.Collections.Generic;

class RungeEcosystem : Ecosystem {
	// each species has a List of interactions with other species.
	public class Interaction {
		public Species otherSpecies;
		public float rate;
		public float transferredAmount;
		
		public Interaction(Species _otherSpecies, float _rate) {
			otherSpecies = _otherSpecies;
			rate = _rate;
			transferredAmount = 0f;
		}
	}
	public class Species {
		public Dictionary<string, Interaction> interactions;
		public float population;
		public float birthRate;

		public float k_1, k_2, k_3, y_plus_k1, y_plus_k2, y_plus_k3; // plus 3 not necessary but makes it more readable
		
		public Species(string name, float _population, float _birthRate, float _selfInteraction) {
			interactions = new Dictionary<string, Interaction>();
			population = _population;
			birthRate = _birthRate;
			interactions.Add(name, new Interaction(this, _selfInteraction));
		}
		public void AddInteraction(string name, Species other, float interactionRate) {
			interactions.Add(name, new Interaction(other, interactionRate));
		}
	}
	
	public Dictionary<string, Species> system { get; private set; }
	
	public RungeEcosystem() {
		system = new Dictionary<string, Species>();
	}
	
	public void AddSpecies(string name, float selfInteraction, float birthRate, float population=0f) {
		if (system.ContainsKey(name)) {
			throw new System.ArgumentException("system already contains a species with that name!");
		}
		Species newSpecies = new Species(name, population, birthRate, -selfInteraction); // -!!!
		system.Add(name, newSpecies);
	}
	public void AddSpecies(string name, float m_i, float b_0, float d_0, float a_ii0, float gamma=0.5f, float beta=0.75f, float population=0f) {
		throw new System.AccessViolationException("function not implemented!");
	}
	public void RemoveSpecies(string name) {
		//TODO: remove all links instead of this, or just set population to zero or ignore this function completely cuz it's dum
		if (!system.ContainsKey(name)) {
			throw new System.ArgumentException("system does not contain a species with that name!");
		}
		SetPopulation(name, 0f);
	}
	
	public void AddInteraction(string predator, string prey, float interactionRate, float conversionEfficiency) {
		system[prey].AddInteraction(predator, system[predator], -interactionRate);
		system[predator].AddInteraction(prey, system[prey], interactionRate * conversionEfficiency);
	}
	public void AddInteraction(string predator, string prey, float a_0, float e, float s, float k) {
		throw new System.AccessViolationException("function not implemented!");
	}

	public void Integrate(float timestep) {
		float h = timestep;
		float h2 = timestep / 2f;
		float h6 = timestep / 6f;
		// k_1
		foreach (Species species in system.Values) {
			species.k_1 = species.birthRate * species.population;
			foreach (Interaction interaction in species.interactions.Values) {
				species.k_1 += interaction.rate * interaction.otherSpecies.population * species.population;
			}
			species.y_plus_k1 = species.population + h2*species.k_1;
		}
		// k_2
		foreach (Species species in system.Values) {
			species.k_2 = species.birthRate * species.y_plus_k1;
			foreach (Interaction interaction in species.interactions.Values) {
				species.k_2 += interaction.rate * interaction.otherSpecies.y_plus_k1 * species.y_plus_k1;
			}
			species.y_plus_k2 = species.population + h2*species.k_2;
		}
		// k_3
		foreach (Species species in system.Values) {
			species.k_3 = species.birthRate * species.y_plus_k2;
			foreach (Interaction interaction in species.interactions.Values) {
				species.k_3 += interaction.rate * interaction.otherSpecies.y_plus_k2 * species.y_plus_k2;
			}
			species.y_plus_k3 = species.population + h*species.k_3;
		}
		// k_4
		foreach (Species species in system.Values) {
			float k_4 = species.birthRate * species.y_plus_k3;
			foreach (Interaction interaction in species.interactions.Values) {
				k_4 += interaction.rate * interaction.otherSpecies.y_plus_k3 * species.y_plus_k3;
			}
			// h/6(k_1 + 2k_2 + 2k_3 + k_4)
			species.population += h6*(species.k_1 + 2f*species.k_2 + 2f*species.k_3 + k_4);
		}
	}
	
	public float GetPopulation(string name) {
		return system[name].population;
	}
	public Dictionary<string, float> GetPopulations(List<string> names) {
		Dictionary<string, float> populations = new Dictionary<string, float>();
		foreach (string name in names) {
			populations.Add(name, system[name].population);
		}
		return populations;
	}
	public Dictionary<string, float> GetAllPopulations() {
		Dictionary<string, float> populations = new Dictionary<string, float>();
		foreach (KeyValuePair<string, Species> species in system) {
			populations.Add(species.Key, species.Value.population);
		}
		return populations;
	}
	public float GetInteraction(string predator, string prey) {
		throw new System.AccessViolationException("function not implemented!");
	}
	
	public void SetPopulation(string name, float population) {
		system[name].population = population;
	}
	public void ChangePopulation(string name, float amount) {
		system[name].population += amount;
	}
}

