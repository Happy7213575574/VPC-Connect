using System;
namespace ConnectApp.Analytics
{
    public interface ICrashlyticsReporter
    {
        void Init();
        void SimulateCrash();
        void RecordException(Exception exception);
        void RecordExceptionObject(object obj);
        void RecordError(string error);
    }
}
