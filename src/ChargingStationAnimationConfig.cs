namespace BringMIPHome.Simulation
{
    public class ChargingStationAnimationConfig
    {
        /// <summary>
        /// Time interval (seconds) between updates of each battery unit during looping animations.
        /// Used to visually indicate progress of upload/extraction actions.
        /// </summary>
        public float BatteryUnitAnimationStepTime { get; set; }

        /// <summary>
        /// Total duration of the extraction action animation.
        /// Controls how long extraction effects and sounds are played.
        /// </summary>
        public float ExtractionDuration { get; set; }

        /// <summary>
        /// Total duration of the upload action animation.
        /// Controls the overall time window for upload visual/audio effects,
        /// including battery unit progression and reward sound playback.
        /// </summary>
        public float UploadDuration { get; set; }
    }
}