using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTarget
{
   private NavMapManager _manager;
   private int _id;
   private Vector3 orgPos;
   public NavTarget(int id,Vector3 orgPos,NavMapManager manager)
   {
      this.orgPos = orgPos;
      _id = id;
      this._manager = manager;
   }

   public int GetId()
   {
      return _id;
   }
}
