using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerpointEditor : MonoBehaviour 
{

    [SerializeField]
    SlideElement[] slidePrefabs;

    [SerializeField]
    int numChoices;

    [SerializeField]
    RectTransform spawnZone;

    [SerializeField]
    string[] titles;
    
    [SerializeField]
    Camera slideRenderer;
    [SerializeField]
    Slideshow slideshow;
    [SerializeField]
    RectTransform editedSlide;

    private void Awake()
    {
        Game.Powerpoint = this;
    }

    void Start()
    {
        SpawnChoices();
    }

    public void SpawnChoices()
    {
        SlideElement[] toDestroy = GetComponentsInChildren<SlideElement>();
        for (int i = 0; i < toDestroy.Length; i++)
        {
            Destroy(toDestroy[i].gameObject);
        }

        for (int i = 0; i < numChoices; i++)
        {
            SlideElement newElement = Instantiate(slidePrefabs[Random.Range(0, slidePrefabs.Length)], transform, false);
            Vector3 newPos = Vector3.zero;
            newPos.x = Random.Range(spawnZone.rect.xMin, spawnZone.rect.xMax);
            newPos.y = Random.Range(spawnZone.rect.yMin, spawnZone.rect.yMax);

            if (newElement.type == SlideElement.Type.Title)
            {
                newElement.GetComponentInChildren<Text>().text = titles[Random.Range(0, titles.Length)];
            }

            newElement.transform.localPosition = new Vector3(spawnZone.anchoredPosition.x, spawnZone.anchoredPosition.y, 0) + newPos;
        }
    }

    public void NextSlide()
    {
        slideshow.clickables.Clear();
        Rect slideRect = editedSlide.rect;

        SlideElement[] elements = GetComponentsInChildren<SlideElement>();
        foreach (SlideElement ele in elements)
        {
            Slideshow.Clickable click = new Slideshow.Clickable();
            click.type = ele.type;
            click.numTimesSelected = 0;
            click.rect = ele.GetComponent<RectTransform>().rect;
            click.rect.center = ele.GetComponent<RectTransform>().anchoredPosition - editedSlide.GetComponent<RectTransform>().anchoredPosition;

            if (click.rect.Overlaps(slideRect))
            {
                slideshow.clickables.Add(click);
            }
        }
        slideRenderer.Render();
        SpawnChoices();
        Game.Manager.ChangeView(0);
        Game.Manager.PauseBoredom(0.5f);
        Game.Manager.NewSlide();
    }
}
