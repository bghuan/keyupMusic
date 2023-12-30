namespace keyupMusic;
using KeyboardHook;
using System.Timers;
using System.Media;

public partial class Form1 : Form
{
    KeyEventHandler myKeyEventHandeler;
    KeyboardHook k_hook = new KeyboardHook();
    SoundPlayer player = new SoundPlayer();
    bool in_limit_second = false;
    bool in_limit_second2 = false;
    int limit_second = 10000;

    Keys key1 = Keys.LWin;
    Keys key2 = Keys.Oemtilde;

    public Form1()
    {
        InitializeComponent();
        startListen();
        this.Load += new EventHandler(Form1_Load);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Minimized;
        SetVisibleCore(false);
    }

    private void hook_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode.Equals(key1) && !in_limit_second)
        {
            try
            {
                player = new SoundPlayer("e.wav");
                player.Play();
                in_limit_second = true;
                in_limit_second2 = false;
                time();
            }
            catch (Exception) { }
        }
        else if (e.KeyCode.Equals(key2) && !in_limit_second2)
        {
            try
            {
                player = new SoundPlayer("c.wav");
                player.Play();
                in_limit_second = false;
                in_limit_second2 = true;
                time();
            }
            catch (Exception) { }
        }
        else if (e.KeyCode.Equals(Keys.Escape))
        {
            player.Stop();
            in_limit_second = false;
            in_limit_second2 = false;
        }
    }

    //前次的定时恢复会影响到后次的重定时
    public void time()
    {
        Timer timer = new Timer(limit_second);
        timer.AutoReset = false;
        timer.Enabled = true;
        timer.Elapsed += new ElapsedEventHandler(release_limit_second);
        timer.Start();
    }
    public void release_limit_second(object source, System.Timers.ElapsedEventArgs e)
    {
        in_limit_second = false;
        in_limit_second2 = false;
    }
    public void startListen()
    {
        var myKeyEventHandeler = new KeyEventHandler(hook_KeyUp);
        k_hook.KeyUpEvent += myKeyEventHandeler;
        k_hook.Start();
    }
    public void stopListen()
    {
        if (myKeyEventHandeler != null)
        {
            k_hook.KeyDownEvent -= myKeyEventHandeler;
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
