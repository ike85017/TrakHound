   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ComboBoxToggleButton2  �� 
TargetType � ColumnDefinitions ��Name �� CornerRadius F�Name �� Triggers P� Property P� Value �� 
TargetName	 �� Property
 �� Value  ComboBoxTextBox2 �� 
TargetType �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight K� IsItemsHost-    'System.Windows.Input.KeyboardNavigation    DirectionalNavigation P� 
SourceName �� Triggers�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %c  &	        &	 �    '�� V    7   7  �� 5      "  d�64   .R�5   
   �  5      �� 5      �� 5      $��20 �6#   .+�5
      �� $ Border��5      $��1��5      $ 0��5      ���#0080ff���#fff$��0q�5      F� 5      $ Arrow��5      $��1��5      ���#fff$��Center=�5      $��Center=�5      =����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	5      5       5   
   P� 5       �@J�IsMouseOver . . �5      �� $ Border��6   	 � ��
 �#00aaff6C   5      P� 5       �@d�	IsChecked$
 trueY�68   . �5      �� $ Border��6   	 � ��
 �#0070ff6C   5       P� 5!       � {� .  . �5"      �� $ Border��6   	 � ��
 �#0070ff6C   5&      5(      �� 5*      "  ��6/   .R�5+   
   �� $ PART_ContentHost��6   |�.  #��z��6=   5,      �� 5/      " ��6&   .�50   
   �� 	 �@J�SnapsToDevicePixels
 . 60   �� 51   
   	 �@�OverridesDefaultStyle
 . 62   �� 52   
   	 � ��$

 Auto=�6G   �� 53   
   	 � ��$

 Auto=�6E   �� 54   
   	 � ��
 . 6:   �� 58   
   	 � ��
 59      �� 5:      "  ��6"   .R�5;      � .+�5<      d� $��ToggleButton��5=      8��  $��2��5?      |�.  $ Press=�5B       5A      �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5C      �� 5D      $��ContentSite��6+    .  #��z 6X   #��z 5E      #��z��5F      $��5,5,5,5q�5G      $��Center=�5H      $��Stretch=�5I      �� 5J      $ PART_EditableTextBox��6"   8�� $��Stretch=�5M      $��Center=�5N      $��5,5,5,5q�5O      |�. ���#fff���#333$y�Hidden=�5S      # z 5T      ��5K      Z� 5T      � 5U      $��Popup��5V      $��Bottom=�5W      #��z 5X       . |�.  $��Slide=�5[      .��5]      � $��DropDown��6#    . #��z 5_      #��z 5`      .+�5a   "   �� $ DropDownBorder��5b   !   ���#fff5c   !   �� 5e   "   $��4,6,4,6q�6/    . .��5f   &   ��  . $ 	Contained=�6D   5g   #   5h      5i      5j       5k      P� 5l       �@��HasItems .  . �5m      �� $ DropDownBorder��6%   	 � ��$
 95��6V   5n      P� 5o       � {� .  . �5p      �� 	 � ��
 �#8886;   5q      P� 5r       �@��
IsGrouping . . �5s      �� 	 � ��
 .  6N   5t      P� 5u      $ Popup��6"    �@�AllowsTransparency . . �5v      �� $ DropDownBorder��6%   	 �@��CornerRadius$
 0��6Y   �� 5w      $ DropDownBorder��6%   	 � ��$
 0,2,0,0q�6S   5x      P� 5y       �@��
IsEditable . . �5z      �� 	 � ��
 .  6:   �� 5{      $ PART_EditableTextBox��6%   	 � y�$
 Visible=�6]   �� 5|      $ ContentSite��6%   	 � y�$
 Hidden=�6T   5}      5      5�       5�   
   5�      5�      