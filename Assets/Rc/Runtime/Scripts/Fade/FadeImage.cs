using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc
{

    public class FadeImage : MonoBehaviour, IFade
    {
        [SerializeField]
        Material material = null;

        [SerializeField]
        Texture texture = null;

        [SerializeField]
        CanvasRenderer canvasRenderer = null;

        [Range(0.0f, 1.0f)]
        public float range = 0.0f;

        public float Range { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            canvasRenderer.SetMaterial(material, texture);
            canvasRenderer.SetAlpha(range);

        }
    }
}

