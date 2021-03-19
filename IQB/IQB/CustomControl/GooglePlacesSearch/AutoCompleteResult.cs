

using System.Collections.Generic;
using Newtonsoft.Json;

namespace IQB.CustomControl.GooglePlacesSearch
{
	/// <summary>
	/// Auto complete result.
	/// </summary>
	public class AutoCompleteResult
	{
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[JsonProperty("status")]
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the auto complete places.
		/// </summary>
		/// <value>The auto complete places.</value>
		[JsonProperty("predictions")]
		public List<AutoCompletePrediction> AutoCompletePlaces { get; set; }
	}

}
