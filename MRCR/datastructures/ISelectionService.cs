using System.Collections.Generic;

namespace MRCR.datastructures;

public interface ISelectionService{}
public interface ISelectionService<T> : ISelectionService
{
    void Set(List<T> items);
    List<T> Get();
    void Add(T item);
    void Add(List<T> items);
    void Remove(T item);
    void Remove(List<T> items);
}