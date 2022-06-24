using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc.GameMode
{
    public interface IPlayer
    {
        void Posses(IPawn pawn);
        void UnPosses();

        void SetHud(IHud hud);
        IHud GetHud();

        void SetCamera(ICamera camera);
        ICamera GetCamera();
    }

    public class PlayerDummy : IPlayer
    {
        IHud m_Hud = new HudDummy();
        ICamera m_Camera = new CameraDummy();

        public ICamera GetCamera() { return m_Camera; }

        public IHud GetHud() { return m_Hud; }

        public void Posses(IPawn pawn) { } 
        public void SetCamera(ICamera camera) { } 

        public void SetHud(IHud hud) { }

        public void UnPosses() { }
    }
}
