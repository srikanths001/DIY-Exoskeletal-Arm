   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   R NArduino_SerialInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Arduino_SerialInterfaceArduino_SerialInterface 5   	   (   "Arduino_SerialInterface.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   7local%clr-namespace:Arduino_SerialInterface     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��600��6)    � 	Resources 5         
enumValues S� 
MethodName S� 
ObjectType   System.Enum S� MethodParameters M� TypeName��%   &	        S� 5   
   $ 	GetValues��5	      "  5
       5      M� 5      $ local:MotorSpeed��6   5      5       � DataContext 5      &  Arduino_SerialInterface.DataItem  5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��300 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��Stretch=�6%   .+�5      �  5      �� 5      $��* �6'   �� 5      $��* �6'    � RowDefinitions 5      �� 5      $��* �6$   �� 5       $��* �6$   �� 5!      $��* �6$   .+�5#      �� $��Speed��6   $��0��6)   $��0��66   $��
5,93,5,-93q�6F   �� 5$      $	��100��6   $��0��6(   $��1��65   $��5,93,45,-93q�6E   ��6Z   �� 	 �� Source#	 [  
 �� SelectedIndex
 6�   ��  �� Path$ SelectedSpeed��� 5%      $��Number of turns��6   $��1��64   $��0��6A   $��
0,44,0,-44q�6Q   �� 5&      $��1��6   $��2��6*   $��5,22,10,-22q�67   ��6L   �� $ TotalNumberOfTurns��� 5'      -   5        $
��Send��5'      $��2��6(   $��
0,93,0,-93q�65   6I   5)      5*      �� 5+   
   $��1��6   $��Stretch=�6%   .+�5,      �  5-      �� 5.      $��* �6'   �� 5/      $��* �6'   .+�51      �� $��Number of turns��6   $��0��63   $��15,93,131,-93q�6C   $��2��6Z   �� 52      -   5         ��Name$ TestName��52      $��1��6,   $��36,93,14,-93q�6<    �� 
IsReadOnly . ��6R   �� $ NumberOfTurnsCompleted� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   53      54      55      56      