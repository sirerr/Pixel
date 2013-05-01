using UnityEngine;
using System.Collections;

public class MoveTest : MonoBehaviour
{
	Color colorStart = Color.white;
    Color colorEnd = Color.white;
	Color colorCur = Color.white;
	float step = 1.0f;
    float duration = 1.0f;
	float mag = 7.5f;

	public GameObject target = null;
    public GameObject cube = null;
    public GameObject main = null;

    private GUITest mainScipt;
	
	float mood;
    bool animed;
    float dist, prevDist;

	void Awake ()
    {
        mainScipt = main.GetComponent<GUITest>();
		mood = 0.0f;
        animed = false;
        prevDist = 1000;
    }
	
    void Update()
	{
		if (step < duration)
		{
			step += Time.deltaTime;
			if (step < duration)
			{
		        float lerp = step / duration;
				colorCur = Color.Lerp(colorStart, colorEnd, lerp);
				cube.renderer.material.SetColor("_Color", colorCur);
			} else {
				colorCur = colorEnd;
				cube.renderer.material.SetColor("_Color", colorCur);
				colorStart = colorEnd;
			}
		}
    }
	
	void FixedUpdate()
	{
        if (cube.animation.isPlaying)
            animed = true;
        else if (animed)
        {
            transform.position += cube.transform.localPosition*2.6f;
            cube.transform.localPosition = Vector3.zero;
            animed = false;
        }
		if (rigidbody.velocity == Vector3.zero && Time.frameCount > 1)
		{
			rigidbody.MoveRotation(Quaternion.identity);
			if (target != null)
			{
                dist = Vector3.Magnitude(target.transform.position - transform.position);
                if (dist < 2.5f || dist >= prevDist)
				{
                    if (mainScipt.getState() == 2)
                    {
                        mainScipt.setState(6);
                    }
                    else if (mainScipt.getState() == 3)
                    {
                        mainScipt.setState(7);
                    }
                    else if (mainScipt.getState() == 4)
                    {
                        mainScipt.setState(5);
                    }
                    else if (mainScipt.getState() == 5)
                    {
                        mainScipt.setState(0);
                    }
                    target = null;
                    prevDist = 1000;
					return;
				}
				Vector3 dir = target.transform.position - transform.position;
				if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
		        	rigidbody.AddForce(Mathf.Sign(dir.x)*Vector3.right * mag, ForceMode.Impulse);
				else
                    rigidbody.AddForce(Mathf.Sign(dir.z) * Vector3.forward * mag, ForceMode.Impulse);
                prevDist = dist;
			}
		}
	}
	
	void OnMouseDown()
    {
        if (!cube.animation.isPlaying && mainScipt.getState() < 2)
		{
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
	        Physics.Raycast(ray, out hit);
			
			Vector3 pos = (hit.point-transform.position);
			
			float eyeSize = 0.1282f*transform.localScale.x;
            if (pos.y > 0.3f * transform.localScale.y)
                modMood(1);
            else if (pos.x > -eyeSize && pos.x < eyeSize && pos.y > -eyeSize && pos.y < eyeSize)
                modMood(-1);
            mainScipt.setState(0);
		}
    }

    public void modMood(int mod)
    {
        mood += mod * 0.25f;
        if (!cube.animation.isPlaying)
        {
            if (mood > 0.9f)
            {
                mood = 1.0f;
                cube.animation.animation.Play("happy");
            }
            else if (mood < -0.9f)
            {
                mood = -1.0f;
                cube.animation.animation.Play("mad");
            }
        }
        if (mood >= 0.0f)
            colorEnd = Color.Lerp(Color.white, Color.green, mood);
        else
            colorEnd = Color.Lerp(Color.white, Color.red, -mood);
        colorStart = colorCur;
        step = 0.0f;
    }
}
