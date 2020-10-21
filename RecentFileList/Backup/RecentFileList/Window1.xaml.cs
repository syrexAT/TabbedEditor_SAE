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
using System.Windows.Navigation;
using System.IO;
using Microsoft.Win32;

namespace RecentFileList
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		const string NoFile = "No file";
		const string NewFile = "New nameless file";

		public static DependencyProperty FilepathProperty =
			DependencyProperty.Register(
			"Filepath",
			typeof( string ),
			typeof( Window1 ),
			new PropertyMetadata( NoFile ) );
		public string Filepath
		{
			get { return ( string ) GetValue( FilepathProperty ); }
			set { SetValue( FilepathProperty, value ); }
		}

		bool _IsFileNamed = false;
		bool _IsFileLoaded = false;
		MemoryStream _MemoryStream = new MemoryStream();

		public Window1()
		{
			InitializeComponent();

			AddCommandBindings();

			//RecentFileList.UseXmlPersister();
			//RecentFileList.UseXmlPersister( _MemoryStream );

			RecentFileList.MenuClick += ( s, e ) => FileOpenCore( e.Filepath );
		}

		void AddCommandBindings()
		{
			CommandBindings.Add( new CommandBinding( ApplicationCommands.New, ( target, e ) => FileNew() ) );
			CommandBindings.Add( new CommandBinding( ApplicationCommands.Open, ( target, e ) => FileOpen() ) );
			CommandBindings.Add( new CommandBinding( ApplicationCommands.Save, ( target, e ) => FileSave(), ( s, e ) => e.CanExecute = _IsFileLoaded ) );
			CommandBindings.Add( new CommandBinding( ApplicationCommands.SaveAs, ( target, e ) => FileSaveAs(), ( s, e ) => e.CanExecute = _IsFileLoaded ) );
			CommandBindings.Add( new CommandBinding( ApplicationCommands.Close, ( target, e ) => FileClose(), ( s, e ) => e.CanExecute = _IsFileLoaded ) );
		}

		private void Exit( object sender, RoutedEventArgs e )
		{
			Close();
		}

		void FileNew()
		{
			Filepath = NewFile;

			_IsFileLoaded = true;
			_IsFileNamed = false;
		}

		bool FileOpen()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "All Files ( *.* )|*.*";
			if ( true != dlg.ShowDialog( this ) ) return false;
			return FileOpenCore( dlg.FileName );
		}

		bool FileSave()
		{
			if ( !_IsFileLoaded ) return false;
			if ( !_IsFileNamed ) return FileSaveAs();

			return FileSaveCore( Filepath );
		}

		bool FileSaveAs()
		{
			if ( !_IsFileLoaded ) return false;

			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "All Files [ *.* ]|*.*";
			dlg.FileName = _IsFileNamed ? Filepath :
				Path.Combine(
					Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ),
					"NewFile.extension" );
			if ( true != dlg.ShowDialog( this ) ) return false;
			return FileSaveCore( dlg.FileName );
		}

		void FileClose()
		{
			_IsFileLoaded = false;
			_IsFileNamed = false;
			Filepath = NoFile;
		}

		bool FileOpenCore( string filepath )
		{
			FileDialog dlg = new FileDialog();
			dlg.Title = "Open file";
			dlg.Filepath = filepath;
			dlg.Question = "Did the open file operation succeed?";
			if ( true != dlg.ShowDialog() ) return false;

			if ( dlg.Result == FileDialog.FileDialogResult.Yes )
			{
				RecentFileList.InsertFile( filepath );
				Filepath = filepath;
				_IsFileLoaded = true;
				_IsFileNamed = true;
				return true;
			}

			if ( MessageBoxResult.Yes == MessageBox.Show( "Do you want to remove this file from the recent file list?", "Failed to open file", MessageBoxButton.YesNo, MessageBoxImage.Question ) )
				RecentFileList.RemoveFile( filepath );

			return false;
		}

		bool FileSaveCore( string filepath )
		{
			FileDialog dlg = new FileDialog();
			dlg.Title = "Save file";
			dlg.Filepath = filepath;
			dlg.Question = "Did the save file operation succeed?";
			if ( true != dlg.ShowDialog() ) return false;

			if ( dlg.Result == FileDialog.FileDialogResult.Yes )
			{
				RecentFileList.InsertFile( filepath );
				Filepath = filepath;
				_IsFileNamed = true;
				return true;
			}

			return false;
		}
	}
}
