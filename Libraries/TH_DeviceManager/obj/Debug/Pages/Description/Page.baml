   M S B A M L   `   `   `  ����A  =TH_WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null/$clr-namespace:TH_WPF;assembly=TH_WPFTH_WPF  5      Q5clr-namespace:TH_WPF.LoadingAnimation;assembly=TH_WPFTH_WPF.LoadingAnimation  5       TH_DeviceManager-   'TH_DeviceManager.Pages.Description.Page   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7th_wpf$clr-namespace:TH_WPF;assembly=TH_WPF     5      Rloadinganimation5clr-namespace:TH_WPF.LoadingAnimation;assembly=TH_WPF     5      .��5      � $��20q�6     � RowDefinitions  5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5      � $��30��6   $��0,0,0,0q�6d   $��Center=�6u   ��6   �� *ActualHeight+ �� RelativeSource �� *Self+ � 	Resources 5         IMG j� Stretch �� ImageSource��%i   &	        �� 5      $ Uniform=�6-   $8 2/TH_DeviceManager;component/Resources/About_01.png��6?   5      ��5      �� 5       �� 
TargetType" �6   .�5      ��  �� Property �@J�OpacityMask �� Value# [  68   �� 5       � ��  Foreground_Normal# [ 67   5       5"      �� 5$      $��Description��6   $
��20pt(�6+   $��30��6;   #��[ 6G   $��10,0,0,0q�6w   $��Center=�6�   �� 5&      $(��"General Device Desciption Settings��6   $
��10pt(�6B     Foreground_Light#��[ 6R   $
��20,5q�6�   $��Center=�6�   5(      �� 5*   
   $��1��6   $��1��6!     Shade_Normal#��[ 6,   $��0,15,0,0q�6Q   �� 5,   
   $��2��6   $
��Auto=�6$   .��5.      � 	 � ColumnDefinitions	 5/      �� 50      $
��auto �6'   �� 51      $
��auto �6'   �� 52      $��* �6'   .+�56      �� $
��Left=�6   $	��400��68    57         TH_WPF.TextBox��%�   '        �� 58      "  6    .�59      ��  � ��$
 0,10q�67   �� 5:       � ��$	 400��66   �� 5;       � ��$
 Left=�6D   5<      5=      .+�5@        -   5        
   Title$
 Device Description��5@   U      Instructions$! Enter description of device��6p      Text 6%   �� *DeviceDescription+ �� Mode$ TwoWay=�   HelpText$v pUsed to record a general desciption of the device. For example: 5 Axis Mill, Horizontal Lathe, 3 Axis Mill, etc.��5D   3   5E        5H      -   5        $
 Device Type��5H   N   $ Enter device type��6b   $	��200��6�    6%   �� *
DeviceType+$ TwoWay=�$P JUsed to record the type of device. For example: CNC, Press, Conveyor, etc.��5L   3   5M        5P      -   5        $
 Manufacturer��5P   P   $ Enter device manufacturer��6e    6%   �� *Manufacturer+$ TwoWay=�$U OUsed to record the device's manufacturer. For example: Okuma, Mazak, Haas, etc.��5T   3   5U        5X      -   5        $
 	Device Id��5X   L   $ Enter device id��6^   $	��200��6�    6%   �� *
DeviceId+$ TwoWay=�$� �Used to create an ID for the device that is different than other devices with similar Manufacturer, Model, etc. For example: 01, 02, A, B, C, etc.��5\   3   5]        5`      -   5        $
 Model��5`   I   $ Enter device model��6W    6%   �� *Model+$ TwoWay=�$( "Used to record the device's model.��5c   3   5d        5g      -   5        $
 Serial��5g   J   $  Enter device serial number��6Y    6%   �� *Serial+$ TwoWay=�$t nUsed to record what the serial number for the device is. Can be used to help differentiate from other devices.��5j   3   5k        5n      -   5        $
 
Controller��5n   N   $" Enter device controller type��6a    6%   �� *
Controller+$ TwoWay=�$s mUsed to record what type/brand of controller the device has. For example: Fanuc, Mazatrol, OPC, Siemens, etc.��5r   3   5s        5v      -   5        $
 Company��5v   K   $% Enter company device is used by��6[    6%   �� *	Company+$ TwoWay=�$� �Used to record the Company that this machine belongs to. For example if TrakHound is used by a Machine Service Provider and monitors many different company's machines.��5z   3   5{        5~      -	   5        $
 Location��5~   L      Example$ Plant B or Cell 14��6]    6%   �� *
Location+$ TwoWay=�$I CUsed to record the a note about the physical location of the device��5�   3   5�      5�      �� 5�      $��1��6   $��	60,10,0,0q�6-   .+�5�         TH_WPF.ImageBox  -
   5           Title$ Manufacturer Logo��5�   &      HelpText$ Upload a logo��6@   $	��150��5�   "   $	��200��5�   "      Image 5�   "   �� *ManufacturerLogo+   ImageSet 5�   "   �� *ManufacturerLogoSet+   Loading 5�   "   �� *ManufacturerLogoLoading+  5�      -   5        $ Device Image��5�   &   $ Upload an image��6;   $	��200��5�   "   $	��200��5�   "    5�   "   �� *DeviceImage+ 5�   "   �� *DeviceImageSet+ 5�   "   �� *DeviceImageLoading+5�      5�      5�      5�      5�      