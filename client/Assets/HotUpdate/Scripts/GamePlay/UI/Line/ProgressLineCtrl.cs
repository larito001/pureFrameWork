using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressLineCtrl : PoolBaseGameObject
{
  public  Slider slider;

  public void Progress(float progress)
  {
    slider.value = progress;
  }

  public override void ResetAll()
  {
    slider.value = 0;
  }
}
