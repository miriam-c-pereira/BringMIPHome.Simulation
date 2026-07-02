namespace BringMIPHome.Simulation
{
    using System;
    using System.ComponentModel;

    public class StationTelemetry : IStationTelemetry, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        private LocationType location;
        public LocationType Location
        {
            get => this.location;
            set
            {
                if (!Equals(this.location, value))
                {
                    this.location = value;
                    this.Notify(nameof(this.Location));
                }
            }
        }
        private float accumulator;
        public float Accumulator
        {
            get => this.accumulator;
            set
            {
                if (Math.Abs(this.accumulator - value) > float.Epsilon)
                {
                    this.accumulator = value;
                    this.Notify(nameof(this.Accumulator));
                }
            }
        }
        private int consecutiveFailures;
        public int ConsecutiveFailures
        {
            get => this.consecutiveFailures;
            set
            {
                if (this.consecutiveFailures != value)
                {
                    this.consecutiveFailures = value;
                    this.Notify(nameof(this.ConsecutiveFailures));
                }
            }
        }
        
        private int consecutiveRewards;
        public int ConsecutiveRewards
        {
            get => this.consecutiveRewards;
            set
            {
                if (this.consecutiveRewards != value)
                {
                    this.consecutiveRewards = value;
                    this.Notify(nameof(this.ConsecutiveRewards));
                }
            }
        }
        
        private int totalRewards;
        public int TotalRewards
        {
            get => this.totalRewards;
            set
            {
                if (this.totalRewards != value)
                {
                    this.totalRewards = value;
                    this.Notify(nameof(this.TotalRewards));
                }
            }
        }
        
        private int totalFailures;
        public int TotalFailures
        {
            get => this.totalFailures;
            set
            {
                if (this.totalFailures != value)
                {
                    this.totalFailures = value;
                    this.Notify(nameof(this.TotalFailures));
                }
            }
        }

        private int totalUploads;
        public int TotalUploads
        {
            get => this.totalUploads;
            set
            {
                if (this.totalUploads != value)
                {
                    this.totalUploads = value;
                    this.Notify(nameof(this.TotalUploads));
                }
            }
        }

        private void Notify(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}