//using System.DirectoryServices.Protocols;

//namespace ClassLibrary;

//public class Mystructure //TODO: T nad triedou
//{
//    private readonly List<object> _list;
//    public int Count { get => _list.Count; }

//    public Mystructure()
//    {
//        _list = new();
//    }

//    public object this[int index]
//    {
//        get
//        {
//            if (index >= 0 && index < _list.Count)
//                return _list[index];
//            else
//                throw new IndexOutOfRangeException("Index out of range exception.");
//        }
//        set
//        {
//            if (index >= 0 && index < _list.Count)
//                _list[index] = value;
//            else
//                throw new IndexOutOfRangeException("Index out of range exception.");
//        }
//    }

//    public T GetItem<T>(int index)
//    {
//        return (T)_list[index];
//    }

//    public Type GetTypeOf(int index)
//    {
//        if (_list[index].GetType() == typeof(CSVData))
//        {
//            return typeof(CSVData);
//        }
//        else
//        {
//            return typeof(SearchResultReferenceCollection);
//        }

//    }

//    public void Add<T>(T item) => _list.Add(item);

//    public void RemoveAt(int index) => _list.RemoveAt(index);

//    public IEnumerable<object> Iter()
//    {
//        for (int i = 0; i < Count; i++)
//        {
//            yield return _list[i];
//        }
//    }

//    public void Clear() => _list.Clear();
//}
