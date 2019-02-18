using System.Collections.Generic;
/// <summary>
/// Делегат, если данные поменялись
/// </summary>
public delegate void DataChanged(IEnumerable<double> _data);
public interface IDataWriter<T>
{
    bool Start();
    bool Stop();
    int Write(IEnumerable<T> _data);
    event DataChanged onDataChanged;
}
