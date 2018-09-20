using Cinemachine;
using UnityEngine;


[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraShake : CinemachineExtension {
    [Tooltip("Amplitude of the shake")]
    public float m_Range = 0.3f;
    [HideInInspector] public bool shake = false;

    protected override void PostPipelineStageCallback(
    CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if (!shake) {
            return;
        }
        if (stage == CinemachineCore.Stage.Body) {
            Vector3 shakeAmount = GetOffset();
            state.PositionCorrection += shakeAmount;
        }


    }

    Vector3 GetOffset() {
        // Note: change this to something more interesting!
        return new Vector3(
            Random.Range(-m_Range, m_Range),
            Random.Range(-m_Range, m_Range),
            Random.Range(-m_Range, m_Range));
    }
}

/*public class CameraShake : MonoBehaviour {

    /* [SerializeField] float minShakeValue = 0.1f;
     [SerializeField] float maxShakeValue = 1.0f;

    [HideInInspector] public bool shake = false;
    CinemachineVirtualCamera vcam;

    private void Start() {
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }



    void Update() {

        if (shake) {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.5f;
            //transform.rotation = Quaternion.Euler(0.0f, Random.Range(minShakeValue, maxShakeValue), Random.Range(minShakeValue, maxShakeValue));
        }
        else {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
            //transform.rotation = Quaternion.identity;
        }

    }

}*/
