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
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using Color = System.Drawing.Color;
using System.Windows.Media.Animation;

namespace spectrum_sniffer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		
		Dictionary<Color, int> colours = new Dictionary<Color, int>();
		int brojac = 0;
		List<Color> colorss = new List<Color>();

		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Images | *.jpg; *.jpeg; *.png";
			bool? success = ofd.ShowDialog();
			if (success == true )
			{
				string path = ofd.FileName;
				Bitmap bitmap = new Bitmap(path);

				for (int y = 0; y < bitmap.Height; y++)
				{
					for (int x = 0; x < bitmap.Width; x++)
					{
						Color pixelColor = bitmap.GetPixel(x, y);

						int R = (pixelColor.R / 10) * 10; 
						int G = (pixelColor.G / 10) * 10;
						int B = (pixelColor.B / 10) * 10;

						Color px = Color.FromArgb(R, G, B);

						if (!colours.ContainsKey(px))
						{
							colours.Add(px, ++brojac);
						}
					}
				}


				foreach(var colour in colours)
				{
					int R = colour.Key.R;
					int G = colour.Key.G;
					int B = colour.Key.B;
					Color c = Color.FromArgb(R, G, B);
					colorss.Add(c);
				}

				List<double> maxDistances = new List<double>();
				List<Color> maxColours = new List<Color>();
				maxColours.Add(colorss[0]);
				for (int i = 0;i < 9; i++)
				{
					maxDistances.Add(GetMaxDistance(ref colorss, maxDistances, ref maxColours));
				}
				bitmap.Dispose();

				double k = EucledianDistance(Color.FromArgb(255, 230, 220), Color.FromArgb(80, 60, 90));
				Trace.WriteLine(k.ToString());
				Boja0.Text = maxColours[0].ToString();
				Boja1.Text = maxColours[1].ToString();
				Boja2.Text = maxColours[2].ToString();
				Boja3.Text = maxColours[3].ToString();
				Boja4.Text = maxColours[4].ToString();
				Boja5.Text = maxColours[5].ToString();
				Boja6.Text = maxColours[6].ToString();
				Boja7.Text = maxColours[7].ToString();
				Boja8.Text = maxColours[8].ToString();
				Boja9.Text = maxColours[9].ToString();

				Colour0.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[0].R, maxColours[0].G, maxColours[0].B));
				Colour1.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[1].R, maxColours[1].G, maxColours[1].B));
				Colour2.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[2].R, maxColours[2].G, maxColours[2].B));
				Colour3.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[3].R, maxColours[3].G, maxColours[3].B));
				Colour4.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[4].R, maxColours[4].G, maxColours[4].B));
				Colour5.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[5].R, maxColours[5].G, maxColours[5].B));
				Colour6.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[6].R, maxColours[6].G, maxColours[6].B));
				Colour7.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[7].R, maxColours[7].G, maxColours[7].B));
				Colour8.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[8].R, maxColours[8].G, maxColours[8].B));
				Colour9.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, maxColours[9].R, maxColours[9].G, maxColours[9].B));
				
			}

		}
		double[] Convert_RGB_to_YUV(int R, int G, int B)
		{
			double Y = 0.299 * R + 0.587 * G + 0.114 * B;
			double U = -0.14713 * R - 0.28886 * G + 0.436 * B;
			double V = 0.615 * R - 0.51499 * G - 0.10001 * B;
			double[] YUV = {Y, U, V};
			return YUV;
		}

		double EucledianDistance(Color c1, Color c2)
		{
			double[] YUV1 = Convert_RGB_to_YUV(c1.R, c1.G, c1.B);
			double[] YUV2 = Convert_RGB_to_YUV(c2.R, c2.G, c2.B);

			double R = Math.Pow(YUV1[0] - YUV2[0], 2);
			double G = Math.Pow(YUV1[1] - YUV2[1], 2);
			double B = Math.Pow(YUV1[2] - YUV2[2], 2);

			double D = Math.Sqrt(R + G + B);
			if (D > 5)
			{
				return D;
			}
			else return -1;
		}

		double GetMaxDistance(ref List<Color> colorss, List<double> maxDistances, ref List<Color> maxColours)
		{
			Dictionary<int, Color> distances = new Dictionary<int, Color>();
			bool already_there = false;
			for (int i = 1; i < colorss.Count; i++)
			{
				double k = EucledianDistance(colorss[0], colorss[i]);
				if ( k != -1)
				{
					for (int j = 0; j < maxDistances.Count; j++)
					{
						foreach(var c in maxColours)
						{
							if ((EucledianDistance(colorss[i], c) < 5))
							{
								already_there = true;
								break;
							}
						}

					}
					if (already_there == false && !distances.ContainsKey((int)k))
						distances.Add((int)k, colorss[i]);
				}
				
			}

			double max = -1;
			foreach(var colour in distances)
			{
				for (int i = 0; i < distances.Count; i++)
				{
					if (max < colour.Key)
						max = colour.Key;
				}
			}

			if (max != -1)
			{
				maxColours.Add(distances[(int)max]);
			}

			distances.Clear();

			return max;
		}
	}
}