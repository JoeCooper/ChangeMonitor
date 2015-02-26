using UnityEngine;
using System.Collections;

public class ChangeMonitorExample : MonoBehaviour {

	public int MaximumHealth;
	public int Strength;
	public float WatchThisGrow;

	private ChangeMonitor monitor;

	// Use this for initialization
	void Start () {
		monitor = new ChangeMonitor(true);

		monitor.Add(() => MaximumHealth);
		monitor.Add(() => Strength);
		monitor.Add<float>(() => WatchThisGrow, (newValue, priorValue) => newValue > priorValue);
	}
	
	// Update is called once per frame
	void Update () {
		if(monitor.Evaluate())
		{
			Debug.Log("A change occurred");
		}
	}
}
