using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Logic
{
    public interface IDatabaseModel<T>
    {
        int GetLastId();
        T Find(Predicate<T> filter);
        List<T> FindAll(Predicate<T> filter);
        void Insert(T item);
        // Deprecated
        // void Delete(Expression<Func<T, bool>> filter);
        void Delete(T item);
        void Update(Expression<Func<T, bool>> filter, T update);
    }
}