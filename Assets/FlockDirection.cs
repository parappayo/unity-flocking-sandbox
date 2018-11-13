
using UnityEngine;

namespace BushidoBurrito
{

public class FlockDirection : MonoBehaviour, IDirectionProvider
{
	public Transform[] Neighbours;

	public uint NeighbourCount = 10;

	public float NeighbourDistance = 5f;

	public float AlignmentWeight = 1f;
	public float CohesionWeight = 1f;
	public float SeparationWeight = 1f;

	private void Start()
	{
		Neighbours = new Transform[NeighbourCount];
		FindNeighbours();
	}

	public Vector3 GetDirection()
	{
		if (Neighbours.Length == 0) { return Vector3.zero; }

		Vector3 alignment = Vector3.zero;
		Vector3 cohesion = Vector3.zero;
		Vector3 separation = Vector3.zero;

		foreach (var neighbour in Neighbours) {
			if (!neighbour) { continue; }

			var deltaPos = neighbour.position - transform.position;
			alignment += neighbour.forward;
			cohesion += deltaPos;

			if (deltaPos.sqrMagnitude < SeparationWeight * SeparationWeight) {
				separation -= deltaPos.normalized * SeparationWeight - deltaPos;
			}
		}

		var result = (alignment * AlignmentWeight + cohesion * CohesionWeight + separation * SeparationWeight) / Neighbours.Length;
		return result;
	}

	public void FindNeighbours()
	{
		uint count = 0;

		// this implementation is unnecessarily slow
		foreach (Transform sibling in transform.parent) {
			var distanceSquared = (sibling.position - transform.position).sqrMagnitude;

			if (distanceSquared < NeighbourDistance * NeighbourDistance) {
				Neighbours[count] = sibling;
				count++;

				if (count >= Neighbours.Length) {
					return;
				}
			}
		}
	}
}

} // namespace BushidoBurrito
