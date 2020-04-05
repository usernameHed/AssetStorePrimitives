using hedCommon.sceneWorkflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hedCommon.exemple
{
    public class InitializeAfterScenesLoad : AbstractLinker
    {
        public override void InitFromEditor()
        {
            Debug.Log("get called after scene loaded from editor");
        }

        public override void InitFromPlay()
        {
            Debug.Log("get called after scene loaded on play mode");
        }
    }
}