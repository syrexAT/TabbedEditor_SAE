using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecentFileList
{
	/// <summary>
	/// Interaction logic for FileDialog.xaml
	/// </summary>
	public partial class FileDialog : Window
	{
		public static DependencyProperty FilepathProperty =
			DependencyProperty.Register(
			"Filepath",
			typeof( string ),
			typeof( FileDialog ),
			new PropertyMetadata( String.Empty ) );
		public string Filepath
		{
			get { return ( string ) GetValue( FilepathProperty ); }
			set { SetValue( FilepathProperty, value ); }
		}

		public static DependencyProperty QuestionProperty =
			DependencyProperty.Register(
			"Question",
			typeof( string ),
			typeof( FileDialog ),
			new PropertyMetadata( String.Empty ) );
		public string Question
		{
			get { return ( string ) GetValue( QuestionProperty ); }
			set { SetValue( QuestionProperty, value ); }
		}

		public enum FileDialogResult
		{
			Yes,
			No,
		}

		public FileDialogResult Result { get; set; }

		public FileDialog()
		{
			InitializeComponent();

			Result = FileDialogResult.No;

			btnYes.IsDefault = true;
			btnYes.Click += ( s, e ) => Done( FileDialogResult.Yes );

			btnNo.Click += ( s, e ) => Done( FileDialogResult.No );
		}

		void Done( FileDialogResult result )
		{
			Result = result;
			DialogResult = true;
			Close();
		}
	}
}
