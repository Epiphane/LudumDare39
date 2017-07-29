using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCurve : MonoBehaviour
{

    public AnimationCurve xc;
    public float xLen;
    public float xMult;
    public float xOffset;
    public AnimationCurve yc;
    public float yLen;
    public float yMult;
    public float yOffset;
    public AnimationCurve zc;
    public float zLen;
    public float zMult;
    public float zOffset;

    private float t;

	// Update is called once per frame
	void Update ()
	{
	    t += Time.deltaTime;
	    float xt = Mathf.Repeat(t + xOffset, xLen) / xLen;
	    float yt = Mathf.Repeat(t + yOffset, yLen) / yLen;
	    float zt = Mathf.Repeat(t + zOffset, zLen) / zLen;

	    float xPos = transform.localPosition.x;
        float yPos = transform.localPosition.y;
        float zPos = transform.localPosition.z;

        if (Math.Abs(xLen) > 0.01)
        {
            xPos = xc.Evaluate(xt) * xMult;
        }

	    if (Math.Abs(yLen) > 0.01)
	    {
	        yPos = yc.Evaluate(yt) * yMult;
	    }

	    if (Math.Abs(zLen) > 0.01)
	    {
	        zPos = zc.Evaluate(zt) * zMult;
	    }

        transform.localPosition = new Vector3(xPos, yPos, zPos);
	}
}
