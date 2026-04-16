
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

class Program
{
    static Thread thread;
    static bool cancel = false;

    [STAThread]
    static void Main()
    {
        Console.WriteLine("1 - Console");
        Console.WriteLine("2 - WinForms");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine("Введи шлях:");
            string path = Console.ReadLine();

            thread = new Thread(() => Encrypt(path));
            thread.Start();

            Console.WriteLine("Нажми q для отмены");

            if (Console.ReadKey().KeyChar == 'q')
            {
                cancel = true;
            }
        }
        else
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }

    static void Encrypt(string path)
    {
        string text = File.ReadAllText(path);
        string result = "";

        foreach (char c in text)
        {
            if (cancel)
            {
                Console.WriteLine("Скасовано");
                return;
            }

            result += (char)(c + 3);
        }

        File.WriteAllText(path + ".enc", result);
        Console.WriteLine("Готово");
    }
}

public class Form1 : Form
{
    TextBox textBox1;
    Button button1;
    Button button2;

    Thread thread;
    bool cancel = false;

    public Form1()
    {
        textBox1 = new TextBox();
        button1 = new Button();
        button2 = new Button();

        textBox1.Top = 10;
        textBox1.Width = 200;

        button1.Text = "Start";
        button1.Top = 40;
        button1.Click += Start_Click;

        button2.Text = "Cancel";
        button2.Top = 40;
        button2.Left = 100;
        button2.Click += Cancel_Click;

        Controls.Add(textBox1);
        Controls.Add(button1);
        Controls.Add(button2);
    }

    void Start_Click(object sender, EventArgs e)
    {
        cancel = false;
        thread = new Thread(() => Encrypt(textBox1.Text));
        thread.Start();
    }

    void Cancel_Click(object sender, EventArgs e)
    {
        cancel = true;
    }

    void Encrypt(string path)
    {
        string text = File.ReadAllText(path);
        string result = "";

        foreach (char c in text)
        {
            if (cancel)
                return;

            result += (char)(c + 3);
        }

        File.WriteAllText(path + ".enc", result);
    }
}
