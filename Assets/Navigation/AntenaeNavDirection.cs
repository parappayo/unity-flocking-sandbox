
using UnityEngine;

namespace BushidoBurrito
{

public class AntenaeNavDirection : MonoBehaviour, IDirectionProvider
{
	public bool DebugDraw = false;

	public float AntenaeAngle = 30f;
	public float AntenaeLength = 2f;
	public Vector3 AntenaePositionOffset = Vector3.zero;

	public float AvoidanceDuration = 5f;
	private float AvoidanceTimer;

	private Vector3 AvoidanceDirection = Vector3.zero;

	public Vector3 GetDirection()
	{
		return AvoidanceDirection;
	}

	private void Start()
	{
		this.AvoidanceDirection = this.transform.forward;
	}

	private bool Probe(float angle, out RaycastHit hit)
	{
		var direction = Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;

		bool result = Physics.Raycast(
			this.transform.position + this.AntenaePositionOffset,
			direction,
			out hit,
			this.AntenaeLength);

		if (DebugDraw) {
			Debug.DrawRay(this.transform.position, direction, result ? Color.red : Color.green);
		}

		return result;
	}

	private void Update()
	{
		this.AvoidanceTimer -= Time.deltaTime;

		RaycastHit hitL, hitR;
		bool gotHitL = Probe(-this.AntenaeAngle, out hitL);
		bool gotHitR = Probe(this.AntenaeAngle, out hitR);

		if (gotHitL && gotHitR) {
			var distanceL = (hitL.point - this.transform.position).sqrMagnitude;
			var distanceR = (hitR.point - this.transform.position).sqrMagnitude;
			gotHitL = (distanceL > distanceR);
		}

		if (gotHitL) {
			this.AvoidanceTimer = this.AvoidanceDuration;
			this.AvoidanceDirection = Quaternion.AngleAxis(60f, Vector3.up) * this.transform.forward;

		} else if (gotHitR) {
			this.AvoidanceTimer = this.AvoidanceDuration;
			this.AvoidanceDirection = Quaternion.AngleAxis(-60f, Vector3.up) * this.transform.forward;

		} else if (this.AvoidanceTimer <= 0f) {
			this.AvoidanceDirection = this.transform.forward;
		}
	}
}

} // namespace BushidoBurrito
