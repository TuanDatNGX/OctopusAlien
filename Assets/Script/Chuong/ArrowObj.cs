using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowObj : MonoBehaviour
{
    public TextMeshProUGUI infoTxt;
    public Transform arrow;
    public AICharacterController target;
    float w;
    float h;

    private void Start()
    {
        w = Screen.width - 10;
        h = Screen.height - 10;
    }

    private void Update()
    {
        if (target == null) return;
        else
        {
            if(!target.gameObject.activeSelf) gameObject.SetActive(false);
            else
            {
                infoTxt.text = target.botName + " Lvl " + target.currentLevel.ToString();

                Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0);
                if(Mathf.Abs(screenPos.x) / Mathf.Abs(screenPos.y) > w/ h)
                {
                    transform.localPosition = new Vector3(w / 2 * (screenPos.x > 0?1:-1), w / 2 * Mathf.Abs(screenPos.y) / Mathf.Abs(screenPos.x) * (screenPos.y > 0 ? 1 : -1));
                }
                else
                {
                    transform.localPosition = new Vector3(h / 2 * Mathf.Abs(screenPos.x) / Mathf.Abs(screenPos.y) * (screenPos.x > 0 ? 1 : -1), h / 2 * (screenPos.y > 0 ? 1 : -1));
                }
                // Rotate the arrow to face the target (optional, to point towards the target)
                Vector3 direction = screenPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.rotation = Quaternion.Euler(0, 0, angle);

                if(IsObjectInView(Camera.main, target.gameObject))
                {
                    arrow.gameObject.SetActive(false);
                    infoTxt.gameObject.SetActive(false);
                }
                else
                {
                    arrow.gameObject.SetActive(true);
                    infoTxt.gameObject.SetActive(true);
                }
            }
        }
    }

    bool IsObjectInView(Camera camera, GameObject obj)
    {
        Vector3 viewportPos = camera.WorldToViewportPoint(obj.transform.position);

        // Check if the object is within the camera's viewport
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0;
    }
}
