   M S B A M L   `   `   `  ����A  =TH_WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullclr-namespace:TH_WPFTH_WPF  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5          TH_WPF.TimeProgress   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'th_wpfclr-namespace:TH_WPF     5      9system&clr-namespace:System;assembly=mscorlib     5      -   5        $��10q�5
      .��5      �� -   5          ��Name$
  root��5      .+�5   
   �  � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��auto �6#   .+�5      �� .+�5      (� $��16��6   $
��Left=�6)   .��5      �� $��14(�66    �� TextAlignment$
 Left=�6D   ��6    �� *Text+��5      �� 5       �� 
TargetType" ��6$   .�5   "   ��  �� Property � �� �� Value 6?   �� *
Foreground+�� 5   "    � ��$ Normal"�6?    �� Triggers 5   "   �� 5    &    �� Binding 62   �� *
IsSelected+ �� Value true6Q   .a�5!   *   ��  � ��$ SemiBold"�6G   5"   '   5$      5&      5(      �� 5+      $��10(�62   ��6   �� *Time+��6@   �� *
Foreground+5-      �� 50      $��1��6   $��Center=�6(   $��Right=�6C   $
��10,0q�6_   $��20(�6�      
RobotoFont#��[  6�   ��6m   �� *
Percentage+��51      �� 52      " ��6   .�53      ��  � �� 67   �� *
Foreground+�� 54       � ��$ Normal"�67    55      �� 56       6*   �� *
IsSelected+ true6I   .a�57   "   ��  � ��$
 Bold"�6?   58      5:      5<      5>         TH_WPF.ProgressBar  5A   
   $��0,5,0,0q�6:   $��0q�6�   ��6   �� *	BarHeight+	   Value	 6K   �� *
BarValue+
   Maximum
 6f   �� *
BarMaximum+��6�   �� *
BarBrush+��6�   �� *BarBackgroundBrush+6�   5C      5E      