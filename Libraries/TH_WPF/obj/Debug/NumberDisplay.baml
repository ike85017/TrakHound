   M S B A M L   `   `   `  ����
  TH_WPF    TH_WPF.NumberDisplay   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      QiceAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options     5      .��5      � -   5          �Name$
  root��5      ��6   �� *
Background+.+�5   
   � $��Center=�6   $
��0,10q�6,    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��auto �6#   �� 5      $��* �6#   .+�5      � $��0��6   $��Center=�6#   $	��5,0q�6@    � RowDefinitions 5      �� 5      $��* �6$   �� 5      $��* �6$   �� 5      $��* �6$   .+�5      �� $��0��6   $��10(�6I    J� Opacity$
 0.25Q�6y      
RobotoFont#��[  5      ��5   )   �� *PreviousValue3+��6W   �� *
Foreground+5      �� 5!      $��1��6   $��10(�6I   $	 0.5Q�6y   #��[  5"      ��5!   )   �� *PreviousValue2+��6W   �� *
Foreground+5"      �� 5%      $��2��6   $��10(�6I   $
 0.75Q�6y   #��[  5&      ��5%   )   �� *PreviousValue1+��6W   �� *
Foreground+5&      5'      �� 5*      $��1��6   $��24(�6C   $��Center=�5,      $��Right=�6/   #��[  5-      $	��5,0q�5.      ��5*   (   �� *	ValueText+��5+      �� *
Foreground+5.      �� 52      $��2��6   $��30��6(   $��30��63   $	��5,0q�6?   $��Center=�6L   $��Center=�6i    � 	Resources 53       
 Freeze  UP_Arrow_IMG j� Stretch �� ImageSource  DOWN_Arrow_IMG��%�   &	       &	 m     �� 54      4True 6Z   $ Uniform=�6H   $8 2/TH_WPF;component/Resources/Carrot_Arrow_Up_02.png��6Z   �� 55      4True 6\   $ Uniform=�6J   $: 4/TH_WPF;component/Resources/Carrot_Arrow_Down_02.png��6\   56      ��58      �� 59       �� 
TargetType" ��6   .�5;      ��  �� Property � ��	 �� Value	 61   �� *
ArrowBrush+�� 5<       � y�$	 	Collapsed=�67   
 �� Triggers
 5=      �� 5>       �� Binding 6*   �� *ValueIncreasing+ �� Value true6N   .a�5?   "   ��  �@J�OpacityMask#	 [ 6@   �� 5A   "    � y�$	 Visible=�6?   5B      �� 5C       6*   �� *ValueDecreasing+ true6N   .a�5D   "   ��  �@J�OpacityMask#	 [ 6@   �� 5F   "    � y�$	 Visible=�6?   5G      5I      5K      5M      5O      5Q      