namespace caSever01
{
    public partial class FrmMain : Form
    {
        List<Exchange> _exchangeList = new() { new Binance(), new Kucoin(), new Huobi() };

        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogToForm.Instance.Init(Progress);
        }

        void Progress(Msg msg)
        {
            string m =
                $"{msg.id}: [{DateTime.Now.ToString("hh:mm:ss")}]\t{msg.src.PadLeft(15, '-')}\t{msg.msg}\r\n";

            Invoke(new Action(() =>
            {
                switch (msg.type)
                {
                    case 1:
                        txtLog1.Text = m + txtLog1.Text;
                        break;
                    case 2:
                        txtLog2.Text = m + txtLog2.Text;
                        break;
                    case 3:
                        txtLog3.Text = m + txtLog3.Text;
                        break;
                }
            }));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            foreach(Exchange exchange in _exchangeList)
            {
                exchange.UpdateProductsStat();
            }
        }
    }
}