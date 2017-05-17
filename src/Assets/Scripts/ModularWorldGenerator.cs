using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ModularWorldGenerator : MonoBehaviour
{
	public Module[] Modules;
	public Module StartModule;

	public int Iterations = 5;


	void Start()
	{
		var startModule = Instantiate(StartModule, transform.position, transform.rotation);
		var pendingExits = new List<ModuleConnector>(startModule.GetExits());
        var existingModules = new List<Module>();

        for (int iteration = 0; iteration < Iterations; iteration++)
		{
			var newExits = new List<ModuleConnector>();

			foreach (var pendingExit in pendingExits)
			{
				var newTag = GetRandom(pendingExit.Tags);
				var newModulePrefab = GetRandomWithTag(Modules, newTag);
				var newModule = Instantiate(newModulePrefab);
                existingModules.Add(newModule);
				var newModuleExits = newModule.GetExits();
				var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
				MatchExits(pendingExit, exitToMatch);
				newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
			}
			pendingExits = newExits;
		}

        foreach (var pendingExit in pendingExits)
        {
            //if (pendingExit.tag == "room" || pendingExit.tag == "corridor" || pendingExit.tag == "junction")
            //{
                var newTag = "roomend";
                var newModulePrefab = GetRandomWithTag(Modules, newTag);
                var newModule = Instantiate(newModulePrefab);
                var newModuleExits = newModule.GetExits();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);
            //}
        }
		if(existingModules.Count < 15 )
        {
            foreach (var module in existingModules)
            {
                Destroy(module);
            }
            this.Start();
        }
    }

	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}


	private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[Random.Range(0, array.Length)];
	}


	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
	{
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}


	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
}
