using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropboxItem : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private bool m_useParentPosition = false;

    // how much to "overshoot" when scrolling, relative to the selected item's height
    private static float SCROLL_MARGIN = 0.3f;

    public void OnSelect(BaseEventData eventData)
    {
        ScrollRect sr = gameObject.GetComponentInParent<ScrollRect>();
        RectTransform selected = eventData.selectedObject.GetComponent<RectTransform>();

        float contentHeight = sr.content.rect.height;
        float viewportHeight = sr.viewport.rect.height;

        // what bounds must be visible?
        float centerLine = m_useParentPosition ? selected.transform.parent.localPosition.y : selected.transform.localPosition.y; // selected item's center
        float upperBound = centerLine + (selected.rect.height / 2f); // selected item's upper bound
        float lowerBound = centerLine - (selected.rect.height / 2f); // selected item's lower bound

        // what are the bounds of the currently visible area?
        float lowerVisible = (contentHeight - viewportHeight) * sr.normalizedPosition.y - contentHeight;
        float upperVisible = lowerVisible + viewportHeight;

        // is our item visible right now?
        float desiredLowerBound;
        if (upperBound > upperVisible)
        {
            // need to scroll up to upperBound
            desiredLowerBound = upperBound - viewportHeight + selected.rect.height * SCROLL_MARGIN;
        }
        else if (lowerBound < lowerVisible)
        {
            // need to scroll down to lowerBound
            desiredLowerBound = lowerBound - selected.rect.height * SCROLL_MARGIN;
        }
        else
        {
            // item already visible - all good
            return;
        }

        // normalize and set the desired viewport
        float normalizedDesired = (desiredLowerBound + contentHeight) / (contentHeight - viewportHeight);
        sr.normalizedPosition = new Vector2(0, Mathf.Clamp01(normalizedDesired));
    }
}
