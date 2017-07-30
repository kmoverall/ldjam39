using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Slideshow : MonoBehaviour, IPointerClickHandler
{
    public class Clickable
    {
        public SlideElement.Type type;
        public int numTimesSelected;
        public Rect rect;
    }

    [System.NonSerialized]
    public List<Clickable> clickables = new List<Clickable>();

    Rect slideRect;

    private void Awake()
    {
        Game.Slideshow = this;
    }

    private void Start()
    {
        slideRect = GetComponent<RectTransform>().rect;
    }

    public int Variety 
    {
        get
        {
            int result = 0;
            List<SlideElement.Type> typesFound = new List<SlideElement.Type>();
            for (int i = 0; i < clickables.Count; i++)
            {
                if (!typesFound.Contains(clickables[i].type))
                {
                    typesFound.Add(clickables[i].type);
                    result++;
                }
            }
            return result;
        }
    }

    public float OverlapRating
    {
        get
        {
            float result = 0;

            //Check Overlaps with each other
            for (int i = 0; i < clickables.Count; i++)
            {
                for (int j = i + 1; j < clickables.Count; j++)
                {
                    if (clickables[i].rect.Overlaps(clickables[j].rect))
                    {
                        float r = Mathf.Min(clickables[i].rect.xMax, clickables[j].rect.xMax, slideRect.xMax);
                        float l = Mathf.Max(clickables[i].rect.xMin, clickables[j].rect.xMin, slideRect.xMin);
                        float t = Mathf.Min(clickables[i].rect.yMax, clickables[j].rect.yMax, slideRect.yMax);
                        float b = Mathf.Max(clickables[i].rect.yMin, clickables[j].rect.yMin, slideRect.yMin);
                        
                        result += (r - l) * (t - b);
                    }
                }
            }

            //Check out-of-frame elements
            for (int i = 0; i < clickables.Count; i++)
            {
                float area = clickables[i].rect.height * clickables[i].rect.width;
                float r = Mathf.Min(clickables[i].rect.xMax, slideRect.xMax);
                float l = Mathf.Max(clickables[i].rect.xMin, slideRect.xMin);
                float t = Mathf.Min(clickables[i].rect.yMax, slideRect.yMax);
                float b = Mathf.Max(clickables[i].rect.yMin, slideRect.yMin);

                result += area - (r - l) * (t - b);
            }
            
            return result;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 clickPoint = (eventData.position - eventData.pressEventCamera.pixelRect.center) / 2 - GetComponent<RectTransform>().anchoredPosition;
        foreach(Clickable cl in clickables)
        {
            if (cl.rect.Contains(clickPoint))
            {
                Game.Speech.StartTalk(cl);
                break;
            }
        }
    }
}
