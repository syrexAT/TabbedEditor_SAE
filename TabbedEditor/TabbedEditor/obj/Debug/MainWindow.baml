   M S B A M L   `   `   `  ����G  CTabbedEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:TabbedEditorTabbedEditor  5   	       TabbedEditor.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:TabbedEditor     5   	   -   5          � Title$  Tabbed Editor��5   	   $	��450��6   $	��800��6,    J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$
 Open��5
      6(     5   
   -   5        $
 Save��5      6(     5   
   -   5        $ SaveAs��5      6*   .��5      ]� .+�5   
   o� $	��Top=�6   .@�5      m� $
��File��6   .@�5      m� $��Open...��6    m� Command$
 Open��6,   m� 5      $
��Save��6   $
 Save��6)   m� 5      $��
Save as...��6   $ SaveAs��6/   �� 5      m� 5      -   5        $��RecentFileList��5      $��Recent Files��61   6I   5      m� 5      $
��Help��6   .@�5      m� -   5        $��	View Help��5      6.   5      5      ]� 5   
   .+�5      �� -   5        $��	InfoPanel��5      $��Right=�6*   $��0��6A   5      �� 5      -   5        $��TabController��5      $	��Top=�6.   5       5!      5"      5#      