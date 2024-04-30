using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SegmentButton : MonoBehaviour
{
    public Button NextButton;
    public int ButtonID = -1;

    private List<SegmentButton> segments;
    private GroupFormation groupFormation;
    public Button thisButton;

    
    void Start()
    {
        segments = new List<SegmentButton> (FindObjectsOfType<SegmentButton>());
        groupFormation = FindFirstObjectByType<GroupFormation>();
    }

    public void SegmentClicked ()
    {
        thisButton.interactable = false;
        groupFormation.UnselectSegment();

        NextButton.interactable = true;

        groupFormation.WheelValue = ButtonID;
        groupFormation.selectedSegmentButton = this;
    }
}
