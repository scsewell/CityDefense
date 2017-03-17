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
        protected List<ISource<T>> m_sources;
        private Dictionary<ISource<T>, List<T>[]> m_buffer;
        private Dictionary<ISource<T>, List<T>> m_relevantBuffer;

        private List<SourceInfo> m_sourceInfos;
        public List<SourceInfo> SourceInfos
        {
            get { return m_sourceInfos; }
        }

        protected bool m_canBeMuted;
        public bool CanBeMuted
        {
            get { return m_canBeMuted; }
        }

        protected BufferedSource(bool canBeMuted, List<ISource<T>> sources)
        {
            m_canBeMuted = canBeMuted;
            if (sources != null)
            {
                m_sources = new List<ISource<T>>(sources);
            }
            else
            {
                m_sources = new List<ISource<T>>();
            }
            m_buffer = new Dictionary<ISource<T>, List<T>[]>();
            m_relevantBuffer = new Dictionary<ISource<T>, List<T>>();
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
            m_relevantBuffer.Clear();
            m_sourceInfos.Clear();
            foreach (ISource<T> source in m_sources)
            {
                List<T>[] fixedFrames = new List<T>[2];
                fixedFrames[0] = new List<T>();
                fixedFrames[1] = new List<T>();
                m_buffer.Add(source, fixedFrames);

                m_relevantBuffer.Add(source, new List<T>());

                m_sourceInfos.Add(new SourceInfo(source.GetSourceType(), source.GetName()));
            }
        }

        /*
         * Run at the end of every input frame to record the input state for that frame.
         */
        public void RecordUpdateState()
        {
            foreach (KeyValuePair<ISource<T>, List<T>[]> source in m_buffer)
            {
                source.Value[1].Add(source.Key.GetValue());
            }
        }

        /*
         * Run at the end of every fixed update to remove old input frames from the buffer.
         */
        public void RecordFixedUpdateState()
        {
            // ensures the last fixed update with input frames is in the buffer along with an empty frame for new inputs
            foreach (KeyValuePair<ISource<T>, List<T>[]> source in m_buffer)
            {
                if (source.Value[1].Count > 0)
                {
                    List<T> temp = source.Value[0];
                    source.Value[0] = source.Value[1];
                    source.Value[1] = temp;
                    source.Value[1].Clear();
                }
            }
        }

        /*
         * Gets all inputs since the last FixedUpdate step.
         */
        protected Dictionary<ISource<T>, List<T>> GetRelevantInput(bool includePrevious)
        {
            // Allow for forcing the last frame with useful input to be included, needed for detecting button state changes
            // Otherwise, if we have not recieved any inputs since the last FixedUpdate, find the last FixedUpdate that does have inputs
            foreach (KeyValuePair<ISource<T>, List<T>> source in m_relevantBuffer)
            {
                source.Value.Clear();
                if ((includePrevious && m_buffer[source.Key][0].Count > 0))
                {
                    source.Value.Add(m_buffer[source.Key][0].Last());
                }
                source.Value.AddRange(m_buffer[source.Key][1]);
            }
            return m_relevantBuffer;
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