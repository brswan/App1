using System;
using System.Collections.Generic;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public class ViewModelDirtyTrackerTrackAll : ViewModelDirtyTracker
    {
        public override ViewModelDirtyTrackingTypes DirtyTrackerType => ViewModelDirtyTrackingTypes.TrackAll;

        public override void ViewModelPropertyChanged(string propertyName)
        {
            AddPropertyToDirtyMapIfNotExists(propertyName);
            SetPropertyDirty(propertyName);
        }
    }
}