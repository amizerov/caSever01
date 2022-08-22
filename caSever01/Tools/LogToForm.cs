namespace caSever01;

public class Msg
{
    public int id;
    public int type;
    public string src = "";
    public string msg = "";
    public Msg(int t, string s, string m, int n = 0)
    {
        type = t; src = s; msg = m; id = n;
    }
    public static void Send(int t, int n, Product p)
    {
        string s = p.symbol,
            m = $"v={p.volatility};\tl={p.liquidity};\tc1={p.cnt1};\tc2={p.cnt2};\tc3={p.cnt3}";
        LogToForm.Write(new Msg(t, s, m, n));
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