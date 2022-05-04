namespace Common
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class DictionaryOfLists<TKey, TValue> : IDictionary<TKey, List<TValue>>
    {
        private readonly IDictionary<TKey, List<TValue>> innerDictionary;

        public DictionaryOfLists()
        {
            innerDictionary = new Dictionary<TKey, List<TValue>>();
        }

        #region MiscDirectDelegates
        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            return innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)innerDictionary).GetEnumerator();
        }

        public void Clear()
        {
            innerDictionary.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
        {
            innerDictionary.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly => innerDictionary.IsReadOnly;
        #endregion

        #region Add
        public void Add(KeyValuePair<TKey, List<TValue>> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, List<TValue> value)
        {
            if (innerDictionary.ContainsKey(key))
            {
                innerDictionary[key].AddRange(value);
            }
            else
            {
                innerDictionary.Add(key, value);
            }
        }
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (innerDictionary.ContainsKey(key))
            {
                innerDictionary[key].Add(value);
            }
            else
            {
                innerDictionary.Add(key, new List<TValue>{value});
            }
        }
        #endregion
        
        #region Contains
        public bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out List<TValue> value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }

        public bool Contains(KeyValuePair<TKey, List<TValue>> item)
        {
            return innerDictionary.Contains(item);
        }

        public bool Contains(List<TValue> item)
        {
            return innerDictionary.Values.Contains(item);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item.Key, item.Value);
        }

        public bool Contains(TKey key, TValue value)
        {
            return innerDictionary.ContainsKey(key) && innerDictionary[key].Contains(value);
        }
        #endregion

        #region Remove
        public bool Remove(KeyValuePair<TKey, List<TValue>> item)
        {
            return innerDictionary.Remove(item);
        }

        public bool Remove(TKey key)
        {
            return innerDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key, item.Value);
        }

        public bool Remove(TKey key, TValue value)
        {
            if (innerDictionary.ContainsKey(key))
            {
                var found = innerDictionary[key].Remove(value);
                if (!innerDictionary[key].Any())
                {
                    innerDictionary.Remove(key);
                }

                return found;
            }

            return false;
        }

        public bool RemoveAll(KeyValuePair<TKey, List<TValue>> item)
        {
            return RemoveAll(item.Key, item.Value);
        }

        public bool RemoveAll(TKey key, List<TValue> values)
        {
            var success = true;
            if (innerDictionary.ContainsKey(key))
            {
                foreach (var value in values)
                {
                    success &= innerDictionary[key].Remove(value);
                }
                if (!innerDictionary[key].Any())
                {
                    innerDictionary.Remove(key);
                }

                return success;
            }

            return false;
        }


        #endregion

        #region Count
        public int Count => innerDictionary.Count;
        public int CountKeys => innerDictionary.Keys.Count;
        public int CountFlattenedValue => innerDictionary.Values.Sum(innerList => innerList.Count);
        #endregion

        public ICollection<TKey> Keys => innerDictionary.Keys;
        public ICollection<List<TValue>> Values => innerDictionary.Values;
        public IEnumerable<TValue> FlattenedValues => innerDictionary.Values.SelectMany(innerList => innerList);
        public IEnumerable<KeyValuePair<TKey, TValue>> FlattenedValuesWithKeys
            => innerDictionary.SelectMany(innerListWithKey => innerListWithKey.Value.Select(val => new KeyValuePair<TKey,TValue>(innerListWithKey.Key, val)));

        public List<TValue> this[TKey key]
        {
            get => innerDictionary[key];
            set => innerDictionary[key] = value;
        }

    }
}
