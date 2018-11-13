
using UnityEngine;
using UnityEngine.Assertions;

namespace BushidoBurrito
{

public class DirectionMovement : MonoBehaviour
{
	public IDirectionProvider Direction;

	private void Start()
	{
		this.Direction = GetComponent<IDirectionProvider>();
        Assert.IsNotNull(this.Direction);
	}

	private void LateUpdate()
	{
		var movement = this.Direction.GetDirection();
		transform.position = transform.position + movement * Time.deltaTime;
		transform.rotation = Quaternion.LookRotation(movement);
	}
}

} // namespace BushidoBurrito
