using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public abstract class Enity : MonoBehaviour,IAnomalousStateEnter
{
    [SerializeField] public EnityDataSo EnityData;

    public Core core { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody2D rig2D { get; private set; }

    public StateMachine stateMachine { get; private set; }

    

    protected virtual void Awake()
    {
        core = GetComponentInChildren<Core>();

        animator = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public abstract void KnockBackStateEnter(KnockBackStateData anomalousData);

    public abstract void CompulsionStateEnter(CompulsionStateData anomalousData);
}
