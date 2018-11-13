
using UnityEngine;

namespace BushidoBurrito
{

public class AreaSpawner : MonoBehaviour
{
	public Vector3 Size = new Vector3(1f, 0, 1f);
	public Vector3 Spacing = new Vector3(1f, 0, 1f);
	public GameObject Spawnable;

	private void Start()
	{
		SpawnArea();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + (Size * 0.5f), Size);
	}

	private void Spawn(Vector3 position)
	{
		var spawn = Instantiate(Spawnable) as GameObject;
		spawn.transform.parent = transform;
		spawn.transform.position = position;
	}

	private void SpawnRow(Vector3 offset)
	{
		if (Spacing.x <= 0f) { return; }

		for (float x = 0f; x < Size.x; x += Spacing.x) {
			var position = transform.position + offset;
			position.x += x;
			Spawn(position);
		}
	}

	private void SpawnArea()
	{
		if (Spacing.z <= 0f) { return; }

		bool oddRow = false;

		for (float z = 0f; z < Size.z; z += Spacing.z) {
			SpawnRow(new Vector3(oddRow ? Spacing.x * 0.5f : 0f, 0f, z));
			oddRow = !oddRow;
		}
	}
}

} // namespace BushidoBurrito
