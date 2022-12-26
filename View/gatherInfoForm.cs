using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicUploaderGUI
{
    public partial class gatherInfoForm : Form
    {
        private IFormValidator validator;
        private IDownloader downloader;

        public gatherInfoForm()
        {

            InitializeComponent();
            this.validator = new FormValidator();
            this.downloader = new Downloader();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.setIpAddressTextFieldVisibility(this.uploadCheckBox.Checked);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Wether or not to upload to ip address
            bool upload = this.uploadCheckBox.Checked;
            // if true -> playlist; if false -> video
            bool playlist = this.radioButton1.Checked;

            // Get all values from form
            string url = this.urlTextBox.Text;
            string ip = this.ipTextBox.Text;
            int index = -1;
            if (this.textBox1.Text != "")
                index += int.Parse(this.textBox1.Text); // += ensures that index starts at 0 to make things easier.

            // Submit values to validator
            if (upload)
            {
                if (playlist)
                    this.validator.NewSubmit(url, ip, index);
                else
                    this.validator.NewSubmit(url, ip);
            }
            else
            {
                if (playlist)
                    this.validator.NewSubmit(url, index);
                else
                    this.validator.NewSubmit(url);
            }

            // TODO : validate each value
            bool readyToDownload = true;
            bool isUrlValid = validator.IsUrlValid();
            bool isIpValid = validator.IsIpValid();
            bool isIndexValid = validator.IsIndexValid();
            onUrlValidity(isUrlValid);
            onIpValidity(isIpValid);
            onIndexValidity(isIndexValid);
            
            if (isIndexValid && isIpValid && isUrlValid)
            {
                // TODO : download
                // use DownloadView instead and send it download info.
                //this.Hide();
                //Form form2 = new DownloadForm();
                //form2.Closed += (s, args) => this.Close();
                //form2.Show();


                if (upload)
                {
                    //if (playlist)
                    // 
                    //   else
                }
                else
                {
                    if (playlist)
                        downloader.Download(url, index);
                    //else
                    //    this.validator.NewSubmit(url);
                }

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Visible = radioButton1.Checked;
            this.label1.Visible = radioButton1.Checked;
            this.label4.Visible = radioButton1.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = this.textBox1.SelectionStart;

            if (this.textBox1.TextLength > 0)
            {
                for (int i = 0; i < this.textBox1.TextLength; i++)
                {
                    if (!char.IsDigit(this.textBox1.Text[i]))
                    {
                        this.textBox1.Text = this.textBox1.Text.Remove(i, 1);
                        cursorPos--;
                        i--;
                    }
                }
            }
            this.textBox1.Select(cursorPos, 0);
        }
    }
}
