using System;
using System.Collections.Generic;

namespace PaladinDataCleanup
{
	/// <summary>
	/// This class holds a single depth measurement including:
	/// Depth, ROP, TG, C1-C4, and gamma
	/// It represents all the data (that we use) on a single row of
	/// the Bloodhound excel file or at a single depth on a mudlog.
	/// </summary>
	public class Measurement : IComparable
	{
		public int Depth { get; set; }

		public double ROP { get; set; }

		public double TG { get; set; }

		public double C1 { get; set; }

		public double C2 { get; set; }

		public double C3 { get; set; }

		public double C4 { get; set; }

		public double Gamma { get; set; }

		public Measurement()
		{
			Depth = 0;
			ROP = 0;
			TG = 0;
			C1 = 0;
			C2 = 0;
			C3 = 0;
			C4 = 0;
			Gamma = 0;
		}

		public Measurement(int depth, double rop, double tg, double c1, double c2, double c3, double c4, double gamma)
		{
			Depth = depth;
			ROP = rop;
			TG = tg;
			C1 = c1;
			C2 = c2;
			C3 = c3;
			C4 = c4;
			Gamma = gamma;
		}

		/// <summary>
		/// Compares whether two Measurements have identical depth.
		/// </summary>
		/// <returns> negative if less than, positive if greater than, 0 if equal to</returns>
		/// <param name="x">Measurement to compare against</param>
		public int CompareTo(object x)
		{
			if(x == null)
				return 1;

			var otherMeasurement = x as Measurement;
			if(otherMeasurement == null) {
				throw new ArgumentException("Object is not a Measurement");
			} else {
				return otherMeasurement.Depth.CompareTo(Depth);
			}
		}
	}

	/// <summary>
	/// This is the class that actally holds a list of Measurements.
	/// It has methods to actually go through and clean up the data:
	/// eliminate duplicates, interpolate gaps, etc.
	/// These operations all modify the object.
	/// It can also export the data (optionally trimmed to a given depth range)
	/// to .las type tab-delimited text.
	/// </summary>
	public class WorkingSet
	{
		List<Measurement> Data;

		public WorkingSet()
		{
			Data = new List<Measurement>();
		}

		public void AddMeasurement(Measurement m)
		{
			Data.Add(m);
		}

		public void AddMeasurement(IEnumerable<Measurement> ms)
		{
			foreach(var m in ms) {
				AddMeasurement(m);
			}
		}

		/// <summary>
		/// Removes all but one measurement with the same depth.
		/// It leaves behind the first measurement at a given depth
		/// and removes the rest up to the next higher depth.
		/// 
		/// It's probably not all that efficient or clever but works
		/// and is consistent.
		/// </summary>
		public void RemoveDuplicates()
		{
			for(int i = 0; i < Data.Count-1; i++) {
				var d = Data[i].Depth;
				var dNext = Data[i + 1].Depth;

				while(d >= dNext) {
					Data.RemoveAt(i + 1);
					if(Data.Count < i) {
						dNext = Data[i + 1].Depth;
					} else {
						// We've removed the last item in the list, stop.
						break;
					}
				}
			}
		}
	}
}

