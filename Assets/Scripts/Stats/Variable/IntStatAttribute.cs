using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  /// <summary>
  /// Use this attribute to explicitly set a variable as an int. 
  /// This is useful when you want to change an existing asset with the variable to this type 
  /// but it is not using other attributes that can change it.
  /// !!! The asset then has to be selected to update it. 
  /// This attribute can be removed afterwards. !!!
  /// THIS DOES NOT STACK WITH OTHER ATTRIBUTES THAT ARE SPECIFIC TO THE STAT CLASS!
  /// </summary>
  public class IntStatAttribute : PropertyAttribute
  {
  }
}
