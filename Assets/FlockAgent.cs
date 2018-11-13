
using UnityEngine;

namespace BushidoBurrito
{

public class FlockAgent : MonoBehaviour
{
	public Vector3 BaseMovement = new Vector3(1f, 0f, 0f);

	public float Speed = 1f;
	public float DriftMovement = 0.2f;
	public float FlockingWeight = 1f;
	public float AntenaeWeight = 1f;

	public FlockDirection FlockDirection;
	public AntenaeNavDirection AntenaeNavDirection;

	private void Start()
	{
		this.BaseMovement.x += Random.Range(-this.DriftMovement, this.DriftMovement);
		this.BaseMovement.z += Random.Range(-this.DriftMovement, this.DriftMovement);
	}

	private void LateUpdate()
	{
		var movement = this.BaseMovement;

		if (this.FlockDirection != null) {
			movement += this.FlockDirection.GetDirection() * this.FlockingWeight;
		}

		if (this.AntenaeNavDirection != null) {
			movement += this.AntenaeNavDirection.GetDirection() * this.AntenaeWeight;
		}

		movement = movement.normalized;
		this.transform.position = this.transform.position + movement * this.Speed * Time.deltaTime;
		this.transform.rotation = Quaternion.LookRotation(movement);
	}
}

} // namespace BushidoBurrito
