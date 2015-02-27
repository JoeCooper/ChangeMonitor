using System.Collections.Generic;
using System;

public class ChangeMonitor {

	public delegate T Query<T>();
	public delegate bool Evaluator<T>(T currentValue, T previousValue);

	private interface IProperty {
		bool Evaluate();
	}

	private class Property<T>: IProperty {
		public Query<T> query;
		public Evaluator<T> evaluator;
		public T lastObservedValue;

		#region IProperty implementation
		public bool Evaluate ()
		{
			var newValue = query();
			var evaluatesAsTrue = evaluator(newValue, lastObservedValue);
			lastObservedValue = newValue;
			return evaluatesAsTrue;
		}
		#endregion
	}

	private readonly ICollection<IProperty> properties;
	private readonly bool evaluateAsTrueOnFirstPass;
	private bool firstPassPending;

	public ChangeMonitor(bool evaluateAsTrueOnFirstPass = true)
	{
		properties = new HashSet<IProperty>();
		this.evaluateAsTrueOnFirstPass = evaluateAsTrueOnFirstPass;
		firstPassPending = true;
	}
	
	public void Add<T>(Query<T> query) {
		var property = new Property<T> {
			query = query,
			evaluator = (alfa, bravo) => alfa == null ? bravo != null : !alfa.Equals(bravo),
			lastObservedValue = query()
		};
		properties.Add(property);
	}

	public void Add<T>(Query<T> query, Evaluator<T> evaluator) {
		var property = new Property<T> {
			query = query,
			evaluator = evaluator,
			lastObservedValue = query()
		};
		properties.Add(property);
	}

	public bool Evaluate() {
		var result = firstPassPending && evaluateAsTrueOnFirstPass;
		foreach(var property in properties)
		{
			result |= property.Evaluate();
		}
		firstPassPending = false;
		return result;
	}

	public static bool ArrayEvaluator<T>(T[] alfa, T[] bravo)
	{
		if(alfa != bravo)
		{
			return true;
		}
		else if(alfa.Length != bravo.Length)
		{
			return true;
		}
		else if(alfa.Length > 0) {
			var result = false;
			for(int i = 0; i < alfa.Length && !result; i++)
			{
				var a = alfa[i];
				var b = bravo[i];
				result |= a == null ? b != null : !a.Equals(b);
			}
			return result;
		}
		else {
			return false;
		}
	}
}
