
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace BushidoBurrito
{

public class QuadTree
{
	public Rect Bounds { get; private set; }
	public QuadTree[] Subtrees { get; private set; }
	public int MemberCountPerCell { get; private set; }

	private List<Transform> Members;

	public QuadTree(Rect bounds, int memberCountPerCell)
	{
		this.Bounds = bounds;
		this.Subtrees = new QuadTree[4];
		this.MemberCountPerCell = memberCountPerCell;
	}

	public void Add(Transform member)
	{
		if (this.Members != null) {
			this.Members.Add(member);

			if (this.Members.Count > this.MemberCountPerCell &&
				this.MemberCountPerCell > 0) {

				SplitIntoSubtrees();
			}

		} else if (this.Subtrees[0] != null) {
			AddToSubtree(member);

		} else {
			this.Members = new List<Transform>();
			this.Members.Add(member);
		}
	}

	public bool Remove(Transform member)
	{
		if (!ContainsXZ(member.position)) {
			return false;
		}

		if (this.Members != null) {
			return this.Members.Remove(member);
		}

		foreach (var subtree in this.Subtrees) {
			if (subtree.Remove(member)) {
				return true;
			}
		}

		return false;
	}

	public void Clear()
	{
		if (this.Members != null) {
			this.Members.Clear();
		}

		for (int i = 0; i < this.Subtrees.Length; i++) {
			if (this.Subtrees[i] == null) { continue; }
			this.Subtrees[i].Clear();
			this.Subtrees[i] = null;
		}
	}

	public int MemberCount
	{
		get {
			int result = 0;

			if (this.Members != null) {
				result += this.Members.Count;
			}

			if (this.Subtrees[0] != null) {
				foreach (var subtree in this.Subtrees) {
					result += subtree.MemberCount;
				}
			}

			return result;
		}
	}

	public bool Contains(Vector2 position)
	{
		return this.Bounds.Contains(position);
	}

	public bool ContainsXZ(Vector3 position)
	{
		return this.Bounds.Contains(new Vector2(position.x, position.z));
	}

	public bool Overlaps(Rect area)
	{
		return area.xMax >= this.Bounds.xMin &&
			area.xMin <= this.Bounds.xMax &&
			area.yMax >= this.Bounds.yMin &&
			area.yMin <= this.Bounds.yMax;
	}

	public List<Transform> Find(Rect targetArea, int resultsLimit)
	{
		var results = new List<Transform>();
		Find(targetArea, results, resultsLimit);
		return results;
	}

	public List<Transform> FindXZ(Vector3 position, float range, int resultsLimit)
	{
		var targetArea = new Rect(
			position.x - range,
			position.z - range,
			range * 2f,
			range * 2f);

		return Find(targetArea, resultsLimit);
	}

	public int Find(Rect targetArea, List<Transform> results, int resultsLimit)
	{
		if (results == null) { return 0; }
		if (!Overlaps(targetArea)) { return 0; }

		int resultsCount = 0;

		if (this.Members != null) {
			for (int i = 0; i < this.Members.Count; i++) {
				if (resultsCount >= resultsLimit) { break; }

				var member = this.Members[i];
				var memberPositionXZ = new Vector2(member.position.x, member.position.z);

				if (targetArea.Contains(memberPositionXZ)) {
					results.Add(member);
					resultsCount++;
				}
			}
		}

		if (this.Subtrees[0] != null) {
			foreach (var subtree in this.Subtrees) {
				resultsCount += subtree.Find(targetArea, results, resultsLimit - resultsCount);
			}
		}

		return resultsCount;
	}

	public void DebugDraw()
	{
		Debug.DrawLine(new Vector3(this.Bounds.xMin, 0f, this.Bounds.yMin), new Vector3(this.Bounds.xMax, 0f, this.Bounds.yMin), Color.blue);
		Debug.DrawLine(new Vector3(this.Bounds.xMin, 0f, this.Bounds.yMax), new Vector3(this.Bounds.xMax, 0f, this.Bounds.yMax), Color.blue);
		Debug.DrawLine(new Vector3(this.Bounds.xMin, 0f, this.Bounds.yMin), new Vector3(this.Bounds.xMin, 0f, this.Bounds.yMax), Color.blue);
		Debug.DrawLine(new Vector3(this.Bounds.xMax, 0f, this.Bounds.yMin), new Vector3(this.Bounds.xMax, 0f, this.Bounds.yMax), Color.blue);

		if (this.Members != null) {
			Vector3 middlePoint = new Vector3((this.Bounds.xMin + this.Bounds.xMax) * 0.5f, 0f, (this.Bounds.yMin + this.Bounds.yMax) * 0.5f);

			foreach (var member in this.Members) {
				Debug.DrawLine(middlePoint, member.position, Color.grey);
			}
		}

		if (this.Subtrees[0] != null) {
			foreach (var subtree in this.Subtrees) {
				subtree.DebugDraw();
			}
		}
	}

	private void SplitIntoSubtrees()
	{
		float x = this.Bounds.x;
		float y = this.Bounds.y;
		float halfWidth = this.Bounds.width * 0.5f;
		float halfHeight = this.Bounds.height * 0.5f;

		this.Subtrees[0] = new QuadTree(new Rect(x,             y,              halfWidth, halfHeight), this.MemberCountPerCell);
		this.Subtrees[1] = new QuadTree(new Rect(x + halfWidth, y,              halfWidth, halfHeight), this.MemberCountPerCell);
		this.Subtrees[2] = new QuadTree(new Rect(x + halfWidth, y + halfHeight, halfWidth, halfHeight), this.MemberCountPerCell);
		this.Subtrees[3] = new QuadTree(new Rect(x,             y + halfHeight, halfWidth, halfHeight), this.MemberCountPerCell);

		for (int i = 0; i < this.Members.Count; i++) {
			AddToSubtree(this.Members[i]);
		}

		this.Members.Clear();
		this.Members = null;
	}

	private void AddToSubtree(Transform member)
	{
		var subtree = GetSubtreeAtLocation(member.position);
		Assert.IsNotNull(subtree);

		if (subtree != null) {
			subtree.Add(member);
		}
	}

	private QuadTree GetSubtreeAtLocation(Vector3 position)
	{
		foreach (var tree in this.Subtrees) {
			if (tree.ContainsXZ(position)) {
				return tree;
			}
		}

		return null;
	}
}

} // namespace
