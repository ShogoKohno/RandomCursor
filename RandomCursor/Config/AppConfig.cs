namespace RandomCursor.Config;

public class AppConfig
{
    public string Prefix { get; set; }
        = "ランダム_";


    public bool AvoidPrevious { get; set; }
        = true;


    public bool WriteLog { get; set; }
        = true;


    public string LogFile { get; set; }
        = "RandomCursor.log";

    public int MaxLogSizeMB { get; set; }
        = 1;
}