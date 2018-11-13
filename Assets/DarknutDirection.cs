
using System;
using UnityEngine;

namespace BushidoBurrito
{

public class DarknutDirection : MonoBehaviour, IDirectionProvider
{
	public static readonly Vector3 North = new Vector3(0f, 0f, 1f);
	public static readonly Vector3 South = new Vector3(0f, 0f, -1f);
	public static readonly Vector3 East = new Vector3(1f, 0f, 0f);
	public static readonly Vector3 West = new Vector3(-1f, 0f, 0f);

	public Vector3[] Directions = { North, East, South, West };

	public float ChangeDirectionTimeout = 2f;
	private float ChangeDirectionTimer = 0f;

	private int CurrentDirectionIndex = -1;
	private System.Random RandomNumberProvider = new System.Random();

	public Vector3 CurrentDirection { get { return Directions[CurrentDirectionIndex]; } }

	public Vector3 GetDirection() { return CurrentDirection; }

	private void Start()
	{
		GetNewDirection();
	}

	private void Update()
	{
		ChangeDirectionTimer += Time.deltaTime;

		if (ChangeDirectionTimer > ChangeDirectionTimeout)
		{
			ChangeDirectionTimer = 0f;
			GetNewDirection();
		}
	}

	private Vector3 GetNewDirection()
	{
		if (Directions.Length < 1) { return Vector3.zero; }
		if (Directions.Length == 1) { return Directions[0]; }

		int oldCurrentDirectionIndex = CurrentDirectionIndex;
		CurrentDirectionIndex = RandomNumberProvider.Next(0, Directions.Length-1);
		if (CurrentDirectionIndex == oldCurrentDirectionIndex) {
			CurrentDirectionIndex = Directions.Length-1;
		}

		return CurrentDirection;
	}
}

} // namespace BushidoBurrito
