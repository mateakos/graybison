using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spare.Jsonp.Entity
{
    public class EntityCollection : IList<Entity>
    {
        private List<Entity> _items;

        public EntityCollection()
        {
            _items = new List<Entity>();
        }

        public int IndexOf(Entity item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Entity item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public Entity this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public Entity this[Guid id]
        {
            get {
                foreach (Entity item in _items)
                    if (item.Id.Equals(id))
                        return item;
                return null;
            }
            set{
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].Id.Equals(id))
                        _items[i] = value;
                }
            }
        }

        public void Add(Entity item)
        {
            if (this[item.Id] == null)
                _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(Entity item)
        {
            if (this[item.Id] != null)
                return true;
            return false;
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Entity item)
        {
            Entity rem = this[item.Id];
            return _items.Remove(rem != null ? rem : item);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
