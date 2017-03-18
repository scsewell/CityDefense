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
            foreach (List<bool> source in GetRelevantInput(false))
            {
                for (int i = 0; i < source.Count; i++)
                {
                    if (source[i])
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
            foreach (List<bool> source in GetRelevantInput(true))
            {
                for (int i = source.Count - 1; i > 0; i--)
                {
                    if (source[i] && !source[i - 1])
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
            foreach (List<bool> source in GetRelevantInput(true))
            {
                for (int i = source.Count - 1; i > 0; i--)
                {
                    if (!source[i] && source[i - 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Returns true if any relevant keys are down this frame.
         */
        public bool VisualIsDown()
        {
            foreach (List<bool> source in GetRelevantInput(false))
            {
                if (source.Count > 0 && source.Last())
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Returns true if a relevant keyboard or joystick key was pressed this frame.
         */
        public bool VisualJustDown()
        {
            foreach (List<bool> source in GetRelevantInput(true))
            {
                //UnityEngine.Debug.Log(source.Count);
                if (source.Count > 1 && source[source.Count - 1] && !source[source.Count - 2])
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
            foreach (List<bool> source in GetRelevantInput(true))
            {
                if (source.Count > 1 && !source[source.Count - 1] && source[source.Count - 2])
                {
                    return true;
                }
            }
            return false;
        }
    }
}