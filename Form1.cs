namespace keyupMusic;
using KeyboardHook;
using System.Media;
using System.Runtime.InteropServices;
using System.Timers;  

public partial class Form1 : Form
{
    private const int RID_INPUT = 0x1007;
    private const int RIM_TYPEKEYBOARD = 1;
    KeyEventHandler myKeyEventHandeler;
    KeyEventHandler myKeyEventHandeler_down;
    KeyboardHook k_hook = new KeyboardHook();
    SoundPlayer player = new SoundPlayer();
    SoundPlayer player_d = new SoundPlayer("d2.wav");
    Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
    Keys[] keys_stop = { Keys.Escape };
    Keys[] keys_exit = { Keys.D9, Keys.D9, Keys.D0 };
    string keys_exits = "990";
    string keys_exitsss = "990";
    bool keys_exitss = false;
    DateTime dateTime = new DateTime(2000, 1, 1);

    public Form1()
    {
        InitializeComponent();
        startListen();
        this.Load += new EventHandler(Form1_Load);
    }

    private static Timer aTimer;  
    private void Form1_Load(object sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Minimized;
        SetVisibleCore(false);
        
        aTimer = new Timer(3000); // 设置计时器间隔为 3000 毫秒  
        aTimer.Elapsed += OnTimedEvent; // 订阅Elapsed事件  
        aTimer.AutoReset = true; // 设置计时器是重复还是单次  
        aTimer.Enabled = true; // 启动计时器  
  
        Console.WriteLine("按Enter键退出...");  
        Console.ReadLine();  
            // pagedown_edge.yo(sender, e);
    }
    
    private static void OnTimedEvent(Object source, ElapsedEventArgs e)  
    {  
        pagedown_edge.yo(source, new KeyEventArgs(Keys.A));
        // Console.WriteLine("计时器事件触发，时间：{0}", e.SignalTime);  
    }  

    private void hook_KeyUp(object sender, KeyEventArgs e)
    {
        if (keys_exit.Contains(e.KeyCode) && is_exit(e.KeyCode))
        {
            if (keys_exitss)
            {
                player.Stop();
                dateTime = new DateTime(2100, 1, 1);
            }
            else
            {
                dateTime = new DateTime(2000, 1, 1);
            }
        }
        if (keys.Contains(e.KeyCode))
        {
            string wav = e.KeyCode.ToString().Replace("D", "") + ".wav";

            if (DateTime.Now - dateTime < TimeSpan.FromSeconds(30)) return;
            if (!File.Exists(wav)) return;

            player = new SoundPlayer(wav);
            player.Play();

            dateTime = DateTime.Now;

            new Thread(new ThreadStart(winBinWallpaper.changeImg)).Start();
        }
        else if (keys_stop.Contains(e.KeyCode) && dateTime < new DateTime(2099, 1, 1))
        {
            player.Stop();
            dateTime = new DateTime(2000, 1, 1);
        }
    }
    private void hook_KeyDown(object sender, KeyEventArgs e)
    {
        player_d.Play();
    }
    public bool is_exit(Keys keys)
    {
        string number = keys.ToString().Replace("D", "");
        keys_exitsss = (keys_exitsss + number).Substring(1, keys_exitsss.Length);
        if (keys_exits == keys_exitsss)
        {
            keys_exitss = !keys_exitss;
            return true;
        }
        return false;
    }

    public void startListen()
    {
        myKeyEventHandeler = new KeyEventHandler(hook_KeyUp);
        myKeyEventHandeler_down = new KeyEventHandler(hook_KeyDown);
        k_hook.KeyUpEvent += myKeyEventHandeler;
        k_hook.KeyDownEvent += myKeyEventHandeler_down;
        k_hook.Start();
    }
    public void stopListen()
    {
        if (myKeyEventHandeler != null)
        {
            k_hook.KeyUpEvent -= myKeyEventHandeler;
            k_hook.KeyDownEvent -= myKeyEventHandeler_down;
            myKeyEventHandeler = null;
            k_hook.Stop();
        }
    }
    protected override void Dispose(bool disposing)
    {
        stopListen();

        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}
