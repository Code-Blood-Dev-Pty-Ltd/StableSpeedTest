﻿using StableSpeedTest.Models;
using StableSpeedTest.Routines;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FIO = System.IO;

namespace StableSpeedTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient httpClient;
        private string defaultFile = "default_downloads.json";
        private DownloadModels downloadModels = new DownloadModels();

        // Create a thread-safe timer with an interval of 60 seconds.
        //private System.Threading.Timer timer = new System.Threading.Timer(
        //               async (e) =>
        //               {
        //                   var _f = "default_downloads.json";

        //                   var result = new DownloadModels();

        //                   if (FIO.File.Exists(_f))
        //                   {
        //                       result = Routines.JSONRoutines.ParseJsonToModels(FIO.File.ReadAllText(_f));
        //                   }

        //                   result = await RunTestAsync(_f, result.Items);
        //               },
        //                          null,
        //                                     TimeSpan.Zero,
        //                                                TimeSpan.FromSeconds(60));

        public MainWindow()
        {
            httpClient = new HttpClient();
            Initialized += MainWindow_Initialized;
            InitializeComponent();
        }

        private void MainWindow_Initialized(object? sender, EventArgs e)
        {
            if (FIO.File.Exists(defaultFile))
            {
                var _contents = FIO.File.ReadAllText(defaultFile);
                downloadModels = JSONRoutines.ParseJsonToModels(_contents);
            }
        }

        private void GenerateDefaultFile()
        {
            var _f = "default_downloads.json";

            if (FIO.File.Exists(_f))
            {
                FIO.File.Delete(_f);
            }

            var defaultDownloads = new DownloadModels();

            defaultDownloads.Items.Add(new DownloadModel() { Url = "https://www.gov.za/sites/default/files/gcis_document/201505/act36of1919s.pdf" });
            defaultDownloads.Items.Add(new DownloadModel() { Url = "https://www.gov.za/sites/default/files/gcis_document/201505/act-32-1944.pdf" });

            _ = RunTestAsync(_f, defaultDownloads.Items);
        }

        private async Task<DownloadModels> RunTestAsync(string fileName, List<DownloadModel> items)
        {
            var result = new DownloadModels();

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (radioButtonManual.IsChecked == true)
                {
                    buttonRunTest.IsEnabled = false;
                    radioButtonAuto.IsEnabled = false;
                    radioButtonAuto.IsEnabled = false;
                }
            });

            try
            {
                if (items != null && items.Any())
                {
                    foreach (var i in items)
                    {
                        Debug.WriteLine($"Downloading {i.Url}");

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = $"{DateTime.Now:yyyy MMM dd HH:mm:ss}\nDownloading: {i.Url}";
                            textBlock.Margin = new Thickness(5, 5, 5, 0);
                            textBlock.Padding = new Thickness(3);
                            textBlock.Background = Brushes.DeepSkyBlue;
                            textBlock.Foreground = Brushes.Black;
                            stackPanelMain.Children.Add(textBlock);
                        });

                        var content = await HTTPRoutines.DownloadFile(httpClient, i.Url, 4096, new Progress<int>(x =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                progressBarMain.Value = x;

                                if (x == 100)
                                {
                                    progressBarMain.Value = 0;
                                }
                            });

                        }), CancellationToken.None);

                        var history = new SpeedHistoryModel();
                        history.FileSizeInBytes = content.DownloadSizeInBytes;
                        history.TimeInSeconds = content.TimeDuration.TotalSeconds;
                        history.TestedEvent = DateTime.Now;
                        i.Histories.Items.Add(history);
                        result.Items.Add(i);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var _saturation = history.TimeInSeconds == 0 ? 0.0 : history.TimeInSeconds > 1 ? 100.0 : Convert.ToDouble(history.TimeInSeconds) * 100.0;
                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = $"{DateTime.Now:yyyy MMM dd HH:mm:ss}\nStats: {Convert.ToDecimal(history.FileSizeInBytes) / Convert.ToDecimal((1024 * 1024)):0.0} MB in {history.TimeInSeconds:0.000} seconds\nEstimated bandwidth: {history.BandwidthMbps:0.0} Mbps. Line Saturation: {_saturation:0.0}%";
                            textBlock.Margin = new Thickness(5, 5, 5, 0);
                            textBlock.Padding = new Thickness(3);
                            textBlock.Background = Brushes.LawnGreen;
                            textBlock.Foreground = Brushes.Black;
                            _ = stackPanelMain.Children.Add(textBlock);
                            scrollViewerMain.ScrollToBottom();
                        });
                    }
                }

                JSONRoutines.WriteToJsonFile(fileName, result);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (radioButtonManual.IsChecked == true)
                {
                    buttonRunTest.IsEnabled = true;
                    radioButtonAuto.IsEnabled = true;
                    radioButtonAuto.IsEnabled = true;
                }
            });

            return result;
        }

        private async void buttonRunTest_Click(object sender, RoutedEventArgs e)
        {
            downloadModels = await RunTestAsync(defaultFile, downloadModels.Items);
        }

        private void radioButtonAuto_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxInterval.IsEnabled = true;
        }

        private void radioButtonAuto_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxInterval.IsEnabled = false;
        }

        private void radioButtonManual_Checked(object sender, RoutedEventArgs e)
        {
            if (buttonRunTest != null)
            {
                buttonRunTest.IsEnabled = true;
            }
        }

        private void radioButtonManual_Unchecked(object sender, RoutedEventArgs e)
        {
            if (buttonRunTest != null)
            {
                buttonRunTest.IsEnabled = false;
            }
        }
    }
}