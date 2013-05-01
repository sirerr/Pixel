using UnityEngine;
using System.Collections;

public class FoodMove : MonoBehaviour {
    public GUITest mainScipt;

    public bool move;
	public Vector3 end;

    Vector3 start;
    Quaternion startRot;
	float step = 0.0f;
    float duration = 1.0f;
    Transform piece = null;

	void Start () {
		move = false;
		start = end = transform.position;
		startRot = transform.rotation;
	}
	
	void FixedUpdate()
	{
		if (move)
		{
            if (piece == null)
                piece = this.transform.GetChild(0).transform;
            if (step < duration)
            {
                step += Time.deltaTime;
                float lerp = step / duration;
                piece.position = Vector3.Lerp(start, end, lerp);
                piece.rotation = Quaternion.Lerp(startRot, Quaternion.identity, lerp);
            }
            else if (step < 2 * duration)
            {
                step += Time.deltaTime;
                piece.position = end;
                piece.rotation = Quaternion.identity;
            }
            else
            {
                Destroy(piece.gameObject);
                mainScipt.setState(5);
                move = false;
                step = 0.0f;
            }
        }
	}
}
