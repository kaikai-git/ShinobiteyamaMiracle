using UnityEngine;

//懐中電灯の処理を行うクラス。
public class FlashLightBehaviour : MonoBehaviour
{
    [SerializeField] Light flashLight;
    [SerializeField] Transform lightTransform;

    [SerializeField] Transform camTransform;    //カメラのTransform


    private void Update()
    {
        VerticalRotateLight();
    }

    //縦方向のライト回転、横回転は親子で対応
    public void VerticalRotateLight()
    {
        lightTransform.rotation = camTransform.rotation;
    }
   public void SwitchFlashLight(bool _isActice)
   {
        flashLight.enabled = _isActice;
   }
}
