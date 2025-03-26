using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Dar créditos a Mix and Jam

public class PlayerCollisionDetection : MonoBehaviour
{
	[Header("Layers")]
	public LayerMask groundLayer;

	[SerializeField]
	private bool _debugBoxes = false;
	
	private bool onGround;
	private bool onWall;
	private bool onRightWall;
	private bool onLeftWall;
	private int wallSide;

	[Space]

	[Header("Offsets")]
	public Vector2 bottomOffset, rightOffset, leftOffset;

	[Space]
	[Header("Sizes")]
	public Vector2 bottomSize, rightSize, leftSize;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomSize, 0f, groundLayer);
		onWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, rightSize, 0f, groundLayer)
			|| Physics2D.OverlapBox((Vector2)transform.position + leftOffset, leftSize, 0f, groundLayer);

		onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, rightSize, 0f, groundLayer);
		onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, leftSize, 0f, groundLayer);

		wallSide = onRightWall ? -1 : 1;
	}

	void OnDrawGizmos()
	{
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
