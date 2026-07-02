namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;

    public interface ISimulationHost
    {
        /// <summary>
        /// Current simulation telemetry exposed to external systems.
        /// </summary>
        ISimulationTelemetry Telemetry { get; }

        /// <summary>
        /// Raised when the currently executing action has completed.
        /// </summary>
        event Action<ActionEvent> ActionCompleted;

        /// <summary>
        /// Raised when the simulation ends.
        /// This may occur because the mission objective was achieved,
        /// the rover ran out of energy, or the simulation time expired.
        /// </summary>
        event Action<DoneEvent> SimulationCompleted;

        /// <summary>
        /// Advances the simulation by the specified time.
        /// </summary>
        void Tick(float deltaTime);

        /// <summary>
        /// Requests that a new action be started.
        /// Returns false if the action cannot be started in the current simulation state.
        /// </summary>
        bool TryStartAction(ActionType action);

        /// <summary>
        /// Notifies the simulation that the previously requested action
        /// has finished executing.
        /// This is typically called by the rover or station controller.
        /// </summary>
        void NotifyActionCompleted();

        void InitializeControllers(IRoverController rover, IEnumerable<IChargingStationController> chargingStations, IMissionControlController missionControl);
    }
}