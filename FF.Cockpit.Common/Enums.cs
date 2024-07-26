using FF.Cockpit.Common;
using System.ComponentModel;

public enum CheckBoxTag
{
    Anonymized,
    ServiceTab
}

public enum SyncStatus
{
    DownloadCompleted = 7,
    DownloadPartially = 9
}

public enum CloudFilterType
{
    All = 1,
    Images = 2,
    Icons = 3,
    Videos = 4
}
public enum TrendsYears
{
    OneYear = 1,
    FiveYear = 2,
    TwentyYear = 3
}
public enum TrendsType
{
    ExaminationTrends = 1,
    PatientTrends = 2,
    MicroImagesTrends = 3,
    SessionDurationTrends=4,
    AvgSessionDurationTrends = 5
}

public enum ModuleNames
{
    Operation = 1,
    Scheduler = 2,
    Dashboard = 3,
    Performance = 4,
    Service = 5,
    Configuration = 6,
    CloudSync = 7,
    Trends = 8
}
public enum UserType
{
    Admin=1,
    Doctor=2
}

public enum DataGridNames
{
    HighRiskPatient = 1,
    CloudSync=2,
}

public enum DataGridConfigName
{
    ColumnWidth = 1,
    ColumnSort = 2,
    ExpandCollapse = 3,
}

public enum SyncTypes
{
    Automatic = 1,
    Manual = 2
}

public enum DurationTypes
{
    ThirtyMinutes = 30,
    SixtyMinutes = 60,
    NinetyMinutes = 90,
    OneTwentyMinutes = 120
}


public enum MiscellaneousKeys
{
    [Description("StartHour")]
    StartHour,
    [Description("EndHour")]
    EndHour,
    [Description("FirstDayOfWeek")]
    FirstDayOfWeek,
    [Description("SyncCloudDays")]
    SyncCloudDays,
    [Description("IsCloudSyncMenuVisible")]
    IsCloudSyncMenuVisible,
    [Description("IsPatientsNameAnonymized")]
    IsPatientsNameAnonymized,
    [Description("IsServiceMenuVisible")]
    IsServiceMenuVisible
}

public enum FilterType
{
    DAY = 0,
    WEEK = 1,
    MONTH = 2,
    YEAR = 3,
    CUSTOM = 4,
    FIVEYEAR = 5,
    FULLVIEW = 6,
    TWENTYYEAR = 7
}

[TypeConverter(typeof(EnumDescriptionConverter))]
public enum SchedulerTypes
{
    //[Description("Consolidate View")]
    //ConsolidateView = 0,
    [Description("Room View")]
    RoomView = 1,
    [Description("Doctor View")]
    DoctorView = 2
}


[TypeConverter(typeof(EnumDescriptionConverter))]
public enum CustomSchedulerViewType
{
    [Description("Week")]
    Week = 2,
    [Description("Day")]
    Day = 1,
    [Description("Month")]
    Month = 0,
}

