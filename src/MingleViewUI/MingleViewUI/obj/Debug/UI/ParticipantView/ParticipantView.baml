   M S B A M L   `   `   `  ����E  AMingleView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MingleView.UI.ViewModelMingleView.UI.ViewModel  5      (    "MingleView.UI.View.ParticipantView   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      2vm%clr-namespace:MingleView.UI.ViewModel    5      $
��Auto��5      $
��Auto��6   .��5      � $
��Auto��6   $
��Auto��6     � 	Resources  5   
   ,   &MingleView.UI.ViewModel.ImageConverter   IconConverter  CustomHeaderStyle �� 
TargetType �� Property �� Value �� EndPoint �� 
StartPoint � Color � Offset��%�  &	        &	        5      6    �� 5
      " ��6.   .�5      ��  � �� �	#FFCDE7F46/   �� 5       � �� �	#FF07034B6/   �� 5       � �� 5      �� 5      $ 0.5,1&�6.   $ 0.5,0&�6?   .��5      � $ 	#FFA1CCE0��6+   $ 0Q�6=   � 5      $ 	#FFABD5EA��6+   $	 1.0Q�6=   5      5      �� 5       � ��$ 20��6+   5      5       � RowDefinitions 5   
   �� 5      $	��42* �6   .+�5'   
   �� $��0,0,0,0q�6A   $
��Auto��5(      $
��Auto��6   $��0��6+   $��Stretch=�68   ��5'      �� *ParticipantCollection+	 �� View	 5)      �� 5*      !
 �� ColumnHeaderContainerStyle#
 �  6   .P�5+      �� $��	All Users��6%    �� Width$	 400��68    �� CellTemplate 5,      �� 5-      .R�5.   "   ��  �� Orientation$ 
Horizontal=�6-   $��Center=�6F   $��Center=�6a   .+�5/   &   �� $��16��6s   $��16��6   ��6,   ��  �� Path$
 Icon� �� 	Converter# [  6   �� 50   &   $��  ��60   �� 51   &    �� TextAlignment$ Right=�6F   ��60   �� *Name+6F   52   #   53      55      56      58      5;      5<      