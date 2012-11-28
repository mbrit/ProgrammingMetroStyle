using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EncryptionScratch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string keyAsHex = "d1ee5548bae50c6c52e785dbee523f022a1f39eb316dad2d2d50cc72957da4ef";
        private const string ivAsHex = "b2ba13011d845de7be1a246331a46f5d56ceea4bb6e81fde547b54440ad6d415";

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void HandleEncryptClick(object sender, RoutedEventArgs e)
        {
            // input...
            var input = CryptographicBuffer.ConvertStringToBinary(this.textData.Text, BinaryStringEncoding.Utf8);

            // create...
            var keyMaterial = CryptographicBuffer.DecodeFromHexString(keyAsHex);
            var iv = CryptographicBuffer.DecodeFromHexString(ivAsHex);

            // encrypt...
            var encryptor = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var key = encryptor.CreateSymmetricKey(keyMaterial);
            var encrypted = CryptographicEngine.Encrypt(key, input, iv);

            // show...
            this.textData.Text = CryptographicBuffer.EncodeToHexString(encrypted);
        }

        private void HandleDecryptClick(object sender, RoutedEventArgs e)
        {
            // input...
            var input = CryptographicBuffer.DecodeFromHexString(this.textData.Text);

            // create...
            var keyMaterial = CryptographicBuffer.DecodeFromHexString(keyAsHex);
            var iv = CryptographicBuffer.DecodeFromHexString(ivAsHex);

            // decrypt...
            var decryptor = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var key = decryptor.CreateSymmetricKey(keyMaterial);
            var decrypted = CryptographicEngine.Decrypt(key, input, iv);

            // show...
            this.textData.Text = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decrypted);
        }
    }
}
