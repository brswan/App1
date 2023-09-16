using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public abstract class ViewModelDirtyTrackerExplicitPropertyTracking : ViewModelDirtyTracker
    {
        public override ViewModelDirtyTrackingTypes DirtyTrackerType => ViewModelDirtyTrackingTypes.AddPropertiesToTrack;

        public override void ViewModelPropertyChanged(string propertyName)
        {
            if (IsTrackingPropertyDirtyChanged(propertyName))
            {
                SetPropertyDirty(propertyName);
            }
        }

        internal void TrackAllPropertiesFromViewModel(ViewModelBase viewModelBase)
        {
            IEnumerable<PropertyInfo> viewModelProperties = viewModelBase.GetType().GetRuntimeProperties();
            foreach (PropertyInfo viewModelProperty in viewModelProperties)
            {
                AddPropertyToDirtyMapIfNotExists(viewModelProperty.Name);
            }
        }
    }
}
