using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers 
{
    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t) {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static void DestroyGameObjects(this IEnumerable<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            Object.Destroy(gameObject);
        }
    }

    public static void DestroyTiles(this IEnumerable<TileScript> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            Object.Destroy(gameObject);
        }
    }

    public static void DelayAction(this MonoBehaviour monoBehaviour, System.Action action, float delay)
    {
        monoBehaviour.StartCoroutine(DelayActionCoroutine(delay, action));
    }

    private static IEnumerator<WaitForSeconds> DelayActionCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    #region Vectors
    
    public static Vector3 Where(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
    }

    public static Vector3 ToTilePosition(this Vector2 v, float distanceBetweenTiles = 1f)
    {
        return new Vector3(v.x * distanceBetweenTiles, -1, v.y * distanceBetweenTiles);
    }

    public static Vector3 ToWallPosition(this Vector2 v, float distanceBetweenTiles = 1f)
    {
        return new Vector3(v.x * distanceBetweenTiles, -.5f, v.y * distanceBetweenTiles);
    }

    public static Vector3 ToPotiBonommPosition(this Vector2Int v, float distanceBetweenTiles = 1f)
    {
        return new Vector3(v.x * distanceBetweenTiles, 0f, v.y * distanceBetweenTiles);
    }

    public static Vector3 ToPotiBonommPosition(this Vector2 v, float distanceBetweenTiles = 1f)
    {
        return new Vector3(v.x * distanceBetweenTiles, 0f, v.y * distanceBetweenTiles);
    }

    #endregion

    #region Strings

    public static IEnumerable<string> GetLines(this string s)
    {
        return s.Split('\n');
    }

    #endregion
}
