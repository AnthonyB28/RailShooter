using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
	
	
public float FadeTime = 2.0f;
private float FadeMax;


void Start ()

{

    FadeMax = FadeTime;

}



void Update ()

{

    Color MyColor = guiTexture.color;

    if (FadeTime <= 0.0f)

    {

        MyColor.a = 0.0f; 

        GameObject.Destroy(gameObject);

    }

    else

    {

        float Alpha = Mathf.InverseLerp(0.0f,1.0f,FadeTime/FadeMax);

        MyColor.a = Alpha;
			
    }

    guiTexture.color = MyColor;

    FadeTime -= Time.deltaTime;

}
}
