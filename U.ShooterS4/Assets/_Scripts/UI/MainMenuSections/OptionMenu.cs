using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MainMenuSection
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private FloatReference sfxVolume;

    protected override void Start()
    {
        base.Start();
        
        sfxSlider.value = sfxVolume.Value;
        sfxSlider.onValueChanged.AddListener((value) => sfxVolume.Value = value);
    }
}
