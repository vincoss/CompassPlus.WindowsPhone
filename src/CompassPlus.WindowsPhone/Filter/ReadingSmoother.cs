using System;

namespace CompassPlus.Filter
{
    // http://en.wikipedia.org/wiki/Smoothing
    // Windows Phone 8 Unleashed

    public class ReadingSmoother
    {
        /// <summary>
        /// Number of prior samples to keep for averaging.       
        /// The higher this number, the larger the latency will be: 
        /// At 50Hz sampling rate: Latency = 20ms * SamplesCount
        /// </summary>
        readonly int _samplesCount = 25; // averaging and checking stability on 500ms

        private bool _initialized;
        private readonly object _initilizedLock = new object();

        /// <summary>
        /// Circular buffer of filtered samples
        /// </summary>
        private readonly double[] _sampleBuffer;

        /// <summary>
        /// Number of samples for which the accelemeter 
        /// is "stable" (filtered acceleration is within Maximum Stability Tilt 
        /// Delta Angle of average acceleration)
        /// </summary>
        private int _deviceStableCount;

        /// <summary>
        /// Index in circular buffer of samples
        /// </summary>
        private int _sampleIndex;

        private double _sampleSum;

        /// <summary>
        /// Indicates how much maximum variation between two reading 
        /// that causes the reading to be deemed unstable.
        /// </summary>
        public double StabilityDelta { get; set; }

        /// <summary>
        /// Average value. This is a simple arithmetic average over the entire sampleBuffer 
        /// (SamplesCount elements) which contains filtered readings
        /// This is used for the calibration, to get a more steady reading of the value.
        /// </summary>
        private double _averageValue;

        private readonly IFilterStrategy _filterStrategy;

        public ReadingSmoother(IFilterStrategy filterStrategy = null, int samplesCount = 25)
        {
            this._samplesCount = samplesCount;
            _sampleBuffer = new double[samplesCount];
            this._filterStrategy = filterStrategy;
        }

        public double ProcessReading(double rawValue)
        {
            double result = rawValue;

            if (!_initialized)
            {
                lock (_initilizedLock)
                {
                    if (!_initialized)
                    {
                        /* Initialize buffer with first value. */
                        _sampleSum = rawValue * _samplesCount;
                        _averageValue = rawValue;

                        for (int i = 0; i < _samplesCount; i++)
                        {
                            _sampleBuffer[i] = _averageValue;
                        }

                        _initialized = true;
                    }
                }
            }

            double latestValue;
            if (_filterStrategy != null)
            {
                latestValue = result = _filterStrategy.ApplyFilter(rawValue, result);
            }
            else
            {
                latestValue = rawValue;
            }

            /* Increment circular buffer insertion index. */
            if (++_sampleIndex >= _samplesCount)
            {
                /* If at max length then wrap samples back 
                 * to the beginning position in the list. */
                _sampleIndex = 0;
            }

            /* Add new and remove old at sampleIndex. */
            _sampleSum += latestValue;
            _sampleSum -= _sampleBuffer[_sampleIndex];
            _sampleBuffer[_sampleIndex] = latestValue;

            _averageValue = _sampleSum / _samplesCount;

            /* Stability check */
            double deltaAcceleration = _averageValue - latestValue;

            if (Math.Abs(deltaAcceleration) > StabilityDelta)
            {
                /* Unstable */
                _deviceStableCount = 0;
            }
            else
            {
                if (_deviceStableCount < _samplesCount)
                {
                    ++_deviceStableCount;
                }
            }

            if (_filterStrategy == null)
            {
                result = _averageValue;
            }

            return result;
        }
    }
}
