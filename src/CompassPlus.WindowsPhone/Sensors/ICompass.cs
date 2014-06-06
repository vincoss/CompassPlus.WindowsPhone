using System;


namespace CompassPlus.Sensors
{
    public interface ICompass : IDisposable
    {
        void Start();

        void Stop();

        bool IsSupported { get; }

        bool IsDataValid { get; }

        CompassData CurrentValue { get; }

        TimeSpan TimeBetweenUpdates { get; set; }

        event EventHandler<EventArgs> Calibrate;

        event EventHandler<CompassDataChangedEventArgs> CurrentValueChanged;
    }
}
