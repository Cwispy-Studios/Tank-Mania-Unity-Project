using UnityEngine;

namespace CwispyStudios.TankMania
{
  public static class MathHelper
  {
    /// <summary>
    /// Converts a given angle into a signed angle ranging from -180 to 180f
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float ConvertToSignedAngle( float angle )
    {
      float signedAngle = angle;

      // Mod down to within 360
      if (signedAngle > 360f) signedAngle %= 360f;
      else if (signedAngle < -360f) signedAngle %= -360f;

      // Normalise within -180 to 180
      if (signedAngle > 180f) signedAngle -= 360f;
      else if (signedAngle < -180f) signedAngle += 360f;

      return signedAngle;
    }

    /// <summary>
    /// Rotates a point about a pivot point by inputted angles in degrees.
    /// </summary>
    /// <param name="point">
    /// The point to be rotated.
    /// </param>
    /// <param name="pivot">
    /// Where the point is to be rotated around of.
    /// </param>
    /// <param name="angles">
    /// The amouunt to be rotated.
    /// </param>
    /// <returns>
    /// The point after being rotated.
    /// </returns>
    public static Vector3 RotatePointAroundPivot( Vector3 point, Vector3 pivot, Vector3 angles )
    {
      // https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html

      Vector3 dir = point - pivot;
      dir = Quaternion.Euler(angles) * dir;
      point = pivot + dir;

      return point;
    }
  }
}
