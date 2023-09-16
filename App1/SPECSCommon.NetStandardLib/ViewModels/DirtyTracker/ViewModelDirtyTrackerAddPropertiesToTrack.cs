using System;
using System.Collections.Generic;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public class ViewModelDirtyTrackerAddPropertiesToTrack : ViewModelDirtyTrackerExplicitPropertyTracking
    {
        public override ViewModelDirtyTrackingTypes DirtyTrackerType => ViewModelDirtyTrackingTypes.AddPropertiesToTrack;
    }
}
