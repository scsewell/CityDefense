using System.Collections.Generic;
using System.Linq;

namespace InputController
{
    /*
     * Stores all the keyboard and joystick button inputs that are relevant to a specific in game command.
     */
    public class BufferedButton : BufferedSource<bool>
    {
        public BufferedButton(bool canBeMuted, List<ISource<bool>> sources) : base(canBeMuted, sources) {}

        /*
         * Returns true if any relevant keys are down this frame.
         */
        public bool IsDown()
        {
            foreach (KeyValuePair<ISource<bool>, List<bool>> source in GetRelevantInput(true))
            {
                for (int i = source.Value.Count - 1; i > 0; i--)
                {
                    if (source.Value[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Returns true if a relevant keyboard or joystick key was pressed since the last FixedUpdate.
         */
        public bool JustDown()
        {
            foreach (KeyValuePair<ISource<bool>, List<bool>> source in GetRelevantInput(true))
            {
                for (int i = source.Value.Count - 1; i > 0; i--)
                {
                    if (source.Value[i] && !source.Value[i - 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Returns true if a relevant keyboard or joystick key was released since the last FixedUpdate.
         */
        public bool JustUp()
        {
            foreach (KeyValuePair<ISource<bool>, List<bool>> source in GetRelevantInput(true))
            {
                for (int i = source.Value.Count - 1; i > 0; i--)
                {
                    if (!source.Value[i] && source.Value[i - 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Returns true if a relevant keyboard or joystick key was pressed this frame.
         */
        public bool VisualJustDown()
        {
            foreach (KeyValuePair<ISource<bool>, List<bool>> source in GetRelevantInput(true))
            {
                if (source.Value.Count > 1 && source.Value[source.Value.Count - 1] && !source.Value[source.Value.Count - 2])
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Returns true if a relevant keyboard or joystick key was released this frame.
         */
        public bool VisualJustUp()
        {
            foreach (KeyValuePair<ISource<bool>, List<bool>> source in GetRelevantInput(true))
            {
                if (source.Value.Count > 1 && !source.Value[source.Value.Count - 1] && source.Value[source.Value.Count - 2])
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Returns true if any relevant keys are down this frame.
         */
        public bool VisualIsDown()
        {
            foreach (KeyValuePair<ISource<bool>, List<bool>> source in GetRelevantInput(true))
            {
                if (source.Value.Count > 0 && source.Value.Last())
                {
                    return true;
                }
            }
            return false;
        }
    }
}