using System;
using System.Collections.Generic;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public class ViewModelDirtyTrackerTrackNone : ViewModelDirtyTracker
    {
        public override ViewModelDirtyTrackingTypes DirtyTrackerType => ViewModelDirtyTrackingTypes.TrackNone;
    }
}
