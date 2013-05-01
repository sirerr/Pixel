using UnityEngine;
using System.Collections;

public class BookMove : MonoBehaviour {
    public GameObject main = null;
    private GUITest mainScipt;

	public bool move;
	public Vector3 end;
	
    Vector3 start;
    Quaternion startRot;
	float step = 0.0f;
    float duration = 1.0f;
    bool inPlace;

    void Awake()
    {
        mainScipt = main.GetComponent<GUITest>();
    }

    void Start()
    {
		move = false;
        inPlace = false;
		start = end = transform.position;
		startRot = transform.rotation;
	}
	
	void FixedUpdate()
	{
		if (move)
		{
			step += Time.deltaTime;
			if (step < duration)
			{
				float lerp = step / duration;
				transform.position = Vector3.Lerp(start, end, lerp);
				transform.rotation = Quaternion.Lerp(startRot, Quaternion.identity, lerp);
			}
			else if (!inPlace)
			{
				transform.position = end;
				transform.rotation = Quaternion.identity;
                inPlace = true;
            }
            else if (step > 2*duration)
            {
                if (step < 3*duration)
			    {
                    float lerp = (step-2*duration) / duration;
                    transform.position = Vector3.Lerp(end, start, lerp);
                    transform.rotation = Quaternion.Lerp(Quaternion.identity, startRot, lerp);
			    }
                else
                {
                    transform.position = start;
                    transform.rotation = startRot;
                    inPlace = false;
                    move = false;
                    step = 0.0f;
                    mainScipt.setState(5);
                }
			}
		}
	}
}
