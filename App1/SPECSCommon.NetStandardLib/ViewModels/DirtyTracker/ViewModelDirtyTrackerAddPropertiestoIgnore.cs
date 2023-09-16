using System;
using System.Collections.Generic;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public class ViewModelDirtyTrackerAddPropertiesIgnore : ViewModelDirtyTrackerExplicitPropertyTracking
    {
        public override ViewModelDirtyTrackingTypes DirtyTrackerType => ViewModelDirtyTrackingTypes.AddPropertiesToIgnore;
    }
}
