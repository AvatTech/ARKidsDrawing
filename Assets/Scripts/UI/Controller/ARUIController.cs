using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VideoRecorder.Services;
using VoxelBusters.ReplayKit;
using Vuforia;
using Zenject;

namespace UI.Controller
{
    public class ARUIController : MonoBehaviour
    {
        [SerializeField] private PlaneFinderBehaviour planeFinderBehaviour;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject waitingPanel;
        [SerializeField] private GameObject helpPanel;

        [Inject] private ReviewManager _reviewManager;

        private void Start()
        {
            // planeFinderBehaviour.OnAutomaticHitTest.AddListener(HandleAutomaticHitTest);

            // waitingPanel.SetActive(true);

            _reviewManager.EnableARSessionCheck();
        }


        // private bool GroundPlaneHitReceived = false;
        // private long mAutomaticHitTestFrameCount;

        private void LateUpdate()
        {
            // GroundPlaneHitReceived = (mAutomaticHitTestFrameCount == Time.frameCount);

            // var targetStatus = VuforiaBehaviour.Instance.DevicePoseBehaviour.TargetStatus;
            // var isVisible = IsTrackedOrLimited(targetStatus) && GroundPlaneHitReceived;

            // waitingPanel.SetActive(!isVisible);
        }


        // private static bool IsTrackedOrLimited(TargetStatus targetStatus)
        // {
        //     return (targetStatus.Status == Status.TRACKED ||
        //             targetStatus.Status == Status.EXTENDED_TRACKED) &&
        //            targetStatus.StatusInfo == StatusInfo.NORMAL ||
        //            targetStatus.Status == Status.LIMITED && targetStatus.StatusInfo == StatusInfo.UNKNOWN;
        // }


        /// <summary>
        /// Check whether the plane is ready or not?
        /// </summary>
        /// <param name="result"></param>
        // void HandleAutomaticHitTest(HitTestResult result)
        // {
            // mAutomaticHitTestFrameCount = Time.frameCount;

            // // surface is ready!
            // if (result != null)
            //     waitingObject.SetActive(false);
            // else // surface is not ready!
            //     waitingObject.SetActive(true);
        // }

        public void OnExitButton()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnHelpButton()
        {
            if (ReplayKitManager.IsRecording())
                ReplayKitManager.StopRecording();
            else
                RecordingService.StartRecording();


            //helpPanel.SetActive(true);
        }
    }
}