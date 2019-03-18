using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class PropertiesPanelManager : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;

    public Text lengthSliderValue;
    public Slider lengthSlider;

    public Text depthSliderValue;
    public Slider depthSlider;

    public Text heightSliderValue;
    public Slider heightSlider;

    public Dropdown cabinetSelector;
    public Dropdown faceMaterialSelector;

    private PlacementManager manager;
    private List<ICabinet> library = CabinetLibrary.Cabinets;
    private void Awake()
    {
        //Build cabinet list for Cabinet selector droplist
        List<string> cabinetList = new List<string>();
        foreach (ICabinet cabinet in library)
        {
            cabinetList.Add(cabinet.Name.ToString());
        }
        cabinetSelector.ClearOptions();
        cabinetSelector.AddOptions(cabinetList);

    }
    // Start is called before the first frame update
    void Start()
    {
        manager = Object.FindObjectOfType<PlacementManager>();
        leftController.GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(OnLeftButtonTwoPressed);
        PlacementManager.length = (int)lengthSlider.value;
        PlacementManager.depth = (int)depthSlider.value;
        PlacementManager.height = (int)heightSlider.value;
        PlacementManager.faceMat = manager.faceMaterials[faceMaterialSelector.value];
        PlacementManager.cabinetType = cabinetSelector.options[cabinetSelector.value].text;
    }

    void OnLeftButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        GameObject canvas = leftController.transform.GetChild(0).gameObject;
        canvas.SetActive(true);
    }

    public void OnLengthChanged()
    {
        lengthSliderValue.text = lengthSlider.value.ToString("0");
        PlacementManager.length = (int) lengthSlider.value;
    }

    public void OnDepthChanged()
    {
        depthSliderValue.text = depthSlider.value.ToString("0");
        PlacementManager.depth = (int)depthSlider.value;
    }

    public void OnHeightChanged()
    {
        heightSliderValue.text = heightSlider.value.ToString("0");
        PlacementManager.height = (int) heightSlider.value;
    }

    public void OnCabinetTypeChanged()
    {
        Debug.Log(string.Format("Cabinet selection chnaged to {0}", cabinetSelector.options[cabinetSelector.value].text));
        PlacementManager.cabinetType = cabinetSelector.options[cabinetSelector.value].text;
    }

    public void OnFaceMaterialChanged()
    {
        PlacementManager.faceMat = manager.faceMaterials[faceMaterialSelector.value];
    }

}
