
using UnityEngine;

namespace BushidoBurrito
{

public class QuadTreeBehaviour : MonoBehaviour
{
	public Vector3 Size = new Vector3(1f, 0f, 1f);
	public int MemberCountPerCell = 10;
	public string TargetTag;
	public bool DebugDraw = false;

	public QuadTree Root { get; private set; }
	private GameObject[] Targets;

	private void Update()
	{
		if (this.Targets == null) {
			if (string.IsNullOrEmpty(this.TargetTag)) {
				Debug.LogWarning("quad tree has no target tag: " + this.name);
			}

			this.Targets = GameObject.FindGameObjectsWithTag(this.TargetTag);
		}

		this.Root = new QuadTree(
			new Rect(
				transform.position.x,
				transform.position.z,
				this.Size.x,
				this.Size.z),
			this.MemberCountPerCell);

		foreach (var target in this.Targets) {
			this.Root.Add(target.transform);
		}

		if (this.DebugDraw) {
			this.Root.DebugDraw();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + (this.Size * 0.5f), this.Size);
	}
}

} // namespace
