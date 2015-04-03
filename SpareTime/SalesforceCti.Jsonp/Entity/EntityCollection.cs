using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spare.Jsonp.Entity
{
    public class EntityCollection : IList<EntityItem>
    {
        private List<EntityItem> _items;

        public EntityCollection()
        {
            _items = new List<EntityItem>();
        }

        public int IndexOf(EntityItem item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, EntityItem item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public EntityItem this[int index]
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

        public EntityItem this[Guid id]
        {
            get {
                foreach (EntityItem item in _items)
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

        public EntityItem this[string extension]
        {
            get { 
                foreach (EntityItem item in _items)
                    if ((!String.IsNullOrEmpty(item.Extension)) && (item.Extension.Equals(extension)))
                    {
                        return item;
                    }
                return null;
            }
        }

        public void Add(EntityItem item)
        {
            if (this[item.Id] == null)
                _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(EntityItem item)
        {
            if (this[item.Id] != null)
                return true;
            return false;
        }

        public void CopyTo(EntityItem[] array, int arrayIndex)
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

        public bool Remove(EntityItem item)
        {
            EntityItem rem = this[item.Id];
            return _items.Remove(rem != null ? rem : item);
        }

        public IEnumerator<EntityItem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
