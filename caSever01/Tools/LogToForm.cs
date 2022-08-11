public class Msg
{
    public int type;
    public string src = "";
    public string msg = "";
    public Msg(int t, string s, string m)
    {
        type = t; src = s; msg = m;
    }
    public static void Send(int t, string s, string m)
    {
        LogToForm.Write(new Msg(t, s, m));
    }
}
class LogToForm
{
    static Action<Msg>? _write;
    public static void Write(Msg msg) => _write?.Invoke(msg);

    static object _lockFlag = new object();
    static LogToForm? _instance;

    public static LogToForm Instance
    {
        get
        {
            lock (_lockFlag)
            {
                if (_instance == null)
                    _instance = new LogToForm();
            }
            return _instance;
        }
    }
    public void Init(Action<Msg> write)
    {
        _write = write;
    }
}