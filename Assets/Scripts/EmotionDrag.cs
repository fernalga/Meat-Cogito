using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class EmotionDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EmotionType emotionType;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera cam;
    private Vector3 offset;

    public static Dictionary<QuadrantType, List<EmotionType>> quadrantAssignments = new();
    // Stores where each image was last dropped
    public static Dictionary<GameObject, QuadrantType> facePositions = new();


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cam = canvas.renderMode == RenderMode.WorldSpace ? canvas.worldCamera : Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, cam, out var worldMouse))
        {
            offset = rectTransform.position - worldMouse;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, cam, out var worldMouse))
        {
            rectTransform.position = worldMouse + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var hit in results)
        {
            if (hit.gameObject.TryGetComponent(out EmotionDropZone zone))
            {
                // Snap to zone's center (or change to offset if needed)
                //rectTransform.position = zone.transform.position;

                // Update assignment
                if (!quadrantAssignments.ContainsKey(zone.quadrantType))
                {
                    quadrantAssignments[zone.quadrantType] = new List<EmotionType>();
                }
                
                // Check if this face was already placed in a different quadrant
                if (facePositions.TryGetValue(gameObject, out QuadrantType previousZone))
                {
                    if (quadrantAssignments.ContainsKey(previousZone))
                    {
                        quadrantAssignments[previousZone].Remove(emotionType);
                    }
                }

                quadrantAssignments[zone.quadrantType].Add(emotionType);
                
                // Track the new assignment
                facePositions[gameObject] = zone.quadrantType;

                Debug.Log($"Dropped {emotionType} on {zone.quadrantType}");
                return; // Stop after first valid hit
            }
        }
    }
}