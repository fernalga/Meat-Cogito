using UnityEngine;

public class AssignmentDebugger : MonoBehaviour
{
    public void PrintQuadrantAssignments()
    {
        foreach (var pair in EmotionDrag.quadrantAssignments)
        {
            string emotions = string.Join(", ", pair.Value);
            Debug.Log($"Quadrant {pair.Key} has: {emotions}");
        }
    }
}