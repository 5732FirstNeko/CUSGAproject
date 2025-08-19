using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//半山腰太挤，你总得去山顶看看//
public class CollisionComponent : CoreComponent
{
    private MoveComponent moveComponent;

    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;

    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform EnemyChecker;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float EnemyCheckDistance;

    public bool isGrounded { get => Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround); }

    public bool isTouchWall {get => Physics2D.Raycast(wallChecker.position, Vector2.right * moveComponent.FacingDirection, wallCheckDistance, whatIsGround); }

    public bool IsHaveEnemy { get => Physics2D.Raycast(EnemyChecker.position, Vector2.right * moveComponent.FacingDirection, EnemyCheckDistance, whatIsEnemy); }

    private void Start()
    {
        moveComponent = core.GetCoreComponent<MoveComponent>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position,groundCheckRadius);
        if (EnemyChecker != null)
        {
            Gizmos.DrawLine(EnemyChecker.position, new Vector3(EnemyChecker.position.x + EnemyCheckDistance, EnemyChecker.position.y, EnemyChecker.position.z));
        }
    }
}
