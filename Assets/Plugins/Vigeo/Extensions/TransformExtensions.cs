using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Vigeo {

	public static class TransformExtensions {

		public static bool SetActiveRecursive(this Transform transform, bool active = true) {
			foreach (Transform child in transform) {
				child.gameObject.SetActive(active);
			}
			return active;
		}

		public static Bounds GetBounds(this Transform transform) {
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			foreach (MeshFilter meshFilter in transform.GetComponentsInChildren<MeshFilter>()) {
				Bounds meshBounds = meshFilter.mesh.bounds;
				meshBounds.center = meshFilter.transform.position; // position bounds globally
				if (bounds.size.x == 0 && bounds.size.y == 0) {
					bounds = meshBounds;
				} else {
					bounds.Encapsulate(meshBounds);
				}
			}
			bounds.center = transform.position;
			return bounds;
		}

		public static Vector3 GetCenter(this Transform transform) {
			Vector3 sumOfPositions = Vector3.zero;
			int numberOfObjects = 0;
			foreach (Transform childTransform in transform) {
				if (sumOfPositions == Vector3.zero) {
					sumOfPositions = childTransform.position;
				} else {
					sumOfPositions += childTransform.position;
				}
				numberOfObjects++;
			}
			Vector3 center = sumOfPositions / numberOfObjects;
			return center;
		}

		/// <summary>
		/// Determines if the transform has default local values. If true, calling ResetTransform will change nothing.
		/// </summary>
		public static bool IsDefault(this Transform transform) {
			return transform.localPosition == Vector3.zero && transform.localRotation == Quaternion.identity &&
			transform.localScale == Vector3.one;
		}

		/// <summary>
		/// Resets the transform locally.
		/// </summary>
		public static void ResetTransform(this Transform transform) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}

		/// <summary>
		/// Sets position and optionally rotation in this transform.
		/// </summary>
		public static void Set(this Transform transform, Vector3 position, Quaternion? rotation = null) {
			transform.position = position;
			transform.rotation = rotation.GetValueOrDefault(transform.rotation);
		}

		/// <summary>
		/// Sets rotation and optionally position in this transform.
		/// </summary>
		public static void Set(this Transform transform, Quaternion rotation, Vector3? position = null) {
			transform.rotation = rotation;
			transform.position = position.GetValueOrDefault(transform.position);
		}

		/// <summary>
		/// Sets local position and optionally local rotation in this transform.
		/// </summary>
		public static void SetLocal(this Transform transform, Vector3 localPosition, Quaternion? localRotation = null) {
			transform.localPosition = localPosition;
			transform.localRotation = localRotation.GetValueOrDefault(transform.localRotation);
		}

		/// <summary>
		/// Sets local rotation and optionally local position in this transform.
		/// </summary>
		public static void SetLocal(this Transform transform, Quaternion localRotation, Vector3? localPosition = null) {
			transform.localRotation = localRotation;
			transform.position = localPosition.GetValueOrDefault(transform.position);
		}

		public static void SetPositionX(this Transform transform, float x) {
			transform.position = transform.position.With(x: x);
		}

		public static void SetPositionY(this Transform transform, float y) {
			transform.position = transform.position.With(y: y);
		}

		public static void SetPositionZ(this Transform transform, float z) {
			transform.position = transform.position.With(z: z);
		}

		public static void SetLocalPositionX(this Transform transform, float x) {
			transform.localPosition = transform.localPosition.With(x: x);
		}

		public static void SetLocalPositionY(this Transform transform, float y) {
			transform.localPosition = transform.localPosition.With(y: y);
		}

		public static void SetLocalPositionZ(this Transform transform, float z) {
			transform.localPosition = transform.localPosition.With(z: z);
		}

		public static void ChangeLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
			transform.localPosition = transform.localPosition.With(x: x, y: y, z: z);
		}

		public static void SetEulerAnglesX(this Transform transform, float newX) {
			transform.eulerAngles = new Vector3(newX, transform.eulerAngles.y, transform.eulerAngles.z);
		}

		public static void SetEulerAnglesY(this Transform transform, float newY) {
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
		}

		public static void SetEulerAnglesZ(this Transform transform, float newZ) {
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZ);
		}

		public static void SetLocalEulerAnglesX(this Transform transform, float newX) {
			transform.localEulerAngles = new Vector3(newX, transform.localEulerAngles.y, transform.localEulerAngles.z);
		}

		public static void SetLocalEulerAnglesY(this Transform transform, float newY) {
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
		}

		public static void SetLocalEulerAnglesZ(this Transform transform, float newZ) {
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newZ);
		}


		public static void SetLocalScaleX(this Transform transform, float newX) {
			transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
		}

		public static void SetLocalScaleY(this Transform transform, float newY) {
			transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);
		}

		public static void SetLocalScaleZ(this Transform transform, float newZ) {
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZ);
		}

		/// <summary>
		/// Transforms a point in local space relative to fromTransform into local space relative to toTransform.
		/// </summary>
		/// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
		/// <param name="fromTransform">From transform.</param>
		/// <param name="toTransform">To transform.</param>
		/// <param name="localPoint">Local point.</param>
		public static Vector3 TransformPointTo(this Transform transform, Vector3 localPoint, Transform toTransform) {
			return toTransform.InverseTransformPoint(transform.TransformPoint(localPoint));
		}

		///
		///Paths
		///

		/// <summary>
		/// Recursively travels to the top-level parent, or the parent at level maxLevels and outputs a string of the hierarchy path of the transform
		/// </summary>
		public static string HierarchyPath(this Transform t, int maxLevels = 0) {
			string path = "";
			//Transform p = t.GetParent(maxLevels);
			//return p.name;
			Transform[] parents = t.GetParents(maxLevels);
			for (int i = parents.Length - 1; i >= 0; i--) {
				path += parents[i].name;
				path += "/";
			}
			path += t.name;
			return path;
		}

		/// <summary>
		/// Destroys all children.
		/// </summary>
		/// <param name="transform">The parent transform.</param>
		public static void DestroyAllChildren(this Transform transform) {
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Object.Destroy(transform.GetChild(i).gameObject);
			}
		}

		public static void DestroyAllChildrenImmediate(this Transform transform) {
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Object.DestroyImmediate(transform.GetChild(i).gameObject);
			}
		}

		/// <summary>
		/// Gets the children of the transform. 
		/// Remember that you can also loop through children using Transform's enumerator: foreach (Transform child in transform)
		/// </summary>
		/// <returns>The children.</returns>
		/// <param name="current">Current.</param>
		public static Transform[] GetChildren(this Transform transform) {
			Transform[] children = new Transform[transform.childCount];
			for (int i = 0; i < children.Length; i++) {
				children[i] = transform.GetChild(i);
			}
			return children;
		}

		/// <summary>
		/// Gets the siblings of a transform.
		/// </summary>
		/// <returns>The siblings.</returns>
		/// <param name="current">Current.</param>
		public static Transform[] GetSiblings(this Transform transform) {
			Transform[] siblingsIncludingSelf = transform.parent.GetChildren();
			Transform[] siblings = new Transform[siblingsIncludingSelf.Length - 1];
			int siblingsIndex = 0;
			for (int i = 0; i < siblingsIncludingSelf.Length; i++) {
				if (siblingsIncludingSelf[i] != transform) {
					siblings[siblingsIndex] = siblingsIncludingSelf[i];
					siblingsIndex++;
				}
			}
			return siblings;
		}


		///
		///Finding
		///

		/// <summary>
		/// Recursively travels to the top-level parent, or the parent at level maxLevels
		/// </summary>
		public static Transform GetParent(this Transform transform, int maxLevels = 0) {
			maxLevels--;
			if (maxLevels == 0) {
				return transform.parent;
			} else if (transform.parent != null) {
				return GetParent(transform.parent, maxLevels);
			} else {
				return transform;
			}
		}

		/// <summary>
		/// Recursively travels to the top-level parent, or the parent at level maxLevels
		/// </summary>
		public static Transform[] GetParents(this Transform t, int maxLevels = 0, IList<Transform> parents = null) {
			if (parents == null)
				parents = new List<Transform>();
			maxLevels--;
			if (t.parent != null) {
				parents.Add(t.parent);
				if (maxLevels == 0) {
					return parents.ToArray();
				} 
				return GetParents(t.parent, maxLevels, parents);
			} else {
				return parents.ToArray();
			}
		}

		/// <summary>
		/// Recursively travels to the top-level parent, or the parent at level maxLevels
		/// </summary>
		public static int GetNumParents(this Transform t) {
			return t.GetParents().Length;
		}


		/// <summary>
		/// Finds the first transform in the descendants of the transform with a specified name.
		/// Searches using a breadth-first search, so it doesn't go too deeply too quickly.
		/// </summary>
		/// <returns>The in children.</returns>
		/// <param name="current">The start transform.</param>
		/// <param name="name">The name of the child to search for.</param>
		public static Transform FindInChildren(this Transform current, string name) {
			return FindInChildren<Transform>(current, name);
		}

		/// <summary>
		/// Breadth first search algorithm to find a child of a particular type, optionally with a particular name 
		/// in the full ancestry below a certain node.
		/// (We use breadth first so that we find the "shallowest" child possible, so we don't recursively go very
		/// deep very fast. It's a more complicated algorithm, but will be faster in most use cases.)
		/// </summary>
		/// <returns>A child Transform, if one exists with a given name.</returns>
		/// <param name="name">The exact name to search for.</param>
		public static T FindInChildren<T>(this Transform current, string name = null) where T : Component {
			return FindInChildren<T>(current, t => t.name == name);
		}

		/// <summary>
		/// Breadth first search algorithm to find a child of a particular type, optionally according to a particular
		/// predicate in the full ancestry below a certain node.
		/// (We use breadth first so that we find the "shallowest" child possible, so we don't recursively go very
		/// deep very fast. It's a more complicated algorithm, but will be faster in most use cases.)
		/// </summary>
		/// <returns>A child Transform, if one exists that passes the test of the predicate function.</returns>
		/// <param name="predicate">An optional predicate that returns true or false depending on whether
		/// we want to consider the given object.</param>
		public static T FindInChildren<T>(this Transform current, Predicate<T> predicate = null) where T : Component {
			// Keep queue around so that we allocate as little memory as possible for
			// multiple find calls.
			if (_objectQueue == null)
				_objectQueue = new Queue<Transform>();

			foreach (Transform child in current) {
				if (child.gameObject.activeInHierarchy)
					_objectQueue.Enqueue(child);
			}

			T result = FindInChildrenFromMainQueue<T>(predicate);

			// Clear up after ourselves
			_objectQueue.Clear();

			return result;
		}

		/// <summary>
		/// Private/internal function: using _objectQueue (constructed by FindInChildren for example),
		/// search for the objects within the queue, as well as within their children.
		/// </summary>
		/// <returns>A child component, if one exists with a given name.</returns>
		/// <param name="predicate">A predicate to test on each object of the given type.</param>
		/// <typeparam name="T">The specific component type to search for.</typeparam>
		static T FindInChildrenFromMainQueue<T>(Predicate<T> predicate) where T : Component {
			while (_objectQueue.Count > 0) {

				var child = _objectQueue.Dequeue();

				var comp = child.GetComponent<T>();
				if (comp) {
					if (predicate == null || predicate(comp)) {
						return comp;
					}
				}

				foreach (Transform subChild in child) {
					_objectQueue.Enqueue(subChild);
				}
			}

			return null;
		}

		// Keep a global pool around so we don't have to keep allocating a new one.
		// Used for the above breadth first search system.
		static Queue<Transform> _objectQueue;

		/// <summary>
		/// Finds all transforms in the descendants of the transform with a specified name.
		/// </summary>
		/// <returns>The all in children.</returns>
		/// <param name="current">Current.</param>
		/// <param name="name">Name.</param>
		public static Transform[] FindAllInChildren(this Transform current, string name) {
			return current.FindAllInChildrenList(name).ToArray();
		}

		public static List<Transform> FindAllInChildrenList(this Transform current, string name, List<Transform> transforms = null) {
			if (transforms == null)
				transforms = new List<Transform>();
			//current.name.Contains(name)
			if (current.name == name)
				transforms.Add(current);
			for (int i = 0; i < current.childCount; ++i) {
				current.GetChild(i).FindAllInChildrenList(name, transforms);
			}
			return transforms;
		}

		/// <summary>
		/// Find the nearest component in a list to this Transform in 3d space, returning the distance found.
		/// </summary>
		public static T Nearest<T>(this Transform current, IEnumerable<T> objectList, out float distance) where T : Component {
			var currPos = current.position;

			float nearestDist = float.MaxValue;
			T nearestObj = null;
			foreach (var obj in objectList) {
				float dist = Vector3.Distance(currPos, obj.transform.position);
				if (nearestObj == null || dist < nearestDist) {
					nearestObj = obj;
					nearestDist = dist;
				}
			}

			distance = nearestDist;

			return nearestObj;
		}

		/// <summary>
		/// Find the nearest component in a list to this Transform in 3d space.
		/// </summary>
		public static T Nearest<T>(this Transform current, IEnumerable<T> objectList) where T : Component {
			float distance;
			return Nearest(current, objectList, out distance);
		}
	}
}
