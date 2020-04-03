using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace DotLangTranslator
{
    public partial class Form1 : Form
    {
        public List<string> formated = new List<string>();
        public string filePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result =  MessageBox.Show("You did not select a language for translation! The default language for translation is English - Russian (Click No to use the default language)", "Info", MessageBoxButtons.YesNo);

            if(result == DialogResult.Yes)  
            {
                Form2 select = new Form2();
                select.ShowDialog();
            }

            List<string> selected = new Form2().selectedItems;

            if (selected.Count() <= 0)
            {
                selected.Add("en");
                selected.Add("ru");
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Lang files (*.lang)|*.lang";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    string[] fileContent = File.ReadAllLines(filePath, Encoding.UTF8);                    
                    MessageBox.Show("It will take a lot of time (Depends on api)", "Info");                 
                    int length = fileContent.Length;
                    label2.Visible = true;
                    label2.Refresh();
                    progressBar1.Visible = true;
                    progressBar1.Maximum = length;
                    int index = 1;
                    foreach (string s in fileContent)
                    {
                        label2.Text = $"Translating {index} of {length}";
                        label2.Refresh();
                        progressBar1.Value += 1;
                        index++;
                        if (s.IndexOf('=') >= 0)
                        {
                            string text = WebUtility.UrlEncode(s.Substring(s.IndexOf('=') + 1));
                            var che = Get($"https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20200402T222348Z.6ce0f256c702bf80.98e1d734a9dcd384e34a8cb246fa3224ac709887&text={text}&lang={selected[0]}-{selected[1]}");
                            formated.Add(che);
                        }
                    }
                    MessageBox.Show("Now select where to save!");
                    progressBar1.Visible = false;
                    label2.Visible = false;
                }
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.RestoreDirectory = true;

            string[] fileContent = File.ReadAllLines(filePath, Encoding.UTF8);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    string path = saveFileDialog1.FileName;
                    using (StreamWriter sw = new StreamWriter(File.Create(path), Encoding.UTF8))
                    {
                        int index = 0;
                        foreach (string s in fileContent)
                        {                           
                            if(s.IndexOf("=") >= 0)
                            {
                                sw.WriteLine(s.Replace(s.Substring(s.IndexOf("=") + 1), formated[index]));
                                index++;
                            }                            
                        }                        
                    }
                    MessageBox.Show("All is done!");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Text = "Если ты еще раз тыкнешь нахуй я тебя сломаю нахуй\nты не на того напал другалёчек ебать, я оффник.";
        }

        public dynamic Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var objText = reader.ReadToEnd();
                JObject joRespone = JObject.Parse(objText);
                string reurn = joRespone.SelectToken("text").Single().ToString();
                return reurn;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
