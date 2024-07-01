using UnityEngine;
using Zenject;
using VoxelBusters.ReplayKit;
using VoxelBusters.ReplayKit.Internal;

namespace VideoRecorder.Services
{
    public static class RecordingService
    {
        private static bool IsAvailable()
        {
            return ReplayKitManager.IsRecordingAPIAvailable();
        }

        public static void StartRecording()
        {
            if (!IsAvailable())
                return;

            ReplayKitManager.StartRecording();


            Debug.Log($"Recording status: {ReplayKitManager.IsRecording()}");
        }

        public static void StopRecording()
        {
            if (!ReplayKitManager.IsRecording())
                return;

            ReplayKitManager.StopRecording();
        }
    }
}