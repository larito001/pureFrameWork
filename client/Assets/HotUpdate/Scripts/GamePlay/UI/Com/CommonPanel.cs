using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YOTO;

public class CommonPanel : MonoBehaviour
{
    public Button closeBtn;
    private UIPageBase parent;
    private void Awake()
    {
        parent=this.GetComponentInParent<UIPageBase>();
        
        closeBtn.onClick.AddListener(() =>
        {
            if (parent != null)
            {
                YOTOFramework.uIMgr.Hide(parent.uiType); 
            }

        });
    }
}
