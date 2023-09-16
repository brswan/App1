using System;
using System.Collections.Generic;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels.DirtyTracker
{
    public enum ViewModelDirtyTrackingTypes
    {
        TrackNone,
        AddPropertiesToIgnore,  // Users must explicitly ignore properties from dirty state tracking
        AddPropertiesToTrack,     // Users must explicitly add properties to track for dirty state changes
        TrackAll
    }
}
