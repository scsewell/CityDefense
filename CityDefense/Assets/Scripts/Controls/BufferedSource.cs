using System;
using System.Collections.Generic;
using System.Linq;

namespace InputController
{
    /*
     * Manages input buffering.
     */
    public class BufferedSource<T>
    {
        private static int MAX_BUFFER_SIZE = 20;

        protected List<ISource<T>> m_sources;
        private List<List<T>[]> m_buffer;
        private List<List<T>> m_relevantInput;

        private List<SourceInfo> m_sourceInfos;

        protected bool m_canBeMuted;
        public bool CanBeMuted
        {
            get { return m_canBeMuted; }
        }

        protected BufferedSource(bool canBeMuted, List<ISource<T>> sources)
        {
            m_canBeMuted = canBeMuted;
            if (sources != null && sources.Count > 0)
            {
                m_sources = new List<ISource<T>>(sources);
            }
            else
            {
                m_sources = new List<ISource<T>>();
            }
            m_buffer = new List<List<T>[]>();
            m_relevantInput = new List<List<T>>();
            m_sourceInfos = new List<SourceInfo>();
            ResetBuffers();
        }

        /*
         * Initializes the buffer lists from the current sources.
         */
        public void ResetBuffers()
        {
            m_sources = m_sources.OrderBy(s => (int)s.GetSourceType()).ToList();

            m_buffer.Clear();
            m_relevantInput.Clear();
            m_sourceInfos.Clear();
            foreach (ISource<T> source in m_sources)
            {
                List<T>[] fixedFrames = new List<T>[2];
                fixedFrames[0] = new List<T>(MAX_BUFFER_SIZE);
                fixedFrames[1] = new List<T>(MAX_BUFFER_SIZE);
                m_buffer.Add(fixedFrames);

                m_relevantInput.Add(new List<T>(2 * MAX_BUFFER_SIZE));

                m_sourceInfos.Add(new SourceInfo(source.GetSourceType(), source.GetName()));
            }
        }

        /*
         * Clears out any unprocessed input frames.
         */
        public void ClearBuffers()
        {
            for (int i = 0; i < m_sources.Count; i++)
            {
                m_buffer[i][0].Clear();
                m_buffer[i][1].Clear();
                m_relevantInput[i].Clear();
            }
        }

        /*
         * Run at the end of every input frame to record the input state for that frame.
         */
        public void RecordUpdateState(bool muting)
        {
            if (!(muting && m_canBeMuted))
            {
                for (int i = 0; i < m_sources.Count; i++)
                {
                    m_buffer[i][1].Add(m_sources[i].GetValue());
                    if (m_buffer[i][1].Count > MAX_BUFFER_SIZE)
                    {
                        m_buffer[i][1].RemoveAt(0);
                    }
                }
            }
        }

        /*
         * Run at the end of every fixed update to remove old input frames from the buffer.
         * Ensures the last fixed update with input frames is in the buffer along with an empty frame for new inputs.
         */
        public void RecordFixedUpdateState(bool muting)
        {
            if (!(muting && m_canBeMuted))
            {
                foreach (List<T>[] source in m_buffer)
                {
                    if (source[1].Count > 0)
                    {
                        List<T> temp = source[0];
                        source[0] = source[1];
                        source[1] = temp;
                        source[1].Clear();
                    }
                }
            }
        }

        /*
         * Gets all inputs since the last FixedUpdate step.
         * Allow for adding the last input frame of the previous FixedUpdate to be included, needed for detecting button state changes.
         */
        protected List<List<T>> GetRelevantInput(bool includePrevious)
        {
            for (int i = 0; i < m_sources.Count; i++)
            {
                m_relevantInput[i].Clear();
                if (includePrevious && m_buffer[i][0].Count > 0)
                {
                    m_relevantInput[i].Add(m_buffer[i][0].Last());
                }
                m_relevantInput[i].AddRange(m_buffer[i][1]);
            }
            return m_relevantInput;
        }

        public List<SourceInfo> GetSourceInfo()
        {
            return m_sourceInfos;
        }

        /*
         * Adds a new source and resets the buffer.
         */
        public void AddSource(ISource<T> source)
        {
            if (!Contains(source))
            {
                m_sources.Add(source);
                ResetBuffers();
            }
        }

        /*
         * Removes a source and resets the buffer.
         */
        public void RemoveSource(int index)
        {
            m_sources.RemoveAt(index);
            ResetBuffers();
        }

        /*
         * Checks if a source is already used in the buffer.
         */
        private bool Contains(ISource<T> source)
        {
            return m_sources.Any(s => s.GetName() == source.GetName() && s.GetSourceType() == source.GetSourceType());
        }
    }
}