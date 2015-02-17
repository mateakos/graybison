using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace SalesforceCti.Jsonp.Generic
{
    public class BlockingQueue<T> : IEnumerable<JObject>
    {
        private int _count = 0;
        private Queue<JObject> _queue = new Queue<JObject>();

        public JObject Dequeue()
        {
            lock (_queue)
            {
                while (_count <= 0) Monitor.Wait(_queue);
                _count--;
                return _queue.Dequeue();
            }
        }

        public void Enqueue(JObject data)
        {
            if (data == null) throw new ArgumentNullException("data");
            lock (_queue)
            {
                _queue.Enqueue(data);
                _count++;
                Monitor.Pulse(_queue);
            }
        }

        IEnumerator<JObject> IEnumerable<JObject>.GetEnumerator()
        {
            while (true) yield return Dequeue();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            while (true) yield return Dequeue();
        }
    }
}
