   M S B A M L   `   `   `  ����J  FReactorControl, Version=25.6.1.1, Culture=neutral, PublicKeyToken=null;"clr-namespace:ReactorControl.PopupReactorControl.Popup  5   	   G CProcessValue, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:ProcessValue;assembly=ProcessValueProcessValue 5   	   ,    &ReactorControl.Popup.Popup_pumpControl   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:ReactorControl.Popup     5   	   IProcessValue0clr-namespace:ProcessValue;assembly=ProcessValue     5   	     � Title$  Popup_ProcessControl1��5	   	   $	��280��6'   $	��300��64    � WindowStartupLocation$ CenterScreen=�5
   	    � WindowStyle$
 None=�6.    � AllowsTransparency . ���#999999$��16(�5      .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   .+�5   
   � $��0��6   ���#AAAAAA.+�5      �� $��펌프 제어��6   $��Center=�6%   $��Center=�6@   $��16(�6]   �� 5      -   5        $��X��5      $��25��65   $��25��6@   $��Right=�6L   $��Center=�6h   $��0,0,0,0q�6�   5      � 5   
   $��1��6   ���#CCCCCC.+�5       �� $��
장치명:��6   $
��Left=�6#   $��35��6>   $��Center=�6J   $
��Bold"�6e   $��16(�6w   �� 5!      $��PP-001��6   $
��Left=�6%   $��35��6@   $��Center=�6L   $��60,0,0,0q�6g   $��16(�6y   5"      � 5#   
   $��2��6   ���#CCCCCC.+�5$      �� $��현재상태:��6   $
��Left=�6$   $��35��6?   $��Center=�6K   $
��Bold"�6f   $��16(�6x   �� 5%      $	��RUN��6   $
��Left=�6"   $��35��6=   $��Center=�6I   $��75,0,0,0q�6d   ���#555555$��16(�6�   5&      � 5'   
   $��3��6   ���#CCCCCC.+�5(      �� $��통신상태:��6   $
��Left=�6$   $��35��6?   $��Center=�6K   $
��Bold"�6f   $��16(�6x   �� 5)      -   5        $��lbl_server_request��5)      $��요청��6.   $
��Left=�6;   $��35��6V   $��Center=�6b   $��70,0,0,0q�6}   ���#aaaaaa$��16(�6�   �� 5*      -   5        $��lbl_server_receive��5*      $��->응답��6.   $
��Left=�6@   $��35��6[   $��Center=�6g   $��	110,0,0,0q�6�   ���#aaaaaa$��16(�6�   �� 5+      -   5        $��lbl_server_process��5+      $��->처리��6.   $
��Left=�6@   $��35��6[   $��Center=�6g   $��	160,0,0,0q�6�   ���#aaaaaa$��16(�6�   �� 5,      -   5        $��lbl_server_result��5,      $��->결과��6-   $
��Left=�6?   $��35��6Z   $��Center=�6f   $��	210,0,0,0q�6�   ���#aaaaaa$��16(�6�   5.      � 5/   
   $��4��6   ���#CCCCCC.+�50      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Center=�6O   $��0q�6j   .+�51      �� -   5        $��BtnOpen��51      $	��RUN��6(   $��80��6L   $��30��6W   $��5q�6c   �� 52      -   5        $��BtnClose��52      $
��STOP��6)   $��80��6O   $��30��6Z   $��5q�6f   54      55      � 56   
   $��5��6   ���#CCCCCC57      � 58   
   $��6��6   ���#CCCCCC.+�59      �� $��상태:��6   $
��Left=�6"   $��35��6=   $��Center=�6I   $
��Bold"�6d   $��16(�6v   �� 5:      $'��!펌프 제어 대기 중입니다��6   $
��Left=�6,   $��35��6G   $��Center=�6S   $��45,0,0,0q�6n   ���#333333$��16(�6�   5;      5<      5=      