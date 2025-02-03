using System.Collections.Generic;
using Godot;

namespace TESTCS.helpers;

public static class ProjectileHelper
{
    public static List<Vector2> GetSpreadPositions(Vector2 origin, Vector2 direction, int count, float spreadDistance)
    {
        List<Vector2> positions = new List<Vector2>();

        // Get perpendicular vector to direction
        Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

        for (int i = 0; i < count; i++)
        {
            float offset = (i - (count - 1) * 0.5f) * spreadDistance;
            Vector2 spreadPosition = origin + (perpendicular * offset);
            positions.Add(spreadPosition);
        }

        return positions;
    }
}