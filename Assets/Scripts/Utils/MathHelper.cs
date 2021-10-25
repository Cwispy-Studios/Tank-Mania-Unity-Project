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
  }
}
