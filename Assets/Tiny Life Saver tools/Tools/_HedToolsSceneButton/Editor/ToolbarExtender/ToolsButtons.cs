﻿using hedCommon.saveLastSelection;
using hedCommon.sceneWorkflow;
using UnityEditor;

namespace hedCommon.toolbarExtent
{
    [InitializeOnLoad]
    public class ToolsButton
    {
        private static CustomSceneButtons _customSceneButtons = new CustomSceneButtons();
        private static SaveLastSelections _saveLastSelections = new SaveLastSelections();
        private static TimeScaleSlider _timeScaleSlider = new TimeScaleSlider();

        static ToolsButton()
        {
            _timeScaleSlider.Init();
            _customSceneButtons.InitTextures();

            ToolbarExtender.LeftToolbarGUI.Add(_saveLastSelections.DisplaySelectionsButtons);
            ToolbarExtender.LeftToolbarGUI.Add(_customSceneButtons.DisplayScenesButton);
            ToolbarExtender.RightToolbarGUI.Add(_timeScaleSlider.DisplaySlider);
        }
    }
}