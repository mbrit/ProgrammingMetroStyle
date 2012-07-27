using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using MetroWnsPush;
using Newtonsoft.Json;

namespace StreetFoo.FakeCloud
{
    public partial class Form1 : Form
    {
        private WnsAuthentication Authentication { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonAuthenticate_Click(object sender, EventArgs e)
        {
            this.buttonAuthenticate.Enabled = false;
            try
            {
                SaveSettings();

                this.Authentication = null;

                string sid = this.textSid.Text.Trim();
                if (string.IsNullOrEmpty(sid))
                {
                    MessageBox.Show(this, "You must supply a SID.");
                    return;
                }
                string secret = this.textSecret.Text.Trim();
                if (string.IsNullOrEmpty(secret))
                {
                    MessageBox.Show(this, "You must supply a secret");
                    return;
                }

                // get the token...
                var authenticator = new WnsAuthenticator();
                this.Authentication = await authenticator.AuthenticateAsync(sid, secret);

                // et...
                this.textToken.Text = this.Authentication.Token;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
            finally
            {
                this.buttonAuthenticate.Enabled = true;
            }
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            this.buttonSend.Enabled = false;
            try
            {
                SaveSettings();

                string xml = this.textXml.Text.Trim();
                if (string.IsNullOrEmpty(xml))
                {
                    MessageBox.Show(this, "You must provide some XML.");
                    return;
                }
                string uri = this.textUri.Text.Trim();
                if (string.IsNullOrEmpty(uri))
                {
                    MessageBox.Show(this, "You must provide a client URI.");
                    return;
                }

                // token?
                if(this.Authentication == null)
                {
                    MessageBox.Show(this, "You must authenticate yourself.");
                    return;
                }

                // send...
                var pusher = new WnsPusher();
                var result = await pusher.PushAsync(this.Authentication, uri, xml);

                // ok?
                MessageBox.Show(this, result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
            finally
            {
                this.buttonSend.Enabled = true;
            }
        }

        private void LoadSettings()
        {
            var path = SettingsPath;
            if (!(File.Exists(path)))
                return;

            // load...
            string json = null;
            using (var reader = new StreamReader(path))
                json = reader.ReadToEnd();
            var settings = JsonConvert.DeserializeObject<Settings>(json);

            // set...
            this.textSecret.Text = settings.Secret;
            this.textSid.Text = settings.Sid;
            this.textUri.Text = settings.Uri;
        }

        private void SaveSettings()
        {
            var settings = new Settings()
            {
                Secret = this.textSecret.Text.Trim(),
                Sid = this.textSid.Text.Trim(),
                Uri = this.textUri.Text.Trim()
            };

            // save...
            var json = JsonConvert.SerializeObject(settings);
            string path = SettingsPath;
            using (var writer = new StreamWriter(path))
                writer.Write(json);       
        }

        private string SettingsPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                path = Path.Combine(path, "MetroWnsPush");
                if (!(Directory.Exists(path)))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "Settings.json");
                return path;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var template in NotificationTemplate.GetTemplates())
                this.listContent.Items.Add(template);

            if (this.listContent.Items.Count > 0)
                this.listContent.SelectedIndex = 0;

            this.LoadSettings();
        }

        private void listContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (NotificationTemplate)this.listContent.SelectedItem;
            if (selected != null)
                this.textXml.Text = selected.Content;
            else
                this.textXml.Text = null;
        }
    }
}
