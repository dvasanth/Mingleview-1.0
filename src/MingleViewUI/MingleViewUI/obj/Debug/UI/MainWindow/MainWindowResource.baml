   M S B A M L   `   `   `  ����E  AMingleView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MingleView.UI.ViewModelMingleView.UI.ViewModel  5      7 clr-namespace:MingleView.UI.ViewMingleView.UI.View  5      K*clr-namespace:MingleView.UI.ProgressWindowMingleView.UI.ProgressWindow  5      T PAvalonDock, Version=1.3.3585.0, Culture=neutral, PublicKeyToken=85a1e0ada7ec13e4;,clr-namespace:AvalonDock;assembly=AvalonDock
AvalonDock 5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35    BooleanToVisibilityConverter ! MingleViewTitleMenuBackground  �� 
StartPoint �� EndPoint � Color � Offset  EllipseStyle �� 
TargetType �� Property �� Value �� ScaleX �� ScaleY	 �� CenterX
 �� CenterY4    .MingleView.UI.ProgressWindow.ProgressIndicator �� TargetProperty O� KeyTime O� Value �� 
TargetType �� 	Converter � RowDefinitions ��Name J� SnapsToDevicePixels    LoadingText    ProgressValue�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    5      ,vm%clr-namespace:MingleView.UI.ViewModel  5      'vw clr-namespace:MingleView.UI.View  5      ;progressview*clr-namespace:MingleView.UI.ProgressWindow  5      3ad,clr-namespace:AvalonDock;assembly=AvalonDock  5      %	  &	        &	      &	 �     '   �    7   7  7  7  7  7  7  7  7  �� 5      6#   �� 5      $  0.5,0&�6@   $ 0.5,1&�6S   .��5   
   � $ #F4F4F4��6   $ 0Q�6'   � 5   
   $ #CACACA��6   $	 0.5Q�6'   � 5   
   $ White��6   $ 1Q�6%   5      �� 5      " A�6!   �5   
   �� 5       � ��$ 15��6&   �� 5       � ��$ 15��6'   �� 5       � �� 5      
� 5      .��5      � $ #1293F1��6'   $ 0.634Q�67   � 5      $ White��6'   $ 1Q�65   � 5      $ #63AAEE��6'   $
 0.33Q�67   � 5      $ #44BBFF��6'   $ 0.062Q�67   5      5      �� 5        � z� 5!      �� 5"      $ 0Q�5#      $ 0Q�6$   $		 7.5Q�5$      $	
 7.5Q�6'   5&      5(      �� 5*      "   6   �5+   
   �� 5,       � ��$
 Auto��6'   �� 5-       � ��$
 Auto��6&   �� 5.       �@  ElementStoryboard 5/      �� 50      .��51      U� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleX)�68   .\�52      �� $ 00:00:00��63    06F   �� 53      $ 
00:00:00.5��63    0.56H   �� 54      $ 00:00:01��63    16F   �� 55      $ 
00:00:01.5��63    1.56H   56      U� 57      $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�68   .\�58      �� $ 00:00:00��63    06F   �� 59      $ 
00:00:00.5��63    0.56H   �� 5:      $ 00:00:01��63    16F   �� 5;      $ 
00:00:01.5��63    1.56H   5<      5=      5?      �� 5@       � �� 5A      �� 5B      "   6&   .R�5C      � y�6   �� *	IsRunning+8    5D      �� 5E   "   $
��Auto �60   �� 5F   "   $��20 �60   �� 5G   "   $
��Auto �60   .+�5I      �� $ PART_Canvas��5J   !   $��0��5I   %    . $��80��5L   !   $��80��5M   !   .+�5N   "   A� 8�� 6*   A� 5O   "   8�� 6*   A� 5P   "   8�� 6*   A� 5Q   "   8�� 6*   A� 5R   "   8�� 6*   A� 5S   "   8�� 6*   A� 5T   "   8�� 6*   A� 5U   "   8�� 6*   5V      �� 5X      $��LoadingTextBlock��5Y   !   $��2��5X   )   #��z 5Z   !   �� 5\      $��ProgressTextBlock��5]   !   $��0��5\   (   #��z 5^   !   $��Center=�5_   !   $��Center=�5`   !   5a      5b      5d      5f      5h      