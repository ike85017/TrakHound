   M S B A M L   `   `   `  ���� 
  TH_WPF    TH_WPF.NumberDisplay   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     QiceAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options     .��� -     �Name$
  root������ *
Background+.+�� $��Center=�$
��0,10q� � ColumnDefinitions �� $��* ��� $
��auto ��� $��* �.+�� $��0��$��Center=�$	��5,0q� � RowDefinitions �� $��* ��� $��* ��� $��* �.+��� $��0��$��10(� J� Opacity$
 0.25Q�   
RobotoFont#��[  ���� *PreviousValue3+���� *
Foreground+�� $��1��$��10(�$	 0.5Q�#��[  ���� *PreviousValue2+���� *
Foreground+�� $��2��$��10(�$
 0.75Q�#��[  ���� *PreviousValue1+���� *
Foreground+�� $��1��$��24(�$��Center=�$��Right=�#��[  $	��5,0q����� *	ValueText+���� *
Foreground+�� $��2��$��30��$��30��$	��5,0q�$��Center=�$��Center=� � 	Resources  
 Freeze  UP_Arrow_IMG j� Stretch �� ImageSource  DOWN_Arrow_IMG��%�   &	       &	 U     �� 4True $ Uniform=�$8 2/TH_WPF;component/Resources/Carrot_Arrow_Up_02.png���� 4True $ Uniform=�$: 4/TH_WPF;component/Resources/Carrot_Arrow_Down_02.png������  �� 
TargetType" ��.���  �� Property � ��	 �� Value	 �� *
ArrowBrush+��  � y�$	 	Collapsed=�
 �� Triggers
 ��  �� Binding �� *ValueIncreasing+ �� Value true.a���  �@J�OpacityMask#	 [ ��  � y�$	 Visible=���  �� *ValueDecreasing+ true.a���  �@J�OpacityMask#	 [ ��  � y�$	 Visible=�