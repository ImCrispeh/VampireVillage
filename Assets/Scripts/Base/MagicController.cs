using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {
    public Transform target;

	void Update () {
        if (target != null) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 5 * Time.deltaTime);
        } else {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            BaseController._instance.DealDamage(target.gameObject);
            Destroy(this.gameObject);
        }
    }
}
