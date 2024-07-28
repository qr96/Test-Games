using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAIC : MonoBehaviour
{
    public Rigidbody2D rigid;
    public GameObject target;

    private void Update()
    {
        if (target != null)
            ChaseTarget();
    }

    void ChaseTarget()
    {

    }
}
