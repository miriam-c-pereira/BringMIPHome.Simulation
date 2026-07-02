namespace BringMIPHome.Simulation
{
    using System;
    using System.ComponentModel;

    public class RoverTelemetry : IRoverTelemetry, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };


        private bool leftBatteryIsDetached;

        public bool LeftBatteryIsDetached
        {
            get => this.leftBatteryIsDetached;
            set
            {
                if (this.leftBatteryIsDetached != value)
                {
                    this.leftBatteryIsDetached = value;
                    this.Notify(nameof(this.LeftBatteryIsDetached));
                }
            }
        }

        private bool rightBatteryIsDetached;

        public bool RightBatteryIsDetached
        {
            get => this.rightBatteryIsDetached;
            set
            {
                if (this.rightBatteryIsDetached != value)
                {
                    this.rightBatteryIsDetached = value;
                    this.Notify(nameof(this.RightBatteryIsDetached));
                }
            }
        }

        private float leftBatteryEnergy;

        public float LeftBatteryEnergy
        {
            get => this.leftBatteryEnergy;
            set
            {
                if (Math.Abs(this.leftBatteryEnergy - value) > float.Epsilon)
                {
                    this.leftBatteryEnergy = value;
                    this.Notify(nameof(this.LeftBatteryEnergy));
                    this.Notify(nameof(this.TotalBatteryEnergy));
                }
            }
        }

        private float rightBatteryEnergy;

        public float RightBatteryEnergy
        {
            get => this.rightBatteryEnergy;
            set
            {
                if (Math.Abs(this.rightBatteryEnergy - value) > float.Epsilon)
                {
                    this.rightBatteryEnergy = value;
                    this.Notify(nameof(this.RightBatteryEnergy));
                    this.Notify(nameof(this.TotalBatteryEnergy));
                }
            }
        }

        public float TotalBatteryEnergy => this.LeftBatteryEnergy + this.RightBatteryEnergy;

        private SimVector3 position;

        public SimVector3 Position
        {
            get => this.position;
            set
            {
                if (!this.position.Equals(value))
                {
                    this.position = value;
                    this.Notify(nameof(this.Position));
                }
            }
        }

        private float heading;

        public float Heading
        {
            get => this.heading;
            set
            {
                if (Math.Abs(this.heading - value) > float.Epsilon)
                {
                    this.heading = value;
                    this.Notify(nameof(this.Heading));
                }
            }
        }

        private float angularVelocity;

        public float AngularVelocity
        {
            get => this.angularVelocity;
            set
            {
                if (Math.Abs(this.angularVelocity - value) > float.Epsilon)
                {
                    this.angularVelocity = value;
                    this.Notify(nameof(this.AngularVelocity));
                }
            }
        }

        private bool isMoving;

        public bool IsMoving
        {
            get => this.isMoving;
            set
            {
                if (this.isMoving != value)
                {
                    this.isMoving = value;
                    this.Notify(nameof(this.IsMoving));
                }
            }
        }

        private float linearVelocity;

        public float LinearVelocity
        {
            get => this.linearVelocity;
            set
            {
                if (Math.Abs(this.linearVelocity - value) > float.Epsilon)
                {
                    this.linearVelocity = value;
                    this.Notify(nameof(this.LinearVelocity));
                    this.Notify(nameof(this.IsMoving)); // if UI binds to IsMoving
                }
            }
        }

        private LocationType targetLocation;

        public LocationType TargetLocation
        {
            get => this.targetLocation;
            set
            {
                if (this.targetLocation != value)
                {
                    this.targetLocation = value;
                    this.Notify(nameof(this.TargetLocation));
                }
            }
        }

        private float distanceToTarget;

        public float DistanceToTarget
        {
            get => this.distanceToTarget;
            set
            {
                if (Math.Abs(this.distanceToTarget - value) > float.Epsilon)
                {
                    this.distanceToTarget = value;
                    this.Notify(nameof(this.DistanceToTarget));
                }
            }
        }

        private float estimatedTimeToArrival;

        public float EstimatedTimeToArrival
        {
            get => this.estimatedTimeToArrival;
            set
            {
                if (Math.Abs(this.estimatedTimeToArrival - value) > float.Epsilon)
                {
                    this.estimatedTimeToArrival = value;
                    this.Notify(nameof(this.EstimatedTimeToArrival));
                }
            }
        }

        private void Notify(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}