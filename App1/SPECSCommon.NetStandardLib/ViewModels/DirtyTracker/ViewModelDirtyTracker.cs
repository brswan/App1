using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public abstract class ViewModelDirtyTracker : IViewModelDirtyTracker
    {
        private object m_Locker;
        private Dictionary<string, bool> m_DirtyPropertyMap;

        public abstract ViewModelDirtyTrackingTypes DirtyTrackerType { get; }

        public ViewModelDirtyTracker()
        {
            m_Locker = new object();
            m_DirtyPropertyMap = new Dictionary<string, bool>();
        }

        public static IViewModelDirtyTracker GetViewModelDirtyTracker(ViewModelBase viewModelBase, ViewModelDirtyTrackingTypes dirtyTrackingType)
        {
            switch (dirtyTrackingType)
            {
                case ViewModelDirtyTrackingTypes.TrackNone:
                    return new ViewModelDirtyTrackerTrackNone();
                case ViewModelDirtyTrackingTypes.AddPropertiesToIgnore:
                    return CreateNewExplicitRemovePropertyViewModel(viewModelBase);
                case ViewModelDirtyTrackingTypes.AddPropertiesToTrack:
                    return new ViewModelDirtyTrackerAddPropertiesToTrack();
                case ViewModelDirtyTrackingTypes.TrackAll:
                    return new ViewModelDirtyTrackerTrackAll();
            }

            throw new ArgumentException($"DirtyTrackingType: {dirtyTrackingType} does not have a class implemented for it.");
        }

        private static IViewModelDirtyTracker CreateNewExplicitRemovePropertyViewModel(ViewModelBase viewModelBase)
        {
            var explicitRemoveDirtyTracker = new ViewModelDirtyTrackerAddPropertiesIgnore();
            explicitRemoveDirtyTracker.TrackAllPropertiesFromViewModel(viewModelBase);
            return explicitRemoveDirtyTracker;
        }

        public virtual void ViewModelPropertyChanged(string propertyName)
        {

        }

        public virtual void RemovePropertyDirtyChangeTracking(params string[] propertyNames)
        {
            lock (m_Locker)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    string propertyName = propertyNames[i];
                    if (propertyName == null)
                    {
                        continue;
                    }

                    if (m_DirtyPropertyMap.ContainsKey(propertyName))
                    {
                        m_DirtyPropertyMap.Remove(propertyName);
                    }
                }
            }
        }

        public void RemovePropertyDirtyChangeTracking<T>(params Expression<Func<T>>[] paths)
        {
            string[] propertyNames = GetPropertyNamesFromExpression(paths);

            RemovePropertyDirtyChangeTracking(propertyNames);
        }

        public virtual void AddPropertyDirtyChangeTracking(params string[] propertyNames)
        {
            lock (m_Locker)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    string propertyName = propertyNames[i];
                    if (propertyName == null)
                    {
                        continue;
                    }

                    AddPropertyToDirtyMapIfNotExists(propertyName);
                }
            }
        }

        public void AddPropertyDirtyChangeTracking<T>(params Expression<Func<T>>[] paths)
        {
            string[] propertyNames = GetPropertyNamesFromExpression(paths);

            AddPropertyDirtyChangeTracking(propertyNames);
        }

        public bool IsPropertyDirty(string propertyName)
        {
            bool isPropertyDirty = false;
            lock (m_Locker)
            {
                if(m_DirtyPropertyMap.ContainsKey(propertyName))
                {
                    isPropertyDirty = m_DirtyPropertyMap[propertyName];
                }
            }
            return isPropertyDirty;
        }

        public bool IsPropertyDirty<T>(Expression<Func<T>> path)
        {
            string propertyName = ViewModelBase.GetPropertyName(path);
            return IsPropertyDirty(propertyName);
        }

        public bool IsAnyPropertyDirty()
        {
            bool isDirty = false;
            lock (m_Locker)
            {
                isDirty = m_DirtyPropertyMap.Values.Any(dirty => dirty);
            }

            return isDirty;
        }

        public void ClearDirtyProperty(string propertyName)
        {
            lock (m_Locker)
            {
                if (m_DirtyPropertyMap.ContainsKey(propertyName))
                {
                    m_DirtyPropertyMap[propertyName] = false;
                }
            }
        }

        public void ClearDirtyProperty<T>(Expression<Func<T>> path)
        {
            string propertyName = ViewModelBase.GetPropertyName(path);
            ClearDirtyProperty(propertyName);
        }

        public void ClearAllDirtyProperties()
        {
            lock (m_Locker)
            {
                List<string> keys = m_DirtyPropertyMap.Keys.ToList();

                for (int i = 0; i < keys.Count; i++)
                {
                    m_DirtyPropertyMap[keys[i]] = false;
                }
            }
        }

        protected void AddPropertyToDirtyMapIfNotExists(string propertyName)
        {
            lock (m_Locker)
            {
                if (!m_DirtyPropertyMap.ContainsKey(propertyName))
                {
                    m_DirtyPropertyMap.Add(propertyName, false);
                }
            }
        }

        protected void SetPropertyDirty(string propertyName)
        {
            lock (m_Locker)
            {
                m_DirtyPropertyMap[propertyName] = true;
            }
        }

        protected bool IsTrackingPropertyDirtyChanged(string propertyName)
        {
            lock (m_Locker)
            {
                return m_DirtyPropertyMap.ContainsKey(propertyName);
            }
        }

        private static string[] GetPropertyNamesFromExpression<T>(Expression<Func<T>>[] paths)
        {
            string[] propertyNames = new string[paths.Length];

            for (int i = 0; i < propertyNames.Length; i++)
            {
                propertyNames[i] = ViewModelBase.GetPropertyName(paths[i]);
            }

            return propertyNames;
        }
    }
}