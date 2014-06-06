using System;


namespace CompassPlus.Filter
{
    // http://en.wikipedia.org/wiki/Smoothing
    // Windows Phone 8 Unleashed

    public interface IFilterStrategy
    {
        double ApplyFilter(double newInput, double previousOutput);
    }

    public class LowPassFilterStrategy : IFilterStrategy
    {
        public LowPassFilterStrategy()
        {
            this.LowPassFilterCoefficient = 0.1D;
            this.NoiseThreshold = double.MaxValue;
        }

        /// <summary>
        /// This is the smoothing factor used for the first order discrete Low-Pass filter
        /// The cut-off frequency fc = fs * K/(2*PI*(1-K))
        /// Default is 0.1, and with a 50Hz sampling rate, this is gives a 1Hz cut-off.
        /// </summary>
        public double LowPassFilterCoefficient { get; set; }

        /// <summary>
        /// Maximum amplitude of noise from sample to sample. 
        /// This is used to remove the noise selectively 
        /// while allowing fast trending for larger amplitudes.
        /// Default is <c>double.MaxValue</c>, which means no filtering takes place.
        /// </summary>
        public double NoiseThreshold { get; set; }

        /// <summary>
        /// A discrete low-magnitude fast low-pass filter used to remove noise 
        /// from raw sensor values while allowing fast trending on high amplitude changes.
        /// </summary>
        /// <param name="newInput">New input value (latest sample)</param>
        /// <param name="previousOutput">The previous (n-1) output value (filtered, one sampling period ago)</param>
        /// <returns>The new output value</returns>
        public double ApplyFilter(double newInput, double previousOutput)
        {
            double newOutputValue = newInput;
            if (Math.Abs(newInput - previousOutput) <= NoiseThreshold)
            {
                /* A simple low-pass filter. */
                newOutputValue = previousOutput + LowPassFilterCoefficient * (newInput - previousOutput);
            }
            return newOutputValue;
        }
    }
}
