   M S B A M L   `   `   `  ���� E  AGetInfoGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:GetInfoGUI
GetInfoGUI      GetInfoGUI.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:GetInfoGUI       � Title$  
MainWindow��$	��350��$	��410�� � 	Resources    DataTemplate1 J� Opacity � RowDefinitions � ColumnDefinitions ��Name��%�  &	        �� .R�� $��42.59��$��Stretch=�
�������$��366.517��$	 0.9Q� �� ��  �� �� �� .+��� $ label��$��0��$��0��$��
User name:��$
��Left=�$��0q�$	��Top=�$��2q��� $ label1��$��1��$��0��$��Pc name:��$��0,0,50.84,0q�$	��Top=�$��Stretch=�$��2q��� $ label2��$��2��$��0��$��
IP addres:��$��Right=�$��-0.001,0,60.589,0q�$	��Top=�$��2q��� $ 	text_user��$��0��$��1��$
��Left=�$��0,0.335,0,0q�$	��Top=�$��2q����� *Name+�� $ text_pc��$��1��$��1��$
��Left=�$��0,-0.335,0,0q�$	��Top=�$��2q����� *PC+�� $ text_ip��$��2��$��1��$
��Left=�$��-0.001,-0.335,0,0q�$	��Top=�$��2q����� *IP+.��� -    �Name$ g1��.+��� -    ��Name$ button��$��Опросить сеть��$��
10,0,10,10q�$��Bottom=��� -    ��Name$ text_ip��$��23��$��10,47.235,10,0q�	 �� TextWrapping$
	 Wrap=�$	��Top=��� -   
 ��Name$
 listBox��$��10,75.235,10,34.96q�#���   