using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public interface IViewModelDirtyTracker
    {
        ViewModelDirtyTrackingTypes DirtyTrackerType { get; }

        void ViewModelPropertyChanged(string propertyName);

        void RemovePropertyDirtyChangeTracking(params string[] propertyNames);

        void AddPropertyDirtyChangeTracking(params string[] propertyNames);

        void RemovePropertyDirtyChangeTracking<T>(params Expression<Func<T>>[] paths);

        void AddPropertyDirtyChangeTracking<T>(params Expression<Func<T>>[] paths);


        bool IsAnyPropertyDirty();

        bool IsPropertyDirty(string propertyName);

        bool IsPropertyDirty<T>(Expression<Func<T>> path);

        void ClearDirtyProperty(string propertyName);

        void ClearDirtyProperty<T>(Expression<Func<T>> path);

        void ClearAllDirtyProperties();
    }
}