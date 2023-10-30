using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Core.TelegramBot
{
    /// <summary>
    /// менеджер сущностей
    /// </summary>
    public class DataManager
    {
        private Dictionary<Type, Data> entityList;

        private class Data
        {
            public object Entity;
            public bool IsDispose;
        }

        public DataManager(params object[] instances)
        {
            foreach (var instance in instances)
            {
                AddData(instance);
            }
        }

        /// <summary>
        /// Добавить сущность
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void AddData<T>(T instance, bool isDispose = false)
        {
            Data newData = new Data() { Entity = instance, IsDispose = isDispose };
            if (entityList != null)
            {
                foreach (var entity in entityList)
                {
                    if (entity.Key is T || entity.Key.IsAssignableFrom(typeof(T)))
                    {
                        try
                        {
                            if (((IDisposable)entity.Value.Entity) != null && isDispose)
                            {
                                ((IDisposable)entity.Value.Entity).Dispose();
                                entityList.Remove(entity.Key);
                                entityList.Add(typeof(T), newData);
                            }
                            else
                            {
                                entityList[typeof(T)] = newData;
                            }
                        }
                        catch (InvalidCastException)
                        {
                            entityList[typeof(T)] = newData;
                        }
                        return;
                    }
                }
                entityList.Add(typeof(T), newData);
            }
            else
            {
                entityList = new Dictionary<Type, Data>
                {
                    { instance.GetType(), newData }
                };
            }
        }

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveData<T>(T instance)
        {
            foreach (var entity in entityList)
            {
                if (entity.Key is T || entity.Key.IsAssignableFrom(typeof(T)))
                {
                    try
                    {
                        if (((IDisposable)entity.Value.Entity) != null && entity.Value.IsDispose)
                        {
                            ((IDisposable)entity.Value.Entity).Dispose();
                        }
                    }
                    catch (InvalidCastException) { }
                    entityList.Remove(entity.Key);
                    return;
                }
            }
        }

        /// <summary>
        /// Очистка
        /// </summary>
        public void Dispose()
        {
            var keysToRemove = entityList.Where(kv => kv.Value.IsDispose).Select(kv => kv.Key).ToList();

            foreach (var key in keysToRemove)
            {
                try
                {
                    if (entityList.TryGetValue(key, out var entity) && entity.Entity is IDisposable disposableEntity)
                    {
                        disposableEntity.Dispose();
                    }
                }
                catch (InvalidCastException) { }

                entityList.Remove(key);
            }
        }

        /// <summary>
        /// Получить сущность
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public T GetData<T>()
        {
            foreach (var instance in entityList)
            {
                if (instance.Key is T || instance.Key.IsAssignableFrom(typeof(T)))
                {
                    return (T)instance.Value.Entity;
                }
            }
            throw new KeyNotFoundException($"Не найдена сущность с типом {typeof(T)} в {nameof(DataManager)}.");
        }
    }
}
