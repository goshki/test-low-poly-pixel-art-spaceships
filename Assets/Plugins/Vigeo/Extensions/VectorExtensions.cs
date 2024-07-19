using UnityEngine;

namespace Vigeo {

	public static class VectorExtensions {
		
		public static float SqrDistance(this Vector3 a, Vector3 b) {
			return (a - b).sqrMagnitude;
		}
	}
}
