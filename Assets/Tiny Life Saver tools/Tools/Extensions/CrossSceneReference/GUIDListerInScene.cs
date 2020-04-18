using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hedCommon.crossSceneReference
{
    public class GUIDListerInScene : MonoBehaviour
    {
        public Dictionary<GuidDescription, GuidComponent> GuidPresentInScene = new Dictionary<GuidDescription, GuidComponent>();

        public GuidComponent GetGUIDFromKey(GuidDescription guidDescription)
        {
            GuidPresentInScene.TryGetValue(guidDescription, out GuidComponent value);
            return (value);
        }
    }
}