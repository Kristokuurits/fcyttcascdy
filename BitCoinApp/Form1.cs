using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BitCoinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetRates_Click(object sender, EventArgs e)
        {
            if (CurrencyCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select a currency.");
                return;
            }

            if (string.IsNullOrEmpty(amountOfCoinBox.Text))
            {
                MessageBox.Show("Please enter a BTC amount.");
                return;
            }

            resultLabel.Visible = true;
            ResultTextBox.Visible = true;
            BitcoinRates bitcoin = GetRates();

            float rate;
            string code;

            if (CurrencyCombo.SelectedItem.ToString() == "EUR")
            {
                rate = bitcoin.bpi.EUR.rate_float;
                code = bitcoin.bpi.EUR.code;
            }
            else if (CurrencyCombo.SelectedItem.ToString() == "USD")
            {
                rate = bitcoin.bpi.USD.rate_float;
                code = bitcoin.bpi.USD.code;
            }
            else
            {
                MessageBox.Show("Unsupported currency.");
                return;
            }

            float result = float.Parse(amountOfCoinBox.Text) * rate;
            ResultTextBox.Text = $"{result.ToString()} {code}";
        }

        public static BitcoinRates GetRates()
        {
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            BitcoinRates bitcoin;
            using (var responseReader = new StreamReader(webStream))
            {
                var response = responseReader.ReadToEnd();
                bitcoin = JsonConvert.DeserializeObject<BitcoinRates>(response);
            }

            return bitcoin;
        }
    }

    public class BitcoinRates
    {
        public Bpi bpi { get; set; }
    }

    public class Bpi
    {
        public Eur EUR { get; set; }
        public Usd USD { get; set; }
        public Gbp GBP { get; set; }
    }

    public class Eur
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public float rate_float { get; set; }
        public string rate_string { get; set; }
        public string description { get; set; }
        public string rate_html { get; set; }
    }

    public class Usd
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public float rate_float { get; set; }
        public string rate_string { get; set; }
        public string description { get; set; }
        public string rate_html { get; set; }
    }

    public class Gbp
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public float rate_float { get; set; }
        public string rate_string { get; set; }
        public string description { get; set; }
        public string rate_html { get; set; }
    }
}