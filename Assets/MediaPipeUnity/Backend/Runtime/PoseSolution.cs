using System.Collections;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using UnityEngine;
using UnityEngine.Rendering;
using TextureFramePool = Mediapipe.Unity.Experimental.TextureFramePool;

namespace MediaPipeUnity.Backend
{
    public class PoseSolution : VisionTaskApiRunner<PoseLandmarker>
    {
        [SerializeField] private PoseController poseController;

        public readonly PoseLandmarkDetectionConfig config = new PoseLandmarkDetectionConfig();
        private TextureFramePool _textureFramePool;

        public override void Stop()
        {
            base.Stop();
            _textureFramePool?.Dispose();
            _textureFramePool = null;
        }

        protected override IEnumerator Run()
        {
            Debug.Log($"Delegate = {config.Delegate}");
            Debug.Log($"Model = {config.ModelName}");
            Debug.Log($"Running Mode = {config.RunningMode}");
            Debug.Log($"NumPoses = {config.NumPoses}");
            Debug.Log($"MinPoseDetectionConfidence = {config.MinPoseDetectionConfidence}");
            Debug.Log($"MinPosePresenceConfidence = {config.MinPosePresenceConfidence}");
            Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");
            Debug.Log($"OutputSegmentationMasks = {config.OutputSegmentationMasks}");

            yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

            var options = config.GetPoseLandmarkerOptions(OnPoseLandmarkDetectionOutput);
            taskApi = PoseLandmarker.CreateFromOptions(options, GpuManager.GpuResources);
            var imageSource = ImageSourceProvider.ImageSource;

            yield return imageSource.Play();

            if (!imageSource.isPrepared)
            {
                Debug.LogError("Failed to start ImageSource, exiting...");
                yield break;
            }

            // Use RGBA32 as the input format.
            // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
            _textureFramePool = new TextureFramePool(imageSource.textureWidth, imageSource.textureHeight,
                TextureFormat.RGBA32, 10);

            // NOTE: The screen will be resized later, keeping the aspect ratio.

            if (screen)
                screen.Initialize(imageSource);

            var transformationOptions = imageSource.GetTransformationOptions();
            var flipHorizontally = transformationOptions.flipHorizontally;
            var flipVertically = transformationOptions.flipVertically;

            // Always setting rotationDegrees to 0 to avoid the issue that the detection becomes unstable when the input image is rotated.
            // https://github.com/homuler/MediaPipeUnityPlugin/issues/1196
            var imageProcessingOptions = new Mediapipe.Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: 0);

            AsyncGPUReadbackRequest req = default;
            var waitUntilReqDone = new WaitUntil(() => req.done);
            // var result = PoseLandmarkerResult.Alloc(options.numPoses, options.outputSegmentationMasks);

            // NOTE: we can share the GL context of the render thread with MediaPipe (for now, only on Android)
            var canUseGpuImage = options.baseOptions.delegateCase == Mediapipe.Tasks.Core.BaseOptions.Delegate.GPU &&
                                 SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 &&
                                 GpuManager.GpuResources != null;

            using var glContext = canUseGpuImage ? GpuManager.GetGlContext() : null;

            while (true)
            {
                if (isPaused)
                    yield return new WaitWhile(() => isPaused);

                if (!_textureFramePool.TryGetTextureFrame(out var textureFrame))
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }

                Mediapipe.Image image;
                if (canUseGpuImage)
                {
                    yield return new WaitForEndOfFrame();
                    textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    image = textureFrame.BuildGpuImage(glContext);
                }
                else
                {
                    req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally,
                        flipVertically);
                    yield return waitUntilReqDone;

                    if (req.hasError)
                    {
                        Debug.LogError($"Failed to read texture from the image source, exiting...");
                        break;
                    }

                    image = textureFrame.BuildCPUImage();
                    textureFrame.Release();
                }

                taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
            }
        }

        private void OnPoseLandmarkDetectionOutput(PoseLandmarkerResult result, Mediapipe.Image image,
            long timestamp) =>
            poseController.UpdatePose(result);
    }
}