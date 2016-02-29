using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    public Transform Target;
    public Vector3 OffsetPosition;
    public Vector3 OffsetRotation;

	// Update is called once per frame
	void Update () {
        if (Target != null)
        {
            this.gameObject.transform.position = Target.position + OffsetPosition;
            this.gameObject.transform.rotation = Quaternion.Euler(OffsetRotation.x, OffsetRotation.y, OffsetRotation.z);
        }

	}
}
