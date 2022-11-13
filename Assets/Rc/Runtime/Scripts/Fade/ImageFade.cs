using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rc
{
    public class ImageFade : MonoBehaviour, IFade
    {
        [SerializeField]
        Image image = null;

        [SerializeField]
        CanvasGroup canvasGroup = null;

        public float Range { get => canvasGroup.alpha; set => canvasGroup.alpha = value; }

        public Sprite Sprite { get => image.sprite; set => image.sprite = value; }

        public Color Color { get => image.color; set => image.color = value; }
    }
}
