using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace StreetFoo.FakeCloud
{
    public partial class Form1 : Form
    {
        private WnsAuthorization Authentication { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonAuthenticate_Click(object sender, EventArgs e)
        {
            this.buttonAuthenticate.Enabled = false;
            try
            {
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

                //var content = new StringContent(string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com",
                //    HttpUtility.UrlEncode(this.textSid.Text).Trim(), HttpUtility.UrlEncode(this.textSecret.Text).Trim()));
                //content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";

                //var client = new HttpClient();
                //var result = await client.PostAsync("https://login.live.com/accesstoken.srf", content);

                //string json = await result.Content.ReadAsStringAsync();
                //var tokens = JObject.Parse(json);

                //// then...
                //// <toast><visual><binding template="ToastText01"><text id="1"></text></binding></visual></toast>
                //content = new StringContent("<toast><visual><binding template=\"ToastText01\"><text id=\"1\">Hello, world.</text></binding></visual></toast>");
                //content.Headers.ContentType.MediaType = "text/xml";
                //content.Headers.Add("X-WNS-Type", "wns/toast");
                //string token = (string)tokens["access_token"];

                //client = new HttpClient(new OAuthHandler(token));
                //var notifyUrl = "https://db3.notify.windows.com/?token=AgUAAAAG%2fwQVJ9PM6EMSDDMiin2IxCawutqNORruZSEiDBd552tc0xTG2i7RuZjiXdRECHh%2bFJvj7pwlq02A07VnbJcHnloxvZmNjSk4lx7gDo6vefMRv2%2fxv83m3rp%2b3dIsQH4%3d";
                //result = await client.PostAsync(notifyUrl, content);

                //// what?
                //MessageBox.Show(this, string.Format("{0}: {1}", result.StatusCode, await result.Content.ReadAsStringAsync()));

                // get the token...
                var authenticator = new WnsAuthorizer();
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

        //private class OAuthHandler : DelegatingHandler
        //{
        //    private string Token { get; set; }

        //    internal OAuthHandler(string token)
        //        : base(new HttpClientHandler())
        //    {
        //        this.Token = token;
        //    }

        //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //    {
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
        //        //request.Headers.Host = "cloud.notify.windows.com";
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //}

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            this.buttonSend.Enabled = false;
            try
            {
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

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var template in NotificationTemplate.GetTemplates())
                this.listContent.Items.Add(template);

            if (this.listContent.Items.Count > 0)
                this.listContent.SelectedIndex = 0;
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
