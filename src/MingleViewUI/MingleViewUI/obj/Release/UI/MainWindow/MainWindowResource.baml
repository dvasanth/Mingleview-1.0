   M S B A M L   `   `   `  ���� E  AMingleView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MingleView.UI.ViewModelMingleView.UI.ViewModel  7 clr-namespace:MingleView.UI.ViewMingleView.UI.View  K*clr-namespace:MingleView.UI.ProgressWindowMingleView.UI.ProgressWindow  T PAvalonDock, Version=1.3.3585.0, Culture=neutral, PublicKeyToken=85a1e0ada7ec13e4;,clr-namespace:AvalonDock;assembly=AvalonDock
AvalonDock \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35    BooleanToVisibilityConverter ! MingleViewTitleMenuBackground  �� 
StartPoint �� EndPoint � Color � Offset  EllipseStyle �� 
TargetType �� Property �� Value �� ScaleX �� ScaleY	 �� CenterX
 �� CenterY4    .MingleView.UI.ProgressWindow.ProgressIndicator �� TargetProperty O� KeyTime O� Value �� 
TargetType �� 	Converter � RowDefinitions ��Name J� SnapsToDevicePixels    LoadingText    ProgressValue�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    ,vm%clr-namespace:MingleView.UI.ViewModel  'vw clr-namespace:MingleView.UI.View  ;progressview*clr-namespace:MingleView.UI.ProgressWindow  3ad,clr-namespace:AvalonDock;assembly=AvalonDock  %  &	        &	      &	 v     '   x    7   7  7  7  7  7  7  7  7  �� �� $  0.5,0&�$ 0.5,1&�.��� $ #F4F4F4��$ 0Q�� $ #CACACA��$	 0.5Q�� $ White��$ 1Q��� " A����  � ��$ 15����  � ��$ 15����  � �� 
� .��� $ #1293F1��$ 0.634Q�� $ White��$ 1Q�� $ #63AAEE��$
 0.33Q�� $ #44BBFF��$ 0.062Q���  � z� �� $ 0Q�$ 0Q�$		 7.5Q�$	
 7.5Q��� "   ���  � ��$
 Auto����  � ��$
 Auto����  �@  ElementStoryboard �� .��U� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleX)�.\��� $ 00:00:00�� 0�� $ 
00:00:00.5�� 0.5�� $ 00:00:01�� 1�� $ 
00:00:01.5�� 1.5U� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�.\��� $ 00:00:00�� 0�� $ 
00:00:00.5�� 0.5�� $ 00:00:01�� 1�� $ 
00:00:01.5�� 1.5��  � �� �� "   .R�� y��� *	IsRunning+8    �� $
��Auto ��� $��20 ��� $
��Auto �.+��� $ PART_Canvas��$��0�� . $��80��$��80��.+�A� 8�� A� 8�� A� 8�� A� 8�� A� 8�� A� 8�� A� 8�� A� 8�� �� $��LoadingTextBlock��$��2��#��z �� $��ProgressTextBlock��$��0��#��z $��Center=�$��Center=�