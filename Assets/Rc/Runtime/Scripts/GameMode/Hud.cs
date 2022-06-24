using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc.GameMode
{
    public interface IHud
    {
        void AddToCanvas(Transform target);
    }

    public class HudDummy : IHud
    {
        public void AddToCanvas(Transform target) { }
    }
}
