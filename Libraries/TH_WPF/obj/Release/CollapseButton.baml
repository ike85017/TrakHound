   M S B A M L   `   `   `  ���� 
  TH_WPF    TH_WPF.CollapseButton   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     
���   �.���� -     ��Name$  bd��._��  � RowDefinitions �� $
��auto ��� $
��auto �.+���  �� CornerRadius$ 2������  �� 
TargetType" ��.���  �� Property � ��   Accent_Normal �� Value# [   �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  �@�Cursor$
 Hand����  � ��  Accent_Light# [ ._�� ���� 	 �� EndPoint$	 0.5,1&�
 �� 
StartPoint$
 0.5,0&�.���  � Color$ 	#20ffffff�� � Offset$ 0Q�� $ 	#20ffffff��$	 0.3Q�� $ Transparent��$ 1Q� � ColumnDefinitions �� $��* ��� $
��auto �.+��  �� $
��auto ��� $��* �.+��� $��30��$��30��$
��10,5q�$	��Top=� J� RenderTransformOrigin$ 0.5, 0.5&� � 	Resources   
Expand_IMG j� Stretch �� ImageSource��%M   &	       �� $ Uniform=�$/ )/TH_WPF;component/Resources/Expand_01.png��z��� ���� " ��.���  �@J�OpacityMask# [ ��  � ��  Foreground_Opposite_Normal# [  ��  �� Binding �� *
IsExpanded+ �� Value true N� EnterActions �� .���� .��W�  �� TargetProperty$9 3(Rectangle.RenderTransform).(RotateTransform.Angle)� W� From$ 0X�	 W� To$
 -180X� g� Duration$ 0:0:0.2D� N� ExitActions �� .���� .��W� $9 3(Rectangle.RenderTransform).(RotateTransform.Angle)�$
 -180X�$ 0X�$ 0:0:0.2D��� $��1��$��14(�#��[ $��Center=����� *Text+���� " ��.���  � y�$ Visible=� ��  �� *Text+  .a���  � y�$ 	Collapsed=���  �� *Text+ Z� .a���  � y�$ 	Collapsed=�� -    �Name$ HitCatcher_GRID��$��2�����	#00000000�� -    ��Name$ ButtonContent_CC��$��1��$
��10,0q�$��Center=����� *ButtonContent+� $��1������ " �.���  � ��$ 0�� ��  �� *
IsExpanded+ true �� .���� .��W� $ 	00:00:0.2D�$ 	MaxHeight�$ 0X�$
 1200X� W� EasingFunction .  (System.Windows.Media.Animation.CubicEase  7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode$ EaseOut=� �� .���� .��W� $ 	00:00:0.2D�$ 	MaxHeight�$
 1200X�$ 0X�   $ EaseIn=�.+��� $��20q����� *PageContent+