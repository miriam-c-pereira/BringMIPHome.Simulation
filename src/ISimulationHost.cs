namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;

    //public abstract class NotifyActionCompleteArgs
    //{
    //    public ActionType Action { get; set; }
    //    public bool IsSuccess { get; set; }
    //}
    
    //public class RoverActionCompleteArgs : NotifyActionCompleteArgs
    //{
    //}

    //public class StationActionCompleteArgs : NotifyActionCompleteArgs
    //{
    //}

    public interface IController
    {
        
    }

    public abstract class SimEventArgs : EventArgs
    {
    }


    public interface ISimulationHost
    {
        /// <summary>
        /// Current simulation telemetry exposed to external systems.
        /// </summary>
        ISimulationTelemetry Telemetry { get; }

        event EventHandler<SimulationEvent> SimulationEvent;

        /// <summary>
        /// Advances the simulation by the specified time.
        /// </summary>
        void Tick(float deltaTime);

        /// <summary>
        /// Requests that a new action be started.
        /// Returns false if the action cannot be started in the current simulation state.
        /// </summary>
        bool TryStartAction(ActionType action);

        ///// <summary>
        ///// Notifies the simulation that the previously requested action
        ///// has finished executing.
        ///// This is typically called by the rover or station controller.
        ///// </summary>
        //void NotifyActionCompleted(NotifyActionCompleteArgs args);

        void InitializeControllers(IRoverController rover, IEnumerable<IChargingStationController> chargingStations, IMissionControlController missionControl);
    }
}