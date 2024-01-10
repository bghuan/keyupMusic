namespace keyupMusic;
using KeyboardHook;
using System.Media;

public partial class Form1 : Form
{
    KeyEventHandler myKeyEventHandeler;
    KeyboardHook k_hook = new KeyboardHook();
    SoundPlayer player = new SoundPlayer();
    Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
    Keys[] keys_stop = { Keys.Escape };
    DateTime dateTime = new DateTime(2000, 1, 1);

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
        if (keys.Contains(e.KeyCode))
        {
            string wav = e.KeyCode.ToString().Replace("D", "") + ".wav";

            if (DateTime.Now - dateTime < TimeSpan.FromSeconds(10)) return;
            if (!File.Exists(wav)) return;

            player = new SoundPlayer(wav);
            player.Play();

            dateTime = DateTime.Now;

            //winBinWallpaper.changeImg();
        }
        else if (keys_stop.Contains(e.KeyCode))
        {
            player.Stop();
            dateTime = new DateTime(2000, 1, 1);
        }
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
