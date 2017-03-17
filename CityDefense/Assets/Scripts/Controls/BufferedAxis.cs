using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace InputController
{
    /*
     * Stores all the mouse and joystick axes that are relevant to a specific in game command.
     */
    public class BufferedAxis : BufferedSource<float>
    {
        private float m_exponent;

        public BufferedAxis(bool canBeMuted, float exponent, List<ISource<float>> sources) : base(canBeMuted, sources)
        {
            m_exponent = exponent;
        }

        /*
         * Returns the value of the axes over the last gamplay update frame, or the last visual update.
         */
        public float GetValue(bool average)
        {
            float maxValue = 0;
            foreach (KeyValuePair<ISource<float>, List<float>> source in GetRelevantInput(false))
            {
                float value = 0;
                foreach (float axisValue in source.Value)
                {
                    value += GetInputValue(source.Key, axisValue);
                }
                if (average && source.Value.Count > 0)
                {
                    value /= source.Value.Count;
                }
                if (Mathf.Abs(maxValue) < Mathf.Abs(value))
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }

        /*
         * Applies modifications to the input values based on the type of source as required.
         */
        private float GetInputValue(ISource<float> source, float value)
        {
            return (source.GetSourceType() == SourceType.Joystick) ? Mathf.Sign(value) * Mathf.Pow(Mathf.Abs(value), m_exponent) : value;
        }
    }
}