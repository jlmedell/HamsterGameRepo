using UnityEngine;

public class Feeder : MonoBehaviour
{
    public GameObject foodObject; // anything you want to activate

    public void Activate()
    {
        foodObject.SetActive(true);
    }

    public void Deactivate()
    {
        foodObject.SetActive(false);
    }
}
