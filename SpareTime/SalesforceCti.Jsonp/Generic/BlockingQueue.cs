using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;


namespace Spare.Jsonp.Generic
{
    public class BlockingQueue
    {
        private bool _closing;
        private readonly Queue<Response> _queue = new Queue<Response>();

        public BlockingQueue()
        {
            lock (_queue)
            {
                _closing = false;
                Monitor.PulseAll(_queue);
            }
        }

        public int Count
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count;
                }
            }
        }

        public bool Enqueue(Response data)
        {
            if (data == null) throw new ArgumentNullException("data");
            lock (_queue)
            {
                if (_closing || null == data)
                {
                    return false;
                }

                _queue.Enqueue(data);

                if (_queue.Count > 0)
                    Monitor.Pulse(_queue);

                return true;
            }
        }

        public bool TryDequeue(out Response value, int timeout = Timeout.Infinite)
        {
            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    if (_closing || (timeout < Timeout.Infinite) || !Monitor.Wait(_queue, timeout))
                    {
                        value = default(Response);
                        return false;
                    }
                }

                value = _queue.Dequeue();
                return true;
            }
        }

        public void Close()
        {
            lock (_queue)
            {
                if (!_closing)
                {
                    _closing = true;
                    _queue.Clear();
                    Monitor.PulseAll(_queue);
                }
            }
        }

        public void Clear()
        {
            lock (_queue)
            {
                _queue.Clear();
                Monitor.Pulse(_queue);
            }
        }
    }
}
