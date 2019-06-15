using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc
{
    public class Fade : MonoBehaviour
    {
        IFade fade = null;

        float range = 0.0f;

        // --------------------------------------------------
        // for MonoBehaviour
        // --------------------------------------------------

        void Start()
        {
            this.fade = GetComponent<IFade>();
            this.Init();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                this.Fadein(1.0f);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                this.Fadeout(1.0f);
            }
        }

        // --------------------------------------------------
        // public functions
        // --------------------------------------------------

        public void Fadein(float time)
        {
            StopAllCoroutines();
            StartCoroutine(CoFadein(time));
        }

        public void Fadeout(float time)
        {
            StopAllCoroutines();
            StartCoroutine(CoFadeout(time));
        }

        public IEnumerator CoFadein(float time)
        {
            float endTime = Time.timeSinceLevelLoad + time * (1.0f - range);

            var wait = new WaitForEndOfFrame();
            while (Time.timeSinceLevelLoad <= endTime)
            {
                range = 1.0f - ((endTime - Time.timeSinceLevelLoad) / time);
                fade.Range = range;
                yield return wait;
            }
            range = 1.0f;
            fade.Range = range;
        }

        public IEnumerator CoFadeout(float time)
        {
            float endTime = Time.timeSinceLevelLoad + time * range;

            var wait = new WaitForEndOfFrame();
            while (Time.timeSinceLevelLoad <= endTime)
            {
                range = (endTime - Time.timeSinceLevelLoad) / time;
                fade.Range = range;
                yield return wait;
            }
            range = 0.0f;
            fade.Range = range;
        }

        // --------------------------------------------------
        // private functions
        // --------------------------------------------------

        private void Init()
        {
            range = 0.0f;
            fade.Range = range;
        }
    }
}
