namespace RandomCursor.Config;

public class AppConfig
{
    public string Prefix { get; set; }
        = "ランダム_";

    public bool AvoidPrevious { get; set; }
        = true;

    public bool WriteLog { get; set; }
        = false;

    public string LogFile { get; set; }
        = "RandomCursor.log";
}