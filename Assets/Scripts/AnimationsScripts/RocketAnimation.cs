using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAnimation : MonoBehaviour
{
    public AoeAttack script;
    public float Speed = 0.05f;
    public bool IsMoving = false;
    public Vector2 Target;



    private void Update()
    {

        if (GameController.instance.GamePaused) return;

        if(IsMoving)
        {

            gameObject.transform.position = new Vector2(Mathf.MoveTowards(gameObject.transform.position.x, Target.x, Speed),
           Mathf.MoveTowards(gameObject.transform.position.y, Target.y, Speed));     

        }

        if(Mathf.Approximately(transform.position.x, Target.x) && Mathf.Approximately(transform.position.y, Target.y))
        {
            // reached the target
            script.OnRocketReachedTheTarget(Target);
            Destroy(gameObject);

        }

    }

    public void ShootAtTarget(Vector2 target, AoeAttack script)
    {


        this.script = script;
        Target = target;
        IsMoving = true;

        transform.SetParent(null, true);

        var dir = Target - (Vector2)transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }



}
