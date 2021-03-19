

using Newtonsoft.Json;

namespace IQB.CustomControl.GooglePlacesSearch
{
	/// <summary>
	/// Auto complete prediction.
	/// </summary>
	public class AutoCompletePrediction
	{
		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		[JsonProperty("description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[JsonProperty("id")]
		public string ID { get; set; }

		/// <summary>
		/// Gets or sets the place identifier.
		/// </summary>
		/// <value>The place identifier.</value>
		[JsonProperty("place_id")]
		public string Place_ID { get; set; }

		/// <summary>
		/// Gets or sets the reference.
		/// </summary>
		/// <value>The reference.</value>
		[JsonProperty("reference")]
		public string Reference { get; set; }
	}

}
