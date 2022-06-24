using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc.GameMode
{
    public interface IPawn
    {
        ICamera GetCamera();
    }

    public class PawnDummy : IPawn
    {
        ICamera m_Camera = new CameraDummy();

        public ICamera GetCamera() { return m_Camera; }
    }
}
