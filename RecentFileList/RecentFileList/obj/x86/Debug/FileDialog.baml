   M S B A M L   `   `   `  ����  RecentFileList    RecentFileList.FileDialog   5   	   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      -   5        $��Window��5        � ShowInTaskbar  .   � WindowStartupLocation$ CenterScreen=�5       � Title$ 
FileDialog��5      $	��300��5      $	��600��6   .��5
      �  � RowDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��Center=�6V   .+�5      �� $��Filepath:    ��6   �� 5      -   5        $��	_Filepath��5      ��6'   ��  �� ElementName$ Window�� �� Path$ Filepath�5      �� 5   
   -   5        $��txtQuestion��5   -   $��1��6   $��20q�6!   $��Center=�6s   $��Center=�6�   ��6@   �� $ Window��$ Question�6�   �� 5   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   $��Center=�6V    � 	Resources 5       �� 
TargetType	 �� Property
 �� Value��%v   '��       �� 5      " ��6   .�5      �� 	 � ��$
 10q�6/   �� 5       	 � ��$
 50��6.   5!      5"      .+�5$      �� -   5        $��btnYes��5$      $	��Yes��6#   �� 5%      -   5        $��btnNo��5%      $��No��6"   5'      5)      5+      