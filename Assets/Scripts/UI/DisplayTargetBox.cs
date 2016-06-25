using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayTargetBox : MonoBehaviour
{
	public float scaleMultiplier = 1.0f;
	private RectTransform rectTransform;
	public GameObject target;
    DisplayTargetAuto displayTargetAuto;
    //Renderer renderChild;

    // Use this for initialization
    void Start ()
	{
		rectTransform = GetComponent<RectTransform>();
        displayTargetAuto = FindObjectOfType<DisplayTargetAuto>().GetComponent<DisplayTargetAuto>();

    }
	
	// Update is called once per frame
	void LateUpdate ()
	{

        target = displayTargetAuto.returnCurrentTarget();

        if(target != null && Vector3.Dot(target.transform.position - Camera.main.transform.position, Camera.main.transform.forward) > 0)
        {

            this.GetComponent<Image>().enabled = true;
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
            Rect worldBounds = GUIRectWithObject(target);

            rectTransform.sizeDelta = new Vector2(worldBounds.width, worldBounds.height) * scaleMultiplier;
        }
        else
        {
            this.GetComponent<Image>().enabled = false;
        }
    }

	public static Rect GUIRectWithObject(GameObject trans)
	{
        var overallBounds = BoundsUtilities.OverallBounds(trans);

        Vector3 cen = overallBounds.Value.center;
        Vector3 ext = overallBounds.Value.extents;

        Vector2[] extentPoints = new Vector2[8]
		{
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
			
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
		};
		
		Vector2 min = extentPoints[0];
		Vector2 max = extentPoints[0];
		
		foreach(Vector2 v in extentPoints)
		{
			min = Vector2.Min(min, v);
			max = Vector2.Max(max, v);
		}
		
		return new Rect(min.x, min.y, max.x-min.x, max.y-min.y);
	}
}
