using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.UI.CustomControl;
using FF.Cockpit.UI.Views.UserControls.CloudSync;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using FF.Cockpit.UI.Views.UserControls.Dashboard;
using System.ComponentModel;
using System.Windows.Shapes;
using Syncfusion.Windows.Controls.Input;
using FF.Cockpit.Entity.StoreProcedure_Result;
using Amazon.Util.Internal;
using System.Text;
using Syncfusion.UI.Xaml.Grid;
using System.Windows.Controls;
using System.Threading;
using localisation = FF.Cockpit.Common.Properties.Resources;



namespace FF.Cockpit.UI.ViewModels
{
    public class CloudSyncViewModel : PropertyChangeHelper
    {
        #region Fields
        private ObservableCollection<TrackUploadStatusLog> originalTrackUploadStatusLogList;
        private ObservableCollection<TrackDownloadStatus> originalTrackDownloadStatusLogList;
        private static readonly RegionEndpoint s3BucketRegion = RegionEndpoint.EUCentral1;
        private readonly IAmazonS3 _s3Client;
        private BackgroundWorker backgroundWorker = null;
        Dictionary<string, string> imagesInfo;

        MiscellaneousAWSConfigurationModel miscAWSConfiguration = null;

        #endregion

        #region Constructor
        public CloudSyncViewModel()
        {


            GetUpcomingAppointments();
            if (!AppStarter.IsCleanLocalStorageRunning)
                AppStarter.TotalFilesForCleanStorage = 0;
            CleanLocalStorageProgessVisibility = Visibility.Collapsed;
            #endregion

            #region Register Commands 
            UpcomingAppointmentList = new ObservableCollection<UpcomingAppointmentModel>();
            DownloadNowCommand = new RelayCommandHelper(x => DownloadNowMethod());
            ViewLogCommand = new RelayCommandHelper(x => ViewLogMethod(x));
            DownloadLogCommand = new RelayCommandHelper(async x => await DownloadLogMethod(x));
            OpenDownOrUploadDateChangeCommand = new RelayCommandHelper(x => OpenPoup(x));
            UploadAllServiceCommand = new RelayCommandHelper(x => UploadAllSyncService(x));
            UploadServiceCommand = new RelayCommandHelper(x => UploadSyncService(x));
            DownloadServiceCommand = new RelayCommandHelper(x => DownloadSyncService(x));
            SaveDownOrUploadDateChangeCommand = new RelayCommandHelper(x => SaveDateForDownOrUpload(x));
            SelectedPatientImagesCommand = new RelayCommandHelper(x => GetSelectedPatientImages(x));
            CleanLocalStorageCommand = new RelayCommandHelper(x => CleanLocalStorage());
            TrackDownloadStatusLogList = new ObservableCollection<TrackDownloadStatus>();
            CloudSyncFilterSelectionCommand = new RelayCommandHelper(x => CloudFilterSelection(x,false));
            CloudSyncUploadFilterSelectionCommand = new RelayCommandHelper(x => CloudFilterSelection(x, true));
            SelectedFilter = CloudFilterType.All.ToString();

            #region AWS Details And Clean Storage Region
            var details = new MiscellaneousAWSConfigurationModel().GetMiscellaneousConfigurationDetails();
            if (details != null)
            {
                miscAWSConfiguration = details.FirstOrDefault(x => x.IsActive);
                if (miscAWSConfiguration != null)
                    _s3Client = new AmazonS3Client(miscAWSConfiguration.AWSAccessKey, miscAWSConfiguration.AWSSecretKey, s3BucketRegion);
            }

            #endregion

            #region Background Events
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(DoWork_CleanLocalImages);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            #endregion
        }
        #endregion

        #region Properties

        private Visibility _cleanLocalStorageProgessVisibility;
        public Visibility CleanLocalStorageProgessVisibility
        {
            get { return _cleanLocalStorageProgessVisibility; }
            set { _cleanLocalStorageProgessVisibility = value; OnPropertyChanged(); }
        }
        private int _cleanLocalStorageProgessValue;
        public int CleanLocalStorageProgessValue
        {
            get { return _cleanLocalStorageProgessValue; }
            set { _cleanLocalStorageProgessValue = value; OnPropertyChanged(); }
        }
        private int _totalFilesForCleanStorage;
        public int TotalFilesForCleanStorage
        {
            get { return _totalFilesForCleanStorage; }
            set { _totalFilesForCleanStorage = value; OnPropertyChanged(); }
        }
        #region Download Now
        private int _downloadedImageCount;
        public int DownloadedImageCount
        {
            get { return _downloadedImageCount; }
            set { _downloadedImageCount = value; OnPropertyChanged(); }
        }

        private int _totalDownloadableImageCount;
        public int TotalDownloadableImageCount
        {
            get { return _totalDownloadableImageCount; }
            set { _totalDownloadableImageCount = value; OnPropertyChanged(); }
        }

        private bool _isDownloadNowStart;
        public bool IsDownloadNowStart
        {
            get { return _isDownloadNowStart; }
            set { _isDownloadNowStart = value; OnPropertyChanged(); }
        }
        #endregion

        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; OnPropertyChanged(); }
        }

        private bool _isDownLoadClick;
        public bool IsDownLoadClick
        {
            get { return _isDownLoadClick; }
            set { _isDownLoadClick = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UpcomingAppointmentModel> _upcomingAppointmentList;
        public ObservableCollection<UpcomingAppointmentModel> UpcomingAppointmentList
        {
            get { return _upcomingAppointmentList; }
            set { _upcomingAppointmentList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TrackUploadStatusLog> _trackUploadStatusLogList;
        public ObservableCollection<TrackUploadStatusLog> TrackUploadStatusLogList
        {
            get { return _trackUploadStatusLogList; }
            set { _trackUploadStatusLogList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TrackDownloadStatus> _trackDownloadStatusLogList;
        public ObservableCollection<TrackDownloadStatus> TrackDownloadStatusLogList
        {
            get { return _trackDownloadStatusLogList; }
            set { _trackDownloadStatusLogList = value; OnPropertyChanged(); }
        }
        private string selectedFilter;
        public string SelectedFilter
        {
            get { return selectedFilter; }
            set { selectedFilter = value; OnPropertyChanged(); }
        }


        #endregion

        #region Command

        public RelayCommandHelper ViewLogCommand { get; private set; }
        public RelayCommandHelper DownloadLogCommand { get; private set; }
        public RelayCommandHelper CloudSyncFilterSelectionCommand { get; private set; }
        public RelayCommandHelper DownloadNowCommand { get; private set; }
        public RelayCommandHelper OpenDownOrUploadDateChangeCommand { get; private set; }
        public RelayCommandHelper UploadAllServiceCommand { get; private set; }
        public RelayCommandHelper UploadServiceCommand { get; private set; }
        public RelayCommandHelper DownloadServiceCommand { get; private set; }
        public RelayCommandHelper SaveDownOrUploadDateChangeCommand { get; private set; }
        public RelayCommandHelper SelectedPatientImagesCommand { get; private set; }
        public RelayCommandHelper CleanLocalStorageCommand { get; private set; }
        public RelayCommandHelper CloudSyncUploadFilterSelectionCommand { get; private set; }

        #endregion

        #region Methods

        //method to select various selection filter
        public void CloudFilterSelection(object selectionType, bool isForUpload)
        {
            try
            {
                if (selectionType != null)
                {
                    Enum.TryParse<CloudFilterType>((string)selectionType, out CloudFilterType cloudFilterType);
                    SelectedFilter = cloudFilterType.ToString();
                    switch (cloudFilterType)
                    {
                        case CloudFilterType.All:
                            // No need to filter, show all data
                            if (isForUpload)
                                TrackUploadStatusLogList = originalTrackUploadStatusLogList != null ? originalTrackUploadStatusLogList : new ObservableCollection<TrackUploadStatusLog>();
                            else
                                TrackDownloadStatusLogList = originalTrackDownloadStatusLogList != null ? originalTrackDownloadStatusLogList : new ObservableCollection<TrackDownloadStatus>();
                            break;
                        case CloudFilterType.Images:
                            // Filter logic for 'Images' filter
                            if (isForUpload)
                                FilterDataByUploadFileType(ConstantHelper.Image);
                            else
                                FilterDataByFileType(ConstantHelper.Image);
                            break;
                        case CloudFilterType.Icons:
                            // Filter logic for 'Icons' filter
                            if (isForUpload)
                                FilterDataByUploadFileType(ConstantHelper.Icon);
                            else
                                FilterDataByFileType(ConstantHelper.Icon);
                            break;
                        case CloudFilterType.Videos:
                            // Filter logic for 'Videos' filter
                            if (isForUpload)
                                FilterDataByUploadFileType(ConstantHelper.Video);
                            else
                                FilterDataByFileType(ConstantHelper.Video);
                            break;
                        default:
                            // Handle unexpected filter types
                            if (isForUpload)
                                TrackUploadStatusLogList = originalTrackUploadStatusLogList;
                            else
                                TrackDownloadStatusLogList = originalTrackDownloadStatusLogList;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private async Task DownloadLogMethod(object selectedRow)
        {
            try
            {
                // Set default filter to All
                if (selectedRow != null)
                {
                    UpcomingAppointmentModel model = (UpcomingAppointmentModel)selectedRow;
                    await GetTrackDownloadStatusLogs(model.PatientId);
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void FilterDataByUploadFileType(string fileType)
        {
            var filteredUploadData = originalTrackUploadStatusLogList.Where(log => log.FileType == fileType).ToList();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                TrackUploadStatusLogList = filteredUploadData.ToObservableCollection();
            }));
        }
        private void FilterDataByFileType(string fileType)
        {
            var filteredData = originalTrackDownloadStatusLogList.Where(log => log.FileType == fileType).ToList();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                TrackDownloadStatusLogList = filteredData.ToObservableCollection();
            }));
        }
        private void ViewLogMethod(object selectedRow)
        {
            if (selectedRow != null)
            {
                UpcomingAppointmentModel model = (UpcomingAppointmentModel)selectedRow;
                GetTrackUploadStatusLogs(model.PatientId);
            }
        }

        private void UploadAllSyncService(object click)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*" };
            var fileName = "FF.Sync.UploadAllWorkerService.exe";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                ProcessDialogFile(openFileDialog, fileName);
                GetUpcomingAppointments();
            }
            // else CustomMessageBox.ShowOK(localisation.AttachFileMsgStr_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx);

        }
        private void UploadSyncService(object click)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*" };
            var fileName = "FF.Sync.UploadWorkerService.exe";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                ProcessDialogFile(openFileDialog, fileName);
                GetUpcomingAppointments();
            }
            // else CustomMessageBox.ShowOK(localisation.AttachFileMsgStr_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx);
        }
        private void DownloadSyncService(object click)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*" };
            var fileName = "FF.Sync.DownloadWorkerService.exe";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                ProcessDialogFile(openFileDialog, fileName);
                GetUpcomingAppointments();
            }

            // else CustomMessageBox.ShowOK(localisation.AttachFileMsgStr_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx);

        }
        private static void ProcessDialogFile(OpenFileDialog openFileDialog, string fileName)
        {
            if (System.IO.Path.GetFileName(openFileDialog.FileName) == fileName)
            {
                Process process = new Process();
                process.StartInfo.FileName = openFileDialog.FileName;
                process.Start();
                process.WaitForExit();
            }
            else CustomMessageBox.ShowOK(localisation.FileNoteExixtsMsg_resx, localisation.ProcessFailedMsg_resx, localisation.OkBtn_resx);
        }

        private UpdateDownOrUploadDate popup;
        private void OpenPoup(object clickedBtn)
        {
            List<UpcomingAppointmentModel> selectedAppointments = UpcomingAppointmentList.Where(x => x.IsSelected).ToList();

            if (clickedBtn != null && selectedAppointments.Count > 0)
            {
                IsDownLoadClick = clickedBtn.ToString().ToLower() == "btnscheduledownload";
                popup = new UpdateDownOrUploadDate(this);
                popup.Owner = AppStarter.MainWindow;
                popup.ShowDialog();
            }
            else
            {
                CustomMessageBox.ShowOK(localisation.PatientSelectionMsg_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx);
            }
        }
        private void SaveDateForDownOrUpload(object date)
        {
            //Insert CloudSyncInfo for download/upload the images b/w local and AWS by sync job(service)
            if (date != null)
            {
                DateTime selectedDate = Convert.ToDateTime(date);
                List<UpcomingAppointmentModel> selectedAppointments = UpcomingAppointmentList.Where(x => x.IsSelected).ToList();

                if (selectedDate != null && selectedAppointments.Count > 0)
                {
                    foreach (var appointment in selectedAppointments)
                    {
                        #region Cloud Sync Info Update
                        CloudSyncInfoModel cloudSyncInfoModel = new CloudSyncInfoModel();
                        cloudSyncInfoModel.Id = 101;
                        cloudSyncInfoModel.AppointmentId = appointment.AppointmentId;
                        cloudSyncInfoModel.PatientId = appointment.PatientId;

                        if (IsDownLoadClick)
                        {
                            cloudSyncInfoModel.DownloadDateTime = DateTime.Now > selectedDate ? DateTime.Now : selectedDate;
                            cloudSyncInfoModel.UploadDateTime = appointment.UploadDateTime;
                        }
                        else
                        {
                            cloudSyncInfoModel.DownloadDateTime = appointment.DownloadDateTime;
                            cloudSyncInfoModel.UploadDateTime = DateTime.Now > selectedDate ? DateTime.Now : selectedDate;
                        }
                        cloudSyncInfoModel.SyncType = SyncTypes.Manual.ToString();

                        cloudSyncInfoModel.InsertOrUpdateCloudSyncInfo_sync(cloudSyncInfoModel);
                        #endregion
                    }
                    GetUpcomingAppointments();
                }
            }
            popup.Close();
        }
        private async void GetUpcomingAppointments()
        {
            try
            {
                var patientsList = await new UpcomingAppointmentModel().GetUpcomingAppointmentList(DateTime.Today, DateTime.Today.AddDays(7));
                if (patientsList != null)
                {
                    GeImagesCountInAWS(patientsList);
                    UpcomingAppointmentList = patientsList.ToObservableCollection();

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void GeImagesCountInAWS(IEnumerable<UpcomingAppointmentModel> patientsList)
        {
            if (miscAWSConfiguration != null && patientsList != null)
            {
                foreach (var patient in patientsList)
                {
                    try
                    {
                        var folder = $"PatientId-{patient.PatientId}/";
                        var request = new ListObjectsV2Request
                        {
                            BucketName = miscAWSConfiguration.AWSBucketName,
                            Prefix = folder
                        };
                        var response = _s3Client.ListObjectsV2(request);
                        patient.AwsImagesCount = response.S3Objects.Count;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex);
                    }
                }
            }

        }
     

        private TrackDownloadStatusLogsPopup trackDownloadLogPopup;
        private async Task GetTrackDownloadStatusLogs(int patientId)
        {
            try
            {
                var trackDownloadStatusLogData = await new TrackDownloadStatusLog().GetTrackDownloadStatusLogList(patientId);

                List<TrackDownloadStatus> trackDownloadStatus = new List<TrackDownloadStatus>();
                foreach (var log in trackDownloadStatusLogData)
                {
                    TrackDownloadStatus trackDownload = new TrackDownloadStatus();
                    trackDownload.PatientId = log.PatientId;
                    trackDownload.PatientName = log.PatientName;
                    trackDownload.MachineName = log.MachineName;
                    trackDownload.MacAddress = log.MacAddress;
                    trackDownload.DownloadTime = log.DownloadTime;
                    switch (log.FileType)
                    {
                        case ConstantHelper.Image:
                            trackDownload.ImageNumber = log.ImageNumber;
                            trackDownload.FileName = log.ImageName;
                            trackDownload.FileStatus = log.ImageDownloadStatus;
                            trackDownload.FileType = log.FileType;
                            break;
                        case ConstantHelper.Icon:
                            trackDownload.ImageNumber = log.ImageNumber;
                            trackDownload.FileName = log.IconName;
                            trackDownload.FileStatus = log.IconDownloadStatus;
                            trackDownload.FileType = log.FileType;
                            break;
                        case ConstantHelper.Video:
                            trackDownload.FileName = log.VideoName;
                            trackDownload.FileStatus = log.VideoDownloadStatus;
                            trackDownload.FileType = log.FileType;
                            break;
                        default:
                            break;
                    }
                    trackDownloadStatus.Add(trackDownload);
                }
                if (trackDownloadStatus != null && trackDownloadStatus.Count() > 0)
                {
                    originalTrackDownloadStatusLogList = trackDownloadStatus.ToObservableCollection();
                    CloudFilterSelection(CloudFilterType.All.ToString(),false);
                    trackDownloadLogPopup = new TrackDownloadStatusLogsPopup(this);
                    trackDownloadLogPopup.Owner = AppStarter.MainWindow;
                    trackDownloadLogPopup.ShowDialog();
                }
                else
                {

                    CustomMessageBox.Show(localisation.DownloadLogNotFoundMsg_resx, localisation.DialogWindowAlertCaption_resx, MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private async void GetTrackUploadStatusLogs(int patientId)
        {
            try
            {
                var trackUploadStatusLogData = await new TrackUploadStatusLog().GetTrackUploadStatusLogList(patientId);

                List<TrackUploadStatusLog> trackUploadStatus = new List<TrackUploadStatusLog>();
                foreach (var log in trackUploadStatusLogData)
                {
                    TrackUploadStatusLog trackUpload = new TrackUploadStatusLog();
                    trackUpload.PatientId = log.PatientId;
                    trackUpload.PatientName = log.PatientName;
                    trackUpload.MachineName = log.MachineName;
                    trackUpload.MacAddress = log.MacAddress;
                    trackUpload.UploadTime = log.UploadTime;
                    switch (log.FileType)
                    {
                        case ConstantHelper.Image:
                            trackUpload.ImageNumber = log.ImageNumber;
                            trackUpload.FileName = log.ImageName;
                            trackUpload.FileStatus = log.ImageUploadStatus;
                            trackUpload.FileType = log.FileType;
                            break;
                        case ConstantHelper.Icon:
                            trackUpload.ImageNumber = log.ImageNumber;
                            trackUpload.FileName = log.IconName;
                            trackUpload.FileStatus = log.IconUploadStatus;
                            trackUpload.FileType = log.FileType;
                            break;
                        case ConstantHelper.Video:
                            trackUpload.FileName = log.VideoName;
                            trackUpload.FileStatus = log.VideoUploadStatus;
                            trackUpload.FileType = log.FileType;
                            break;
                        default:
                            break;
                    }
                    trackUploadStatus.Add(trackUpload);

                }
                if (trackUploadStatus != null && trackUploadStatus.Count() > 0)
                {
                    originalTrackUploadStatusLogList = trackUploadStatus.ToObservableCollection();
                    CloudFilterSelection(CloudFilterType.All.ToString(),true);
                    TrackUploadStatusLogsPopup popup = new TrackUploadStatusLogsPopup(this);
                    popup.Owner = AppStarter.MainWindow;
                    popup.ShowDialog();
                }
                else
                {
                    CustomMessageBox.Show(localisation.UploadLogNotFoundMsg_resx, localisation.DialogWindowAlertCaption_resx, MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }

        }

   

        #region Download Now(Images download from AWS to local machine)

        private async void DownloadNowMethod()
        {
            List<UpcomingAppointmentModel> selectedAppointments = UpcomingAppointmentList.Where(x => x.IsSelected).ToList();

            if (selectedAppointments.Count > 0)
            {
                int downloadCount = 1;
                var lst = UpcomingAppointmentList.Where(x => x.IsSelected).Select(x => x.PatientId).ToList();
                string selectedPatientIds = string.Join(",", lst);

                if (!string.IsNullOrWhiteSpace(selectedPatientIds))
                {
                    var patientImages = await new ImageInfoModel().GetPatientImagesInfo(selectedPatientIds);
                    if (patientImages != null && patientImages.Count() > 0)
                    {
                        AppStarter.IsDownloadNow_InProgress = IsDownloadNowStart = true;
                        AppStarter.TotalDownloadableImageCount_InProgress = TotalDownloadableImageCount = patientImages.Count();

                        foreach (var image in patientImages)
                        {
                            if (await DownloadPatientImagesAsync(image))
                            {
                                AppStarter.DownloadedImageCount_InProgress = DownloadedImageCount = downloadCount;
                                downloadCount++;
                                if (TotalDownloadableImageCount == DownloadedImageCount)
                                {
                                    AppStarter.IsDownloadNow_InProgress = IsDownloadNowStart = false;
                                    AppStarter.TotalDownloadableImageCount_InProgress = AppStarter.DownloadedImageCount_InProgress = TotalDownloadableImageCount = DownloadedImageCount = 0;
                                }
                            }
                        }

                        foreach (var appointment in selectedAppointments)
                        {
                            if (DownloadedImageCount > 0)
                            {
                                // Update cloud sync status
                                if (TotalDownloadableImageCount == DownloadedImageCount)
                                    UpdateCloudSyncDownloadStatus(appointment.AppointmentId, appointment.PatientId, Convert.ToInt32(SyncStatus.DownloadCompleted));
                                else
                                    UpdateCloudSyncDownloadStatus(appointment.AppointmentId, appointment.PatientId, Convert.ToInt32(SyncStatus.DownloadPartially));

                                GetUpcomingAppointments();
                            }
                        }
                    }
                    else
                        CustomMessageBox.ShowOK($"Uploaded file count in AWS is {0}", "Warning", "Ok");
                }
            }
            else
            {
                CustomMessageBox.ShowOK("Please select at least one patient.", "Warning", "Ok");
            }
        }

        public void UpdateCloudSyncDownloadStatus(int appointmentId, int patientId, int syncDownloadStatus)
        {
            new CloudSyncInfoModel().UpdateCloudSyncDownloadStatus(appointmentId, patientId, syncDownloadStatus);
        }
        public async Task<bool> DownloadPatientImagesAsync(ImageInfoModel image)
        {
            bool isDownloaded = false;
            try
            {
                var hasBackslashAtEnd = image.ImagePath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString());
                if (!hasBackslashAtEnd)
                    image.FullImagePath = $"{image.ImagePath}\\{image.ImageName}";

                var folder = $"PatientId-{image.PatientId}/";
                isDownloaded = await DownloadS3FileAsync(folder, image.FullImagePath);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return isDownloaded;
        }

        public async Task<bool> DownloadS3FileAsync(string s3FolderName, string fullImagePath)
        {
            bool isDownloaded = false;
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request()
                {
                    BucketName = miscAWSConfiguration.AWSBucketName,
                    Prefix = s3FolderName
                };
                ListObjectsV2Response response = await _s3Client.ListObjectsV2Async(request);
                if (response.S3Objects.Count == 0)
                    return isDownloaded;

                var localFileName = $"{s3FolderName}{System.IO.Path.GetFileName(fullImagePath)}";
                var s3Response = response.S3Objects.FirstOrDefault(p => p.Key == localFileName);

                if (s3Response != null)
                {
                    string dirPath = System.IO.Path.GetDirectoryName(fullImagePath);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    using (GetObjectResponse getObjectResponse = await _s3Client.GetObjectAsync(miscAWSConfiguration.AWSBucketName, s3Response.Key))
                    using (Stream responseStream = getObjectResponse.ResponseStream)
                    using (FileStream fileStream = File.Create(fullImagePath))
                    {
                        await responseStream.CopyToAsync(fileStream);
                    }
                    isDownloaded = true;
                }
            }
            catch (Exception)
            {
                isDownloaded = false;
            }
            return isDownloaded;
        }

        private void GetSelectedPatientImages(object obj)
        {
            if (obj != null)
            {
                var selectedPatient = (UpcomingAppointmentModel)obj;
                ObservableCollection<ImagesStatusBreakDownModel> imagesInfoList = null;
                SeriesCollection = new SeriesCollection();
                SeriesCollection.Clear();
                try
                {
                    var mapper = Mappers.Pie<ImagesStatusBreakDownModel>().Value(x => x.Value);
                    Charting.For<ImagesStatusBreakDownModel>(mapper);

                    imagesInfoList = new ObservableCollection<ImagesStatusBreakDownModel>();
                    imagesInfoList.Add(new ImagesStatusBreakDownModel() { Title = localisation.CloudSyncLocalImagesText_resx, Value = selectedPatient.LocalImagesCount, Color = "#FF0000" });
                    imagesInfoList.Add(new ImagesStatusBreakDownModel() { Title = localisation.CloudSyncAWSmagesText_resx, Value = selectedPatient.AwsImagesCount, Color = "#964B00" });

                    if (!imagesInfoList.All(x => x.Value == 0))
                    {
                        foreach (var item in imagesInfoList)
                        {
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                SeriesCollection.Add(
                                    new PieSeries
                                    {
                                        Title = item.Title,
                                        Values = new ChartValues<double> { item.Value },
                                        Fill = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(item.Color),
                                        Stroke = System.Windows.Media.Brushes.Transparent,
                                        StrokeThickness = 0,
                                    });
                            }));
                        }
                    }


                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex);
                }
            }
        }

        #endregion

        #region Clean Local Storage
        private async void CleanLocalStorage()
        {
            try
            {
                bool isPopUpDisplayed = false;
                if (UpcomingAppointmentList != null && UpcomingAppointmentList.Where(x => x.IsSelected).Count() > 0)
                {
                    if (AppStarter.SelectedPatientsIdForCleanStorage == null)
                    {
                        AppStarter.SelectedPatientsIdForCleanStorage = new ObservableCollection<object>();
                        foreach (var item in UpcomingAppointmentList)
                        {
                            AppStarter.SelectedPatientsIdForCleanStorage.Add(item);
                        }
                    }
                    foreach (var item in UpcomingAppointmentList.Where(x => x.IsSelected))
                    {
                        UpcomingAppointmentModel model = (UpcomingAppointmentModel)item;
                        var trackUploadStatusLogData = await new TrackUploadStatusLog().GetTrackUploadStatusLogList(model.PatientId);

                        if (trackUploadStatusLogData != null && trackUploadStatusLogData.Count() > 0)
                        {
                            if (imagesInfo == null)
                                imagesInfo = new Dictionary<string, string>();

                            AppStarter.TotalFilesForCleanStorage += trackUploadStatusLogData.Count();

                            //Images that are uploaded to the aws successfully
                            var imagesData = trackUploadStatusLogData.Where(x => x.UploadStatus == "Upload Completed");
                            {
                                foreach (var items in imagesData)
                                    if (!imagesInfo.ContainsKey(items.ImagePath + "\\" + items.ImageName))
                                        imagesInfo.Add(items.ImagePath + "\\" + items.ImageName, items.ImagePath);
                            }
                        }
                    }
                }
                else
                {
                    CustomMessageBox.ShowOK(localisation.PatientSelectionMsg_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx);
                    isPopUpDisplayed = true;
                }
                if (imagesInfo == null && !isPopUpDisplayed)
                {
                    CustomMessageBox.ShowOK($"{localisation.FileCountInAwsMsg_resx} {0}", localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx);
                }
                if (imagesInfo != null && imagesInfo.Count() > 0)
                {
                    backgroundWorker.WorkerReportsProgress = true;
                    backgroundWorker.WorkerSupportsCancellation = true;
                    backgroundWorker.RunWorkerAsync();

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void DeleteFile(string filePath, string iconPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (File.Exists(filePath.ToString().Replace(@"\\", @"\")))
                    {
                        File.Delete(filePath);
                        LogHelper.LogStep($"{localisation.ImageDeletionLogStr_resx} {filePath} -- {DateTime.Now}");
                    }
                    // Delete directory folder in case none of the Image left under that.
                    iconPath = iconPath.ToString().Replace(@"\\", @"\");

                    int fileCount = new DirectoryInfo(iconPath).EnumerateFileSystemInfos("*", SearchOption.AllDirectories).Count();
                    if (fileCount == 0)
                        System.IO.Directory.Delete(iconPath, false);
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public void DoWork_CleanLocalImages(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bw = sender as BackgroundWorker;
                if (imagesInfo.Count() > 0)
                {
                    int _counter = 1;
                    CleanLocalStorageProgessVisibility = Visibility.Visible;
                    foreach (var items in imagesInfo)
                    {
                        Thread.Sleep(100);
                        DeleteFile(items.Key, items.Value);
                        var value = (_counter * 100) / AppStarter.TotalFilesForCleanStorage;
                        CleanLocalStorageProgessValue = value != 100 ? Convert.ToInt32(value) % 100 : 100;

                        if (AppStarter.TotalFilesForCleanStorage == CleanLocalStorageProgessValue)
                            CleanLocalStorageProgessVisibility = Visibility.Collapsed;
                        bw.ReportProgress(CleanLocalStorageProgessValue);
                        _counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AppStarter.IsCleanLocalStorageRunning = false;
            CleanLocalStorageProgessVisibility = Visibility.Collapsed;
            CleanLocalStorageProgessValue = 100;
            CustomMessageBox.Show(localisation.CleanLocalStorageSuccessMsg_resx, localisation.DialogWindowAlertCaption_resx, MessageBoxButton.OK);
            // new CloudSyncViewModel();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CleanLocalStorageProgessValue = e.ProgressPercentage;
            AppStarter.CleanLocalStorageProgessValue = CleanLocalStorageProgessValue;
            if (!AppStarter.IsCleanLocalStorageRunning)
                AppStarter.IsCleanLocalStorageRunning = true;
        }

        #endregion
        #endregion 
    }


}
