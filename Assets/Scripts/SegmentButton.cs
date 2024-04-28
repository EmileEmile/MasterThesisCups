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
    SegmentButton selectedSegment;
    public Button thisButton;

    private void Start()
    {
        segments = new List<SegmentButton> (FindObjectsOfType<SegmentButton>());
        groupFormation = FindFirstObjectByType<GroupFormation>();
        thisButton = this.GetComponent<Button>();
    }

    public void SegmentClicked ()
    {
        thisButton.interactable = false;

        if (selectedSegment != null)
        {
            selectedSegment.thisButton.interactable = true;
        }

        NextButton.interactable = true;

        groupFormation.WheelValue = ButtonID;

        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].selectedSegment = this;
        }
    }
}
