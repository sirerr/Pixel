using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {
    public GameObject startLoc;
    public GameObject bookLoc;
    public GameObject foodLoc;
    public GameObject poopLoc;
	public GameObject cube;
    public Transform book;
    public Transform food;
    public Transform poop;
    public Light room;

    private MoveTest cubeScript;
    private BookMove bookScript;
    private FoodMove foodScript;
	private Animation cubeAnim;

    int state = 0;
    float dayTime = 0;
    float idleTime = 0;
    float[] eatTime = new float[5];
    float lastEat;
    Transform fgo = null;
    Transform pgo = null;
    int age = 0;
    int intel = 0;
    int foodNum;

	void Awake ()
    {
        cubeScript = cube.GetComponent<MoveTest>();
        bookScript = book.GetComponent<BookMove>();
        foodScript = food.GetComponent<FoodMove>();
        foodScript.mainScipt = this;
		cubeAnim = cube.GetComponentInChildren<Animation>();
		foreach (AnimationState state in cubeAnim) {
		    state.speed = 0.5f;
        }
    }

    void Start()
    {
        fgo = (Transform)Instantiate(food, food.transform.position, food.transform.rotation);
        foodNum = 0;
        for (int i = 0; i < eatTime.Length; i++)
            eatTime[i] = float.MaxValue;
        lastEat = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        dayTime += Time.deltaTime;
        if (dayTime >= 100 && dayTime <= 120)
            room.intensity = Mathf.Lerp(0.3f, 0.0f, (dayTime - 100.0f) / 20.0f);
        else if (dayTime >= 220 && dayTime < 240)
            room.intensity = Mathf.Lerp(0.0f, 0.3f, (dayTime - 220.0f) / 20.0f);
        else if (dayTime >= 240)
        {
            room.intensity = 0.3f;
            dayTime -= 240;
            age++;
            Destroy(fgo.gameObject);
            fgo = (Transform)Instantiate(food, food.transform.position, food.transform.rotation);
            foodNum = 0;
        }
        if (state < 2)
        {
            for (int i = 0; i < eatTime.Length; i++)
                if (Time.timeSinceLevelLoad-eatTime[i] >= 60)
                {
                    if (pgo == null)
                    {
                        cubeScript.target =  poopLoc;
                        state = 4;
                        eatTime[i] = float.MaxValue;
                        return;
                    }
                    else
                        cubeScript.modMood(-1);
                }
            idleTime += Time.deltaTime;
            if (state == 0 && idleTime > 5)
            {
                state = 1;
                cubeAnim.animation.Play("idle");
            }
            else if (state == 1 && idleTime > 15)
            {
                state = 0;
                idleTime = 0;
                cubeAnim.animation.Play("idle-move around");
            }
        }
        else if (state == 5 && !cubeAnim.isPlaying)
        {
            cubeScript.target = startLoc;
        }
        if (Time.timeSinceLevelLoad - lastEat > 40)
        {
            cube.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            cubeScript.modMood(-1);
            lastEat += 40;
        }
    }

	void OnGUI () {
		GUI.Box(new Rect(0, Screen.height*0.95f, Screen.width, Screen.height*0.05f), "");
		
		GUIStyle style = new GUIStyle();
		style.fontSize = Screen.height/15;
		style.normal.textColor = Color.red;
        string ts = "Time: ";
        int h = (((int)dayTime+80) / 10) % 24;
        if (h < 10)
            ts += "0";
        ts += h + ":";
        int m = ((int)dayTime) % 10 * 6;
        if (m < 10)
            ts += "0";
        ts += m;
        GUI.Label(new Rect(0, 0, 100, 100), ts, style);
        GUI.Label(new Rect(Screen.width * 0.3f, 0, 100, 100), "Age(days): " + age, style);
		GUI.Label(new Rect(Screen.width*0.65f, 0, 100, 100), "Intelligence: "+intel, style);
		
		if(GUI.Button(new Rect(0, Screen.height*0.95f, Screen.width*0.25f, Screen.height*0.05f), "Feed")) {
            if (!cubeAnim.isPlaying && state < 2 && foodNum < 5)
            {
                cubeScript.target = foodLoc;
                state = 3;
            }
		}

		if(GUI.Button(new Rect(Screen.width*0.25f, Screen.height*0.95f, Screen.width*0.25f, Screen.height*0.05f), "Dance")) {
            idleTime = 0;
			cubeAnim.animation.Play("dance");
            cubeScript.modMood(1);
		}

		if(GUI.Button(new Rect(Screen.width*0.5f, Screen.height*0.95f, Screen.width*0.25f, Screen.height*0.05f), "Clean")) {
			if (pgo != null)
			{
				Destroy(pgo.gameObject);
                pgo = null;
                cubeScript.modMood(1);
			}
		}

		if(GUI.Button(new Rect(Screen.width*0.75f, Screen.height*0.95f, Screen.width*0.25f, Screen.height*0.05f), "Teach")) {
            if (!cubeAnim.isPlaying && state < 2)
            {
                cubeScript.target = bookLoc;
                state = 2;
            }
		}
	}
    
    public int getState()
    {
        return state;
    }

    public void setState(int s)
    {
        if (state == 2)
            intel++;
        else if (state == 3)
        {
            eatTime[foodNum] = Time.timeSinceLevelLoad;
            foodNum++;
            cube.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            lastEat = Time.timeSinceLevelLoad;
        }
        else if (state == 4)
        {
            pgo = (Transform)Instantiate(poop, new Vector3(cube.transform.position.x, 3.7f, cube.transform.position.z), poop.transform.rotation);
            cubeAnim.animation.Play("poop");
            cubeScript.modMood(-1);
        }
        state = s;
        if (state == 0)
            idleTime = 0;
        else if (state == 6)
        {
            bookScript.end = cube.transform.position + new Vector3(0, 0, 2);
            bookScript.move = true;
        }
        else if (state == 7)
        {
            fgo.GetComponent<FoodMove>().end = cube.transform.position + new Vector3(0, 0, 2);
            fgo.GetComponent<FoodMove>().move = true;
        }
    }
}
