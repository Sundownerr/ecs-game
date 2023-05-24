using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DebugSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _value;

        public Slider.SliderEvent OnValueChanged => _slider.onValueChanged;

        private void Start()
        {
            _slider.onValueChanged.AddListener(OnChanged);
        }

        private void OnChanged(float arg0)
        {
            _value.text = arg0.ToString(CultureInfo.InvariantCulture);
        }
        
        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}