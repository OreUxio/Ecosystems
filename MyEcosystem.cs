using System.Collections;
using System.Collections.Generic;

class MyEcosystem : Ecosystem {
	// each species has a List of interactions with other species.
	public class Interaction {
		public Species prey;
		public float interactionRate;
		public float conversionEfficiency;
		
		public Interaction(Species _prey, float _interactionRate, float _conversionEfficiency) {
			prey = _prey;
			interactionRate = _interactionRate;
			conversionEfficiency = _conversionEfficiency;
		}
	}
	public class Species {
		public List<Interaction> interactions;
		public float population;
		public float birthRate;

		public Species(float _population, float _birthRate, float _selfInteraction) {
			interactions = new List<Interaction>();
			population = _population;
			birthRate = _birthRate;
			interactions.Add(new Interaction(this, _selfInteraction, 0f)); //conversionEfficiency of 0 for selfInteraction
		}
		public void AddInteraction(Species prey, float interactionRate, float conversionEfficiency) {
			interactions.Add(new Interaction(prey, interactionRate, conversionEfficiency));
		}
	}

	public Dictionary<string, Species> system { get; private set; }

	public MyEcosystem() {
		system = new Dictionary<string, Species>();
	}

	public void AddSpecies(string name, float selfInteraction, float birthRate, float population=0f) {
		if (system.ContainsKey(name)) {
			throw new System.ArgumentException("system already contains a species with that name!");
		}
		Species newSpecies = new Species(population, birthRate, selfInteraction);
		system.Add(name, newSpecies);
	}
	public void AddSpecies(string name, float m_i, float b_0, float d_0, float a_ii0, float gamma=0.5f, float beta=0.75f, float population=0f) {
		throw new System.AccessViolationException("function not implemented!");
	}
	public void RemoveSpecies(string name) {
		//TODO: remove all links instead of this, or just ignore this function completely cuz it's dum
		if (!system.ContainsKey(name)) {
			throw new System.ArgumentException("system does not contain a species with that name!");
		}
		SetPopulation(name, 0f);
	}

	public void AddInteraction(string predator, string prey, float interactionRate, float conversionEfficiency) {
		system[predator].AddInteraction(system[prey], interactionRate, conversionEfficiency);
	}
	public void AddInteraction(string predator, string prey, float a_0, float e, float s, float k) {
		throw new System.AccessViolationException("function not implemented!");
	}

	public void Integrate(float timestep) {
		// get a record of the current populations that will be written back later
		Dictionary<Species, float> newPopulations = new Dictionary<Species, float>();
		foreach (Species species in system.Values) {
			newPopulations.Add(species, 0f);
		}
		// apply the birthRates and interactions
		foreach (Species predator in system.Values) {
			// birthRate
			newPopulations[predator] += predator.birthRate * predator.population;
			// interactions
			foreach (Interaction interaction in predator.interactions) {
				Species prey = interaction.prey;
				float amountEaten = predator.population * prey.population * interaction.interactionRate;
				newPopulations[prey] -= amountEaten;
				newPopulations[predator] += amountEaten * interaction.conversionEfficiency;
			}
		}
		// write back the populations
		foreach (Species species in system.Values) {
			species.population += newPopulations[species] * timestep;
		}
	}

	public float GetPopulation(string name) {
		return system[name].population;
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
