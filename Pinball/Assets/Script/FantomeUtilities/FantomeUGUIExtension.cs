using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FantomePI {
    public static class FantomeUGUIExtension {

        public static Vector3 rgb(this Color c) => new Vector3(c.r, c.g, c.b);

        #region RECT TRANSFORM

        public static void SetWidth(this Graphic self, float width) => SetWidth(self.rectTransform, width);

        public static void SetHeight(this Graphic self, float height) => SetHeight(self.rectTransform, height);

        public static void SetWidthAndHeight(this Graphic self, float size) => SetWidthAndHeight(self.rectTransform, size);

        public static void SetWidthAndHeight(this Graphic self, float width, float height) => SetWidthAndHeight(self.rectTransform, width, height);

        public static void SetWidth(this RectTransform self, float width) => SetWidthAndHeight(self, width, self.sizeDelta.y);

        public static void SetHeight(this RectTransform self, float height) => SetWidthAndHeight(self, self.sizeDelta.x, height);

        public static void SetWidthAndHeight(this RectTransform self, float size) => SetWidthAndHeight(self, size, size);

        public static void SetWidthAndHeight(this RectTransform self, float width, float height) {
            self.sizeDelta = new Vector2(width, height);
        }

        #endregion

        public static void SetActive(this Slider slider, bool enable) {
            if (slider == null) return;
            slider.gameObject.SetActive(enable);
        }

        #region GRAPHIC

        public static void SetColorNoAlpha(this Graphic self, Color c) {
            if (self.color.rgb() != c.rgb()) self.color = new Color(c.r, c.g, c.b, self.color.a);
        }

        public static void SetAlpha(this Graphic self, float alpha) {
            if (self == null) return;

            var c = self.color;
            c.a = alpha;
            self.color = c;
        }

        public static void SetActive(this Graphic self, bool enable) {
            if (self == null) return;
            self.gameObject.SetActive(enable);
        }

        public static bool activeSelf(this Graphic self) {
            if (self == null) return false;
            return self.gameObject.activeSelf;
        }

        #endregion

        #region IMAGE

        public static void Set(this Toggle self, bool value, bool noti = true) {
            if (self == null) return;
            self.isOn = value;
        }

        //public static void SetSprite(this Image self, string spriteName) => SetSprite(self, FantomeUGUIUtility.GetSprite(spriteName));

        //public static void SetSprite(this Image self, Sprite sprite) {
        //    if (self == null) return;
        //    self.sprite = sprite;
        //}

        public static void SetFillAmount(this Image img, float value) {
            if (img == null) {
                //FTLog.LogWarning("Image is null!");
                return;
            }

            img.fillAmount = Mathf.Clamp01(value);
        }

        public static void SetFillAmount(this Image img, double val, float capacity) => SetFillAmount(img, (float)val, capacity);

        public static void SetFillAmount(this Image img, float val, float capacity) {
            if (img == null) {
                //FTLog.LogWarning("Image is null");
                return;
            }

            img.fillAmount = Mathf.Clamp01(val / capacity);
        }

        #endregion

        #region TEXT

        //public static void SetLocalization(this Text self, string key) => SetText(self, Localization.Get(key));

        //public static void SetLocalizationFormat(this Text self, string key, params object[] args) => SetFormat(self, Localization.Get(key), args);

        public static void SetText(this Text self, object v) {
            if (self == null) {
                Debug.LogWarning("Label is null!");
                return;
            }
            if (v == null) {
                Debug.LogWarning("Argument is null!");
                return;
            }

            if (v is string) self.text = v as string;
            //else if (v is Fantome.BigInteger) self.text = Fantome.UnitParser.GetUnitValue(v as Fantome.BigInteger);
            else self.text = v.ToString();
        }

        public static void SetFormat(this Text self, string format, params object[] args) {
            if (self == null) {
                Debug.LogWarning("Label is null!");
                return;
            }

            try {
                self.SetText(string.Format(format, args));
            } catch (System.Exception) {
                //FTLog.LogWarningFormat(string.Format("string.Format(1): {0}", format));
                self.SetText(format);
            }
        }

        #endregion

        public static void SetTarget(this ScrollRect scroll, RectTransform target) {
            Canvas.ForceUpdateCanvases();

            scroll.content.anchoredPosition = (Vector2)scroll.transform.InverseTransformPoint(scroll.content.position)
            - (Vector2)scroll.transform.InverseTransformPoint(target.position);
        }

        public static void SetTargetVertical(this ScrollRect scroll, RectTransform target) {
            Canvas.ForceUpdateCanvases();

            var pos = (Vector2)scroll.transform.InverseTransformPoint(scroll.content.position)
                - (Vector2)scroll.transform.InverseTransformPoint(target.position);

            pos.x = scroll.content.anchoredPosition.x;
            scroll.content.anchoredPosition = pos;
        }

        public static void SetTargetHorizontal(this ScrollRect scroll, RectTransform target) {
            Canvas.ForceUpdateCanvases();

            var pos = (Vector2)scroll.transform.InverseTransformPoint(scroll.content.position)
                - (Vector2)scroll.transform.InverseTransformPoint(target.position);

            pos.y = scroll.content.anchoredPosition.y;
            scroll.content.anchoredPosition = pos;
        }

        public static void SetPositionAndRotation(this Transform t, Transform target)
        {
            t.SetPositionAndRotation(target.position, target.rotation);
        }
    }
}
