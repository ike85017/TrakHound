   M S B A M L   `   `   `  ����A  =TH_WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullclr-namespace:TH_WPFTH_WPF  5   	       TH_WPF.MessageBox   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   'th_wpfclr-namespace:TH_WPF     5   	     � WindowStartupLocation$  CenterScreen=�5   	   $	��200��5
   	   $	��400��6    � Icon$3 -Resources/TrakHound_Logo_Initials_10_30px.png��6"   $	��600��6W    � Title 5	   	   �� *WindowTitle+.��5      �    Window_Background#��[  6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��auto �6   .+�5   
   � $��20,10q�6   $��Center=�6    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5      $��* �6#   .+�5      �� $��40��6   $��40��6#   $��0,0,20,0q�6/   $��Center=�6A   $��Center=�6\    � 	Resources 5        IMG j� Stretch �� ImageSource��%C   &	       �� 5      $ Uniform=�6-    6?   �� *Image+5      ��5      �� 5        �� 
TargetType" ��6   .�5!      �� 	 �� Property	 �@J�OpacityMask
 �� Value#
 [ 68   �� 5"      	 � ��  Accent_Normal#
 [ 61    �� Triggers 5#      �� 5$       �� Binding 6*   �� *Image+ �� Value 6D   Z� .a�5%   "   �� 	 � y�$
 	Collapsed=�6?   5&      5(      5*      �� 5-      $��1��6     Foreground_Normal#��[ 6>   $
��12pt(�6n    �� TextAlignment$ Center=�6~   $��Center=�6�   $
��Wrap=�6�   ��6(   �� *Text+6�   5/      � 53   
   $��1��6   $
��0,20q�6    54         TH_WPF.Button  
Shade_Dark P� Property P� Value  Accent_Hover��%  '        7  7  7  7  7  �� 55      "  6   .�56      �� 	 � ��8
   63   �� 57      	 �@ 
Background

 ���� 63   �� 58      	 �@ BorderBrush8
  64   �� 59      	 �@ BorderThickness$
 1q�68   �� 5:      	 � ��$
 14(�61   �� 5;      	 � ��$	
 100��61   �� 5<      	 � ��$
 40��6/    5=      P� 5>       �@J�IsMouseOver . . �5?      �� 	 � ��8
  6;   �� 5@      	 �@ 
Background8
  6;   �� 5A      	 �@ BorderBrush8
  6<   5B      5D      5E      .+�5H      � $��Center=�6   ��5I      �� 5J      " �6   .�5K      �� 	 � y�$
 	Collapsed=�67    5L      �� 5M         TH_WPF.MessageBoxButtons	   Ok# Z 6F    6*   �� *	Buttons+.a�5N   "   �� 	 � y�$
 Visible=�6?   5O      5Q      .+�5T           TH_WPF.Button   Clicked$ 
Ok_Clicked��6*      Text$ Ok��6    6*   5V      �� 5Y       �� Orientation$ 
Horizontal=�6   $��Center=�62   ��5Z      �� 5[      " ��6   .�5\      �� 	 � y�$
 	Collapsed=�67    5]      �� 5^         YesNo# Z 6F    6*   �� *	Buttons+.a�5_   "   �� 	 � y�$
 Visible=�6?   5`      5b      .+�5e        $ Yes_Clicked��6Z   $	 Yes��6    $��0,0,20,0q�6+   $��Center=�6=   6Z     5g      $ 
No_Clicked��6G   $ No��6    $��Center=�6*   6G   5i      �� 5l      $ 
Horizontal=�6   $��Center=�62   ��5m      �� 5n      " ��6   .�5o      �� 	 � y�$
 	Collapsed=�67    5p      �� 5q         YesNoCancel# Z 6F    6*   �� *	Buttons+.a�5r   "   �� 	 � y�$
 Visible=�6?   5s      5u      .+�5x        $ Yes_Clicked��6Z   $	 Yes��6    $��0,0,20,0q�6+   $��Center=�6=   6Z     5z      $ 
No_Clicked��6Y   $ No��6    $��0,0,20,0q�6*   $��Center=�6<   6Y     5|      $ Cancel_Clicked��6K   $ Cancel��6    $��Center=�6.   6K   5~      5�      5�      5�      