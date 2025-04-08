using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Dar créditos a Mix and Jam

public class PlayerCollisionDetection : MonoBehaviour
{


	[SerializeField]
	private bool _debugBoxes = false;

	[Header("Layers")]
	[Tooltip("Layers that will be tracked in case there is an overlaping with one of the three boxes.")]
	public LayerMask _detectionLayers;

	
	private bool onGround;
	private bool onWall;
	private bool onRightWall;
	private bool onLeftWall;


	[Header("Sizes")]
	[Space]
	public Vector2 bottomSize;
	public Vector2 rightSize;
	public Vector2 leftSize;

	[Header("Offsets")]
	[Space]
	public Vector2 bottomOffset;
	public Vector2 rightOffset;
	public Vector2 leftOffset;



	// Update is called once per frame
	void Update()
	{
		onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomSize, 0f, _detectionLayers);
		onWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, rightSize, 0f, _detectionLayers)
			|| Physics2D.OverlapBox((Vector2)transform.position + leftOffset, leftSize, 0f, _detectionLayers);

		onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, rightSize, 0f, _detectionLayers);
		onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, leftSize, 0f, _detectionLayers);
	}

	void OnDrawGizmos()
	{
		if (!_debugBoxes) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, bottomSize);
		Gizmos.DrawWireCube((Vector2)transform.position + rightOffset, rightSize);
		Gizmos.DrawWireCube((Vector2)transform.position + leftOffset, leftSize);
	}


	public bool GetPlayerOnGround() => onGround;
	public bool GetPlayerOnWall() => onWall;
	public bool GetPlayerOnRightWall() => onRightWall;
	public bool GetPlayerOnLeftWall() => onLeftWall;
}
