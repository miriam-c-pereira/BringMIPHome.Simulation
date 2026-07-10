namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class SimulationTelemetry : ISimulationTelemetry, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        internal RoverTelemetry rover;
        public IRoverTelemetry Rover => this.rover;

        internal StationTelemetry currentStation;
        public IStationTelemetry CurrentStation => this.currentStation;

        private void Notify(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private SimulationStatus status = SimulationStatus.NotStarted;
        public SimulationStatus Status
        {
            get => this.status;
            set
            {
                this.status = value;
                this.Notify(nameof(this.Status));
            }
        }

        private float timeLeft;
        public float TimeLeft
        {
            get => this.timeLeft;
            set
            {
                if (Math.Abs(this.timeLeft - value) > float.Epsilon)
                {
                    this.timeLeft = value;
                    this.Notify(nameof(this.TimeLeft));
                }
            }
        }

        private DoneReasonType doneReason;

        public DoneReasonType DoneReason
        {
            get => this.doneReason;
            set
            {
                if (this.doneReason != value)
                {
                    this.doneReason = value;
                    this.Notify(nameof(this.DoneReason));
                }
            }
        }

        private IReadOnlyList<ActionType> validActions = Array.Empty<ActionType>();

        public IReadOnlyList<ActionType> ValidActions
        {
            get => this.validActions;
            set
            {
                this.validActions = value ?? Array.Empty<ActionType>();
                this.Notify(nameof(this.ValidActions));
            }
        }


        private ActionType currentAction;


        public ActionType CurrentAction
        {
            get => this.currentAction;
            set
            {
                if (this.currentAction != value)
                {
                    this.currentAction = value;
                    this.Notify(nameof(this.CurrentAction));
                }
            }
        }
    }
}