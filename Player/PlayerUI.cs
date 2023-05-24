using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _experienceProgress;
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _healthCurrent;
        [SerializeField] private TMP_Text _healthMax;

        public void SetExperience(float percent)
        {
            _experienceProgress.fillAmount = percent;
        }
        
        public void SetLevel(int level)
        {
            _level.text = level.ToString();
        }
        
        public void SetHealth(float current, float max)
        {
            _healthBarFill.fillAmount = current / max;
            _healthCurrent.text = Mathf.Round(current).ToString(CultureInfo.CurrentCulture);
            _healthMax.text = Mathf.Round(max).ToString(CultureInfo.CurrentCulture);
        }
    }
}