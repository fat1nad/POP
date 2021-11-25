// Author: Fatima Nadeem - WonderTree

using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    Slider slider;

    public string catName;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioManager.instance.GetCategoryVolume(catName);
        slider.onValueChanged.AddListener(delegate 
        {
            AudioManager.instance.SetCategoryVolume(catName, slider.value);
        });
    }
}
