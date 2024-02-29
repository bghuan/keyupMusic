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
    Keys[] keys_exit = { Keys.D9, Keys.D9, Keys.D8 };
    string keys_exits = "998";
    string keys_exitsss = "998";
    bool keys_exitss = false;
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

            //winBinWallpaper.changeImg();
        }
        else if (keys_stop.Contains(e.KeyCode) && dateTime < new DateTime(2099, 1, 1))
        {
            player.Stop();
            dateTime = new DateTime(2000, 1, 1);
        }
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
        k_hook.KeyUpEvent += myKeyEventHandeler;
        k_hook.Start();
    }
    public void stopListen()
    {
        if (myKeyEventHandeler != null)
        {
            k_hook.KeyUpEvent -= myKeyEventHandeler;
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
